using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro.Multi
{
    public interface IMultiNetworkComputationResult
    {
        double[] Result { get; }

        int Index { get; }

        IList<NetworkComputationResultEntry> Results { get; }
    }

    public class NetworkComputationResultEntry
    {
        public double[] Values { get; private set; }
        public string EventClass { get; private set; }

        public NetworkComputationResultEntry(double[] values, string eventClass)
        {
            EventClass = eventClass;
            Values = values;
        }
    }

    public class MultiNetworkComputationResult : IMultiNetworkComputationResult
    {
        public MultiNetworkComputationResult(IList<NetworkComputationResultEntry> results,
            INetworkTrustRegistry networkTrustRegistry)
        {
            Results = results;
            int vectorLength = results[0].Values.Length;
            Result = new double[vectorLength];
            foreach (NetworkComputationResultEntry resultEntry in results)
            {
                for (int i = 0; i < vectorLength; i++)
                {
                    Result[i] += resultEntry.Values[i] * networkTrustRegistry.GetNetworkTrustLevel(resultEntry.EventClass);
                }
            }
            double trustSum = networkTrustRegistry.Sum;
            int maxIndex = 0;
            double maxValue = 0;
            for (int i = 0; i < vectorLength; i++)
            {
                Result[i] /= trustSum;
                if (Result[i] > maxValue)
                {
                    maxIndex = i;
                    maxValue = Result[i];
                }
            }
            Index = maxIndex;
        }

        public double[] Result { get; private set; }

        public int Index { get; private set; }

        public IList<NetworkComputationResultEntry> Results { get; private set; }
    }

    public interface INetworkTrustRegistry
    {
        double GetNetworkTrustLevel(string NetworkClass);
        void ApplyTrustLevel(IList<NetworkComputationResultEntry> results, int expectedIndex);
        double Sum { get; }
        int Length { get; }
    }

    [Serializable]
    public class NetworkTrustVector : INetworkTrustRegistry
    {
        private class TrustLevel
        {
            public int SuccessCount { get; set; }
            public int Total { get; set; }
            public double Level { get {return(double) SuccessCount /Total;} }

            public TrustLevel(int successCount, int total)
            {
                SuccessCount = successCount;
                Total = total;
            }
        }

        private IDictionary<string, TrustLevel> trustLevelDict = new Dictionary<string, TrustLevel>();

        public void ApplyTrustLevel(IList<NetworkComputationResultEntry> results, int expectedIndex)
        {
            foreach (NetworkComputationResultEntry entry in results) 
            {
                if (trustLevelDict.ContainsKey(entry.EventClass))
                {
                    trustLevelDict[entry.EventClass].Total++;
                    trustLevelDict[entry.EventClass].SuccessCount += expectedIndex == 0 ? 1 : 0;
                }
                else
                {
                    TrustLevel level = new TrustLevel(expectedIndex == 0 ? 1 : 0, 1);
                    trustLevelDict.Add(entry.EventClass, level);
                }
            }
        }

        public double GetNetworkTrustLevel(string networkClass)
        {
            if (trustLevelDict.ContainsKey(networkClass))
            {
                return (double)trustLevelDict[networkClass].Level;
            }
            return 0;
        }

        public int Length { get { return trustLevelDict.Count; } }

        public double Sum
        {
            get
            {
                double sum = 0;
                foreach (KeyValuePair<string, TrustLevel> pair in trustLevelDict)
                    sum += pair.Value.Level;
                return sum;
            }
        }
    }

    [Serializable]
    public class UniformNetworkTrustVector : INetworkTrustRegistry
    {
        private int m_Count;

        public UniformNetworkTrustVector(int count)
        {
            m_Count = count;
        }

        public void ApplyTrustLevel(IList<NetworkComputationResultEntry> results, int expectedIndex) { }

        public double GetNetworkTrustLevel(string NetworkClass)
        {
            return 1;
        }

        public double Sum
        {
            get { return m_Count; }
        }

        public int Length
        {
            get { return m_Count; }
        }
    }
}
