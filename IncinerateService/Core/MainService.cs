using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Base;
using NeuroIncinerate.Neuro;
using Diagnostics.Eventing;
using NeuroIncinerate;
using IncinerateService.API;
using System.ServiceModel;
using System.Diagnostics;
using System.Threading;
using IncinerateService.Core;
using NLog;

namespace IncinerateService.Core
{
#if DEBUG
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
#endif
    public class MainService : IIncinerateService, IDisposable
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        Dictionary<string, IStrategy> Strategies = new Dictionary<string, IStrategy>()
        {
            { "alert", new AlertStrategy(Log)},
            { "alarm", new AlarmStrategy(Log)},
            { "warning", new AlarmStrategy(Log)},
            { "terminate", new HitTerminateStrategy(1, Log)},
            { "terminate10", new HitTerminateStrategy(10, Log)}
        };
        ProcessEventCollector m_Collector = new ProcessEventCollector();
        GlobalHistory m_History = new GlobalHistory();
        AgentRegistry m_AgentRegistry = new AgentRegistry();
        IAgentStorage m_AgentStorage = new CachedAgentStorage();
        Thread m_ProcessingThread;

        public MainService()
        {
            Activate();
        }

        public void Dispose()
        {
            Deactivate();
        }

        private void Activate()
        {
            m_Collector.ActionOccurred += new Action<TraceEvent>(m_Collector_ActionOccurred);
            m_History.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(m_History_SnapshotReady);
            m_Collector.Start();
            m_ProcessingThread = new Thread(new ThreadStart(AttachToProcessing));
            m_ProcessingThread.Start();
        }

        private void Deactivate()
        {
            m_Collector.ActionOccurred -= new Action<TraceEvent>(m_Collector_ActionOccurred);
            m_History.SnapshotReady -= new EventHandler<SnapshotReadyEventArgs>(m_History_SnapshotReady);
            m_Collector.Stop();
            //m_ProcessingThread.Interrupt();
        }

        private void m_History_SnapshotReady(object sender, SnapshotReadyEventArgs e)
        {
            ICollection<Agent> newAgents = m_AgentRegistry.Handle(e.PID, e.Events);
            if (newAgents.Count > 0)
            {
                foreach (Agent agent in newAgents)
                {
                    Log.Info("Агент {0} успешно обучен", agent.Name);
                    m_AgentStorage.SaveAgent(agent.Name, agent);
                    Log.Info("Агент сохранен: {0}", agent.Name);
                    m_AgentRegistry.StopLearning(agent.Name);
                }
            }
        }

        private void m_Collector_ActionOccurred(TraceEvent obj)
        {
            m_History.Add(new WinPID(obj.ProcessID, obj.ProcessName), new ProcessAction(obj));
        }

        private void AttachToProcessing()
        {
            m_Collector.AttachToProcessing();
        }

        private void StartWatching(string name,
            string strategyRed, string strategyYellow,
            double p1, double p2)
        {
            Agent agent = m_AgentStorage.LoadAgent(name);
            if (agent == null)
            {
                return;
            }
            m_AgentRegistry.AddWatcher(agent,
                ParseStrategy(strategyRed), ParseStrategy(strategyYellow),
                p1, p2);
        }

        public void StartGuard(string name, string process, string strategyRed,
            string strategyYellow, double e1, double e2)
        {
            Agent agent = m_AgentStorage.LoadAgent(name);
            if (agent == null)
            {
                return;
            }
            m_AgentRegistry.AddGuardian(agent, process,
                ParseStrategy(strategyRed), ParseStrategy(strategyYellow),
                e1, e2);
        }

        public void StopGuard(string name)
        {
            if (m_AgentRegistry.StopGuard(name))
            {
                Console.WriteLine("Страж {0} остановлен", name);
            }
        }

        private IStrategy ParseStrategy(string strategyName)
        {
            if (Strategies.ContainsKey(strategyName))
            {
                return Strategies[strategyName];
            }
            return new DoNothingStrategy(Log);
        }

        private void StartLearning(LearningConfig learningConfig)
        {
            m_AgentRegistry.CreateLearningAgent(learningConfig);
        }

        public void AddLearningAgent(IList<int> pids, string name)
        {
            Log.Info("AddLearningAgent: {0}", name);
            Process[] processes = Process.GetProcesses();
            ISet<IPID> nativePids = new SortedSet<IPID>();
            ISet<IPID> foreignPids = new SortedSet<IPID>();
            foreach (Process p in processes)
            {
                if (pids.Contains(p.Id))
                {
                    nativePids.Add(new WinPID(p.Id, p.ProcessName));
                }
                else
                {
                    foreignPids.Add(new WinPID(p.Id, p.ProcessName));
                }
            }
            LearningConfig learningConfig = new LearningConfig(name, nativePids, foreignPids);
            StartLearning(learningConfig);
        }

        public IList<AgentInfo> GetAgents()
        {
            Log.Info("GetAgents");
            IList<AgentInfo> response = new List<AgentInfo>();

            ICollection<LearningAgent> learningAgents = m_AgentRegistry.GetLearningAgents();
            foreach (LearningAgent learningAgent in learningAgents)
            {
                response.Add(new AgentInfo { Name = learningAgent.Name, Status = "Learning" });
            }
            ICollection<WatchingAgentSession> watchingAgents = m_AgentRegistry.GetWatchingAgents();
            ISet<string> watchingAgentNames = new HashSet<string>();
            foreach (WatchingAgentSession watchingAgent in watchingAgents)
            {
                response.Add(new AgentInfo { Name = watchingAgent.AgentName, Status = "Watching" });
                watchingAgentNames.Add(watchingAgent.AgentName);
            }
            ICollection<GuardianAgentSession> guardianAgents = m_AgentRegistry.GetGuardianAgents();
            ISet<string> guardianAgentNames = new HashSet<string>();
            foreach (GuardianAgentSession guardianAgent in guardianAgents)
            {
                response.Add(new AgentInfo { Name = guardianAgent.Agent.Name, Status = "Guarding" });
                watchingAgentNames.Add(guardianAgent.Agent.Name);
            }
            foreach (string agentName in m_AgentStorage.GetAgentNames())
            {
                if (!watchingAgentNames.Contains(agentName))
                {
                    response.Add(new AgentInfo { Name = agentName, Status = "Ready" });
                }
            }
            return response;
        }

        public void Watch(string name, string strategyRed, string strategyYellow, double p1, double p2)
        {
            try
            {
                StartWatching(name, strategyRed, strategyYellow, p1, p2);
                Log.Info("Watch: {0} ({1}, {2}) [{3:0.00}, {4:0.00}]", name, strategyRed, strategyYellow, p1, p2);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void Guard(string name, string process, string strategyRed,
            string strategyYellow, double e1, double e2)
        {
            try
            {
                StartGuard(name, process, strategyRed, strategyYellow, e1, e2);
                Log.Info("Guard: {0} - {1} ({2}, {3}) [{4:0.00}, {5:0.00}]",
                    name, process, strategyRed, strategyYellow, e1, e2);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void Stop()
        {
        }

        public void RemoveLearningAgent(string name)
        {
            if (m_AgentRegistry.StopLearning(name))
            {
                Log.Info("Обучение агента {0} остановлено", name);
            }
        }

        public void StopWatch(string name)
        {
            if (m_AgentRegistry.StopWatch(name))
            {
                Log.Info("Режим наблюдения агента {0} был остановлен", name);
            }
        }
    }
}