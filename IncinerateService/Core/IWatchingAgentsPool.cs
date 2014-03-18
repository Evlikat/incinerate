using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    interface IWatchingAgentsPool
    {
        void AddWatcher(Agent agent,
            IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2);

        bool Stop(string name);

        void StopAll();

        IRecognizedAgent Compute(HistorySnapshot snapshot);

        ICollection<WatchingAgentSession> GetAll();
    }

    interface IRecognizedAgent
    {
        string Name { get; }

        double MaxRes { get; }

        bool Apply(int pid);
    }
}
