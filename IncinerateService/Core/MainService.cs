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

namespace IncinerateService.Core
{
#if DEBUG
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, IncludeExceptionDetailInFaults = true)]
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
#endif
    public class MainService : IIncinerateService, IDisposable
    {
        Dictionary<string, IStrategy> Strategies = new Dictionary<string, IStrategy>()
        {
            { "alert", new AlertStrategy()},
            { "terminate10", new HitTerminateStrategy(10)}
        };
        State m_State;
        ProcessEventCollector m_Collector = new ProcessEventCollector();
        GlobalHistory m_History = new GlobalHistory();
        AgentRegistry m_AgentRegistry = new AgentRegistry();
        IAgentStorage m_AgentStorage = new CachedAgentStorage();

        public MainService()
        {
            m_State = new InitialState(m_Collector, m_History, m_AgentRegistry);
            m_State.Enable();
        }

        public void Dispose()
        {
            m_State.Disable();
        }

        private void StartWatching(string name, string strategyRed, string strategyYellow)
        {
            m_State.Disable();
            m_State = new WatchingState(name, m_Collector, m_History,
                m_AgentRegistry, m_AgentStorage,
                ParseStrategy(strategyRed), ParseStrategy(strategyYellow));
            m_State.Enable();
        }

        private IStrategy ParseStrategy(string strategyName)
        {
            if (Strategies.ContainsKey(strategyName))
            {
                return Strategies[strategyName];
            }
            return new DoNothingStrategy();
        }

        private void StartLearning(LearningConfig learningConfig)
        {
            m_State.Disable();
            LearningState newState = new LearningState(learningConfig, m_Collector, m_History, m_AgentRegistry, m_AgentStorage);
            newState.OnLearningCompleted += new Action<LearningState>(NewState_OnLearningCompleted);
            m_State = newState;
            m_State.Enable();
        }

        private void NewState_OnLearningCompleted(LearningState state)
        {
            state.OnLearningCompleted -= new Action<LearningState>(NewState_OnLearningCompleted); ;
            m_State.Disable();
            m_State = new InitialState(m_Collector, m_History, m_AgentRegistry);
            m_State.Enable();
        }

        public void AddLearningAgent(IList<int> pids, string name)
        {
            Console.WriteLine("AddLearningAgent: {0}", name);
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

        public IList<string> GetAgents()
        {
            Console.WriteLine("GetAgents");
            ICollection<LearningAgent> learningAgents = m_AgentRegistry.GetLearningAgents();
            IList<string> response = new List<string>();
            foreach (LearningAgent learningAgent in learningAgents)
            {
                response.Add(learningAgent.Name + " : " + "Learning");
            }
            foreach (string agentName in m_AgentStorage.GetAgentNames())
            {
                response.Add(agentName + " : " + "Ready");
            }
            return response;
        }

        public void Watch(string name, string strategyRed, string strategyYellow)
        {
            StartWatching(name, strategyRed, strategyYellow);
            Console.WriteLine("Watch: {0} ({1}, {2})", name, strategyRed, strategyYellow);
        }

        public void Stop()
        {
            m_State.Disable();
            m_State = new InitialState(m_Collector, m_History, m_AgentRegistry);
            m_State.Enable();
            Console.WriteLine("Stopped");
        }
    }

    abstract class State
    {
        protected ProcessEventCollector m_Collector;
        protected GlobalHistory m_History;
        protected AgentRegistry m_AgentRegistry;

        public State(ProcessEventCollector collector, GlobalHistory history, AgentRegistry agentRegistry)
        {
            m_Collector = collector;
            m_History = history;
            m_AgentRegistry = agentRegistry;
        }

        public abstract void Enable();
        public abstract void Disable();
    }

    class InitialState : State
    {
        public InitialState(ProcessEventCollector collector,
            GlobalHistory history,
            AgentRegistry agentRegistry)
            : base(collector, history, agentRegistry)
        {
        }

        public override void Enable() { }
        public override void Disable() { }
    }

    class LearningState : State
    {
        LearningConfig m_LearningConfig;
        IAgentStorage m_AgentStorage;

        public LearningState(
            LearningConfig learningConfig,
            ProcessEventCollector collector,
            GlobalHistory history,
            AgentRegistry agentRegistry,
            IAgentStorage agentStorage)
            : base(collector, history, agentRegistry)
        {
            m_LearningConfig = learningConfig;
            m_AgentStorage = agentStorage;
        }

        public override void Enable()
        {
            m_Collector.ActionOccurred += new Action<TraceEvent>(m_Collector_ActionOccurred);
            m_History.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(m_History_SnapshotReady);
            m_AgentRegistry.CreateLearningAgent(m_LearningConfig);
            m_Collector.Start();
            Thread thread = new Thread(new ThreadStart(AttachToProcessing));
            thread.Start();
        }

        private void AttachToProcessing()
        {
            m_Collector.AttachToProcessing();
        }

        public override void Disable()
        {
            m_Collector.Stop();
            ICollection<LearningAgent> learningAgents = m_AgentRegistry.GetLearningAgents();
            foreach (LearningAgent agent in learningAgents)
            {
                if (agent.Ready)
                {
                    Agent readyAgent = agent.TurnToAgent();
                    Console.WriteLine("Agent is ready: {0}", readyAgent.Name);
                    m_AgentStorage.SaveAgent(readyAgent.Name, readyAgent);
                    Console.WriteLine("Agent has been saved: {0}", readyAgent.Name);
                    learningAgents.Clear();
                    return;
                }
            }
        }

        void m_History_SnapshotReady(object sender, SnapshotReadyEventArgs e)
        {
            ICollection<Agent> newAgents = m_AgentRegistry.Handle(e.PID, e.Events);
            if (newAgents.Count > 0)
            {
                Console.WriteLine("Learning is complete");
                if (OnLearningCompleted != null)
                    OnLearningCompleted(this);
            }
        }

        void m_Collector_ActionOccurred(TraceEvent obj)
        {
            m_History.Add(new WinPID(obj.ProcessID, obj.ProcessName), new ProcessAction(obj));
        }

        public event Action<LearningState> OnLearningCompleted;
    }

    class WatchingState : State
    {
        IAgentStorage m_AgentStorage;
        string m_Name;
        IStrategy m_RedStrategy;
        IStrategy m_YellowStrategy;

        public WatchingState(string name,
            ProcessEventCollector collector,
            GlobalHistory history,
            AgentRegistry agentRegistry,
            IAgentStorage agentStorage,
            IStrategy redStrategy,
            IStrategy yellowStrategy
            )
            : base(collector, history, agentRegistry)
        {
            m_AgentStorage = agentStorage;
            m_Name = name;
            m_RedStrategy = redStrategy;
            m_YellowStrategy = yellowStrategy;
        }

        public override void Enable()
        {
            Agent agent = m_AgentStorage.LoadAgent(m_Name);
            if (agent == null)
            {
                return;
            }
            m_AgentRegistry.AddWatcher(agent, m_RedStrategy, m_YellowStrategy);
            m_Collector.ActionOccurred += new Action<TraceEvent>(m_Collector_ActionOccurred);
            m_History.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(m_History_SnapshotReady);
            m_Collector.Start();
            Thread thread = new Thread(new ThreadStart(AttachToProcessing));
            thread.Start();
        }

        public override void Disable()
        {
            m_Collector.Stop();
            m_AgentRegistry.StopWatch();
        }

        void m_History_SnapshotReady(object sender, SnapshotReadyEventArgs e)
        {
            m_AgentRegistry.Handle(e.PID, e.Events);
        }

        void m_Collector_ActionOccurred(TraceEvent obj)
        {
            m_History.Add(new WinPID(obj.ProcessID, obj.ProcessName), new ProcessAction(obj));
        }

        private void AttachToProcessing()
        {
            m_Collector.AttachToProcessing();
        }
    }
}