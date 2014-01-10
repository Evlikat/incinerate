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

        public void Stop(Agent agent)
        {
            lock (syncRoot)
            {
                if (!m_Agents.ContainsKey(agent))
                {
                    m_Agents.Remove(agent);
                }
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
            double max = 0;
            IRecognizedAgent recognized = new NotRecognizedAgent();
            lock (syncRoot)
            {
                foreach (KeyValuePair<Agent, WatchingAgentSession> agentSession in m_Agents)
                {
                    double res = agentSession.Key.Compute(snapshot);
                    if (res > max)
                    {
                        max = res;
                        recognized = new RecognizedAgent(res,
                            agentSession.Value.RedStrategy,
                            agentSession.Value.YellowStrategy,
                            agentSession.Value.P1,
                            agentSession.Value.P2);
                    }
                }
            }
            return recognized;
        }
    }

    class WatchingAgentSession
    {
        Agent m_Agent;
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
    }

    class NotRecognizedAgent : IRecognizedAgent
    {
        public void Apply(int pid) { }
    }
}
