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

        ILearningAgentPool m_LearningAgents = new MultiLearningAgentPool();
        Agent m_WatchingAgent;

        public ICollection<Agent> Handle(IPID iPID, IList<IProcessAction> actions)
        {
            HistorySnapshot snapshot = new HistorySnapshot(iPID, actions);
            
            if (m_WatchingAgent != null)
            {
                m_WatchingAgent.Compute(snapshot);
            }

            return m_LearningAgents.TrainAll(iPID, snapshot);
        }

        public void CreateLearningAgent(LearningConfig learningConfig)
        {
            if (!m_LearningAgents.IsEmpty())
            {
                return;
            }
            LearningAgent newAgent = new LearningAgent(
                learningConfig.Name,
                learningConfig.NativePids,
                learningConfig.ForeignPids,
                MinPositive,
                MinNegative);
            m_LearningAgents.AddAgent(newAgent);
        }

        public void StopLearning()
        {
            m_LearningAgents.Clear();
        }

        public ICollection<LearningAgent> GetLearningAgents()
        {
            return m_LearningAgents.GetAll();
        }

        public void AddWatcher(Agent agent)
        {
            m_WatchingAgent = agent;
        }

        public void StopWatch()
        {
            m_WatchingAgent = null;
        }
    }
}
