using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class WatchingAgentPool : IWatchingAgentsPool
    {
        object syncRoot = new object();
        IDictionary<Agent, WatchingAgentSession> m_Agents = new Dictionary<Agent, WatchingAgentSession>();

        public void AddWatcher(Agent agent, IStrategy redStrategy, IStrategy yellowStrategy, double p1, double p2)
        {
            lock (syncRoot)
            {
                if (!m_Agents.ContainsKey(agent))
                {
                    m_Agents.Add(agent, new WatchingAgentSession(agent, redStrategy, yellowStrategy, p1, p2));
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
                        m_Agents.Remove(agent);
                        return true;
                    }
                }
                return false;
            }
        }

        public void StopAll()
        {
            lock (syncRoot)
            {
                m_Agents.Clear();
            }
        }


        public IRecognizedAgent Compute(HistorySnapshot snapshot)
        {
            double max = -1.0;
            IRecognizedAgent recognized = new NotRecognizedAgent();
            lock (syncRoot)
            {
                foreach (KeyValuePair<Agent, WatchingAgentSession> agentSession in m_Agents)
                {
                    double res = agentSession.Key.Compute(snapshot);
                    if (res > max)
                    {
                        max = res;
                        RecognizedAgent newRecognized = new RecognizedAgent(res,
                            agentSession.Value.RedStrategy,
                            agentSession.Value.YellowStrategy,
                            agentSession.Value.P1,
                            agentSession.Value.P2);
                        newRecognized.MaxRes = max;
                        recognized = newRecognized;
                    }
                }
            }
            return recognized;
        }

        public ICollection<WatchingAgentSession> GetAll()
        {
            return m_Agents.Values;
        }
    }

    class WatchingAgentSession
    {
        Agent m_Agent;
        public string AgentName { get { return m_Agent.Name; } }
        public double P1 { get; private set; }
        public double P2 { get; private set; }
        public IStrategy RedStrategy { get; private set; }
        public IStrategy YellowStrategy { get; private set; }

        public WatchingAgentSession(Agent agent, IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2)
        {
            P1 = p1;
            P2 = p2;
            m_Agent = agent;
            RedStrategy = redStrategy;
            YellowStrategy = yellowStrategy;
        }
    }

    class RecognizedAgent : IRecognizedAgent
    {
        double m_P1;
        double m_P2;
        double m_Res;
        IStrategy m_RedStrategy;
        IStrategy m_YellowStrategy;

        public RecognizedAgent(double res,
            IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2)
        {
            m_P1 = p1;
            m_P2 = p2;
            m_Res = res;
            m_RedStrategy = redStrategy;
            m_YellowStrategy = yellowStrategy;
        }

        public void Apply(int pid)
        {
            if (m_Res > m_P1)
            {
                if (m_Res > m_P2)
                {
                    m_RedStrategy.Apply(m_Res, pid);
                }
                else
                {
                    m_YellowStrategy.Apply(m_Res, pid);
                }
            }
            
        }

        public double MaxRes { get; set; }
    }

    class NotRecognizedAgent : IRecognizedAgent
    {
        public void Apply(int pid) { }

        public double MaxRes
        {
            get { return 0.0; }
        }
    }
}
