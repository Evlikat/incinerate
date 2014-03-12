using System;
using System.Collections.Generic;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class AgentRegistry
    {
        public const int MinPositive = 500;
        public const int MinNegative = 2500;
        public const double P1 = 0.17;
        public const double P2 = 0.5;

        ILearningAgentPool m_LearningAgents = new MultiLearningAgentPool();
        IWatchingAgentsPool m_WatchingAgents = new WatchingAgentPool();

        public ICollection<Agent> Handle(IPID iPID, IList<IProcessAction> actions)
        {
            HistorySnapshot snapshot = new HistorySnapshot(iPID, actions);

            IRecognizedAgent recognized = m_WatchingAgents.Compute(snapshot);
            recognized.Apply(iPID.PID);
            
            return m_LearningAgents.TrainAll(iPID, snapshot);
        }

        public void CreateLearningAgent(LearningConfig learningConfig)
        {
            LearningAgent newAgent = new LearningAgent(
                learningConfig.Name,
                learningConfig.NativePids,
                learningConfig.ForeignPids,
                MinPositive,
                MinNegative);
            m_LearningAgents.AddAgent(newAgent);
        }

        public bool StopLearning(string name)
        {
            return m_LearningAgents.RemoveAgent(name);
        }

        public ICollection<LearningAgent> GetLearningAgents()
        {
            return m_LearningAgents.GetAll();
        }

        public ICollection<WatchingAgentSession> GetWatchingAgents()
        {
            return m_WatchingAgents.GetAll();
        }

        public void AddWatcher(Agent agent, IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2)
        {
            m_WatchingAgents.AddWatcher(agent, redStrategy, yellowStrategy, p1, p2);
        }

        public bool StopWatch(string name)
        {
            return m_WatchingAgents.Stop(name);
        }

        public void StopWatchAll()
        {
            m_WatchingAgents.StopAll();
        }
    }
}
