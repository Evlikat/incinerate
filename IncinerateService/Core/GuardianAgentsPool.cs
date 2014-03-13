using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using NeuroIncinerate.Neuro;
using System.Diagnostics;
using NLog;

namespace IncinerateService.Core
{
    class GuardianAgentsPool : IGuardianAgentsPool
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        object syncRoot = new object();
        IDictionary<Agent, GuardianAgentSession> m_Agents = new Dictionary<Agent, GuardianAgentSession>();

        public void AddGuardian(Agent agent, string process, IStrategy redStrategy,
            IStrategy yellowStrategy, double e1, double e2)
        {
            lock (syncRoot)
            {
                if (!m_Agents.ContainsKey(agent))
                {
                    GuardianAgentSession session =
                        new GuardianAgentSession(agent, process, redStrategy, yellowStrategy, e1, e2);
                    m_Agents.Add(agent, session);
                    session.Start();
                }
            }
        }

        public bool Stop(string name)
        {
            lock (syncRoot)
            {
                foreach (Agent agent in m_Agents.Keys)
                {
                    if (String.Compare(agent.Name, name) == 0)
                    {
                        m_Agents[agent].Stop();
                        m_Agents.Remove(agent);
                        return true;
                    }
                }
                return false;
            }
        }

        public IEnumerable<AgentReaction> Compute(HistorySnapshot snapshot)
        {
            lock (syncRoot)
            {
                foreach (KeyValuePair<Agent, GuardianAgentSession> agentSession in m_Agents)
                {
                    if (!agentSession.Value.Check(snapshot.PID.PID))
                    {
                        continue;
                    }
                    double res = agentSession.Key.Compute(snapshot);
                    if (res >= agentSession.Value.E1)
                    {
                        continue;
                    }
                    else if (res >= agentSession.Value.E2)
                    {
                        yield return new AgentReaction(res, snapshot.PID.PID, agentSession.Value.YellowStrategy);
                    }
                    else
                    {
                        yield return new AgentReaction(res, snapshot.PID.PID, agentSession.Value.RedStrategy);
                    }
                }
            }
        }

        public ICollection<GuardianAgentSession> GetAll()
        {
            return m_Agents.Values;
        }
    }

    class GuardianAgentSession
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public Agent Agent { get; private set; }
        public string TargetProcess { get; private set; }
        public IStrategy RedStrategy { get; private set; }
        public IStrategy YellowStrategy { get; private set; }
        public double E1 { get; private set; }
        public double E2 { get; private set; }

        ManagementEventWatcher startWatcher;
        ManagementEventWatcher endWatcher;

        object syncRoot = new object();
        private IList<int> pids = new List<int>();

        public GuardianAgentSession(Agent agent, string process,
            IStrategy redStrategy, IStrategy yellowStrategy, double e1, double e2)
        {
            this.Agent = agent;
            this.TargetProcess = process;
            this.RedStrategy = redStrategy;
            this.YellowStrategy = yellowStrategy;
            this.E1 = e1;
            this.E2 = e2;
        }

        public void Start()
        {
            // Load existing processes
            Process [] processes = Process.GetProcessesByName(TargetProcess);
            lock (syncRoot)
            {
                foreach (Process p in processes)
                {
                    pids.Add(p.Id);
                }
            }

            // Start watching for new instances
            startWatcher = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            endWatcher = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");

            startWatcher.EventArrived += new EventArrivedEventHandler(ProcessStarted);
            startWatcher.Start();

            endWatcher.EventArrived += new EventArrivedEventHandler(ProcessEnded);
            endWatcher.Start();

            Log.Info("Запуск режима охраны для {0}", TargetProcess);
            Log.Info("Наблюдаемые процессы: {0}", String.Join(", ", pids));
        }

        public void Stop()
        {
            startWatcher.Stop();
            endWatcher.Stop();
        }

        public bool Check(int pid)
        {
            return pids.Contains(pid);
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            if (String.Compare(name, TargetProcess) == 0 || String.Compare(name, TargetProcess + ".exe") == 0)
            {
                uint id = (uint)e.NewEvent.Properties["ProcessID"].Value;
                lock (syncRoot)
                {
                    pids.Add((int)id);
                    Log.Info("Обнаружен запуск целевого процесса: {0} [{1}]", name, id);
                    Log.Info("Наблюдаемые процессы: {0}", String.Join(", ", pids));
                }
            }
        }

        private void ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            if (name != null && name.Contains(TargetProcess))
            {
                uint id = (uint)e.NewEvent.Properties["ProcessID"].Value;
                lock (syncRoot)
                {
                    pids.Remove((int)id);
                    Log.Info("Целевой процесс завершен: {0} [{1}]", name, id);
                    Log.Info("Наблюдаемые процессы: {0}", String.Join(", ", pids));
                }
            }
        }
    }
}
