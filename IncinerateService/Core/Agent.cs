using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro.Multi;
using NeuroIncinerate.Neuro;
using NeuroIncinerate;

namespace IncinerateService.Core
{
    [Serializable]
    class Agent
    {
        MultiActivationNetwork m_Analyzer;
        ISet<int> m_NativePIDs = new SortedSet<int>();
        ISet<int> m_ForeignPIDs = new SortedSet<int>();
        string m_Name;

        [NonSerialized]
        IProcessStrategy m_Strategy = new PassStrategy();

        public IProcessStrategy Strategy
        {
            get { return m_Strategy; }
            private set { this.m_Strategy = value; }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public Agent(string name, ISet<IPID> native, ISet<IPID> foreign)
        {
            m_Name = name;
            m_Analyzer = new MultiActivationNetwork(name);
            foreach (IPID pid in native)
            {
                if (!m_NativePIDs.Contains(pid.PID))
                {
                    m_NativePIDs.Add(pid.PID);
                }
            }
            foreach (IPID pid in foreign)
            {
                if (!m_ForeignPIDs.Contains(pid.PID))
                {
                    m_ForeignPIDs.Add(pid.PID);
                }
            }
        }

        public void Train(HistorySnapshot snapshot, bool isTargetProcess)
        {
            m_Analyzer.RunTrain(snapshot, isTargetProcess);
        }

        public double Compute(HistorySnapshot snapshot)
        {
            IMultiNetworkComputationResult result = m_Analyzer.Compute(snapshot);
            return result.Result[0] - result.Result[1];
        }

        public bool Native(int pid)
        {
            return m_NativePIDs.Contains(pid);
        }

        public bool Foreign(int pid)
        {
            return m_ForeignPIDs.Contains(pid);
        }

        public void Apply(IPID iPID)
        {
            if (!m_NativePIDs.Contains(iPID.PID))
            {
                m_NativePIDs.Add(iPID.PID);
            }
        }
    }
}
