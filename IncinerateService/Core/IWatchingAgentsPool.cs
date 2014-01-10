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

        void Stop(Agent agent);

        void StopAll();

        IRecognizedAgent Compute(HistorySnapshot snapshot);
    }

    interface IRecognizedAgent
    {
        void Apply(int pid);
    }
}
