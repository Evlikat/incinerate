using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class MultiLearningAgentPool : ILearningAgentPool
    {
        IList<LearningAgent> m_LearningAgents = new List<LearningAgent>();

        public ICollection<Agent> TrainAll(IPID iPID, HistorySnapshot snapshot)
        {
            ICollection<Agent> readyAgents = new List<Agent>();
            foreach (LearningAgent learningAgent in m_LearningAgents)
            {
                if (learningAgent.IsNative(iPID))
                {
                    learningAgent.Train(snapshot, true);
                }
                else if (learningAgent.IsForeign(iPID))
                {
                    learningAgent.Train(snapshot, false);
                }
                else
                {
                    continue;
                }

                if (learningAgent.Ready)
                {
                    readyAgents.Add(learningAgent.TurnToAgent());
                }
            }
            return readyAgents;
        }

        public void AddAgent(LearningAgent learningAgent)
        {
            m_LearningAgents.Add(learningAgent);
        }

        public void Clear()
        {
            m_LearningAgents.Clear();
        }


        public ICollection<LearningAgent> GetAll()
        {
            return m_LearningAgents;
        }

        public bool IsEmpty()
        {
            return m_LearningAgents.Count == 0;
        }
    }
}
