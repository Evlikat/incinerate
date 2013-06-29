using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    interface ILearningAgentPool
    {
        void AddAgent(LearningAgent learningAgent);

        ICollection<Agent> TrainAll(IPID iPID, HistorySnapshot snapshot);

        bool IsEmpty();

        void Clear();

        ICollection<LearningAgent> GetAll();
    }
}
