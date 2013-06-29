using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class NullPool : ILearningAgentPool
    {
        public ICollection<Agent> TrainAll(IPID iPID, HistorySnapshot snapshot)
        {
            return new List<Agent>();
        }

        public void AddAgent(LearningAgent learningAgent) { }


        public void Clear() { }

        public ICollection<LearningAgent> GetAll() { return new List<LearningAgent>(); }


        public bool IsEmpty()
        {
            return true;
        }
    }
}
