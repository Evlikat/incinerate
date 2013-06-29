using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro.Multi;

namespace NeuroApplication
{
    class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(int count, int totalCount)
        {
            Count = count;
            TotalCount = totalCount;
        }

        public int Count { get; private set; }

        public int TotalCount { get; private set; }
    }

    class NetworkComputationResultEventArgs : EventArgs
    {
        public NetworkComputationResultEventArgs(IMultiNetworkComputationResult result, bool expectedYes)
        {
            Result = result;
            ExpectedYes = expectedYes;
        }

        public IMultiNetworkComputationResult Result { get; private set; }

        public bool ExpectedYes { get; private set; }
    }

    class ComputationFinishedEventArgs : EventArgs
    {
        public ComputationFinishedEventArgs(IList<KeyValuePair<IMultiNetworkComputationResult, bool>> results)
        {
            Results = results;
        }

        public IList<KeyValuePair<IMultiNetworkComputationResult, bool>> Results { get; private set; }
    }
}
