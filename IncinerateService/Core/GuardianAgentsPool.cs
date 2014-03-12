using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class GuardianAgentsPool : IGuardianAgentsPool
    {
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
        public Agent Agent { get; private set; }
        public string Process { get; private set; }
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
            this.Process = process;
            this.RedStrategy = redStrategy;
            this.YellowStrategy = yellowStrategy;
            this.E1 = e1;
            this.E2 = e2;
        }

        public void Start()
        {
            startWatcher = WatchForProcessStart(Process);
            endWatcher = WatchForProcessEnd(Process);
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

        private ManagementEventWatcher WatchForProcessStart(string processName)
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceCreationEvent " +
                "WITHIN  10 ";// +
                //" WHERE TargetInstance ISA 'Win32_Process' " +
                //"   AND TargetInstance.Name = '" + processName + "'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessStarted;
            watcher.Start();
            return watcher;
        }

        private ManagementEventWatcher WatchForProcessEnd(string processName)
        {
            string queryString =
                "SELECT TargetInstance" +
                "  FROM __InstanceDeletionEvent " +
                "WITHIN  10 ";// +
                //" WHERE TargetInstance ISA 'Win32_Process' "; +
                //"   AND TargetInstance.Name = '" + processName + "'";

            // The dot in the scope means use the current machine
            string scope = @"\\.\root\CIMV2";

            // Create a watcher and listen for events
            ManagementEventWatcher watcher = new ManagementEventWatcher(scope, queryString);
            watcher.EventArrived += ProcessEnded;
            watcher.Start();
            return watcher;
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.
                Properties["TargetInstance"].Value;
            lock (syncRoot)
            {
                pids.Add((int) targetInstance.Properties["Name"].Value);
            }
        }

        private void ProcessEnded(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent.
                Properties["TargetInstance"].Value;
            lock (syncRoot)
            {
                pids.Remove((int) targetInstance.Properties["Name"].Value);
            }
        }
    }
}
