using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using AForge.Neuro;
using AForge.Neuro.Learning;
using Diagnostics.Eventing;

namespace NeuroIncinerate.Base
{
    public class ProcessHistoryCollector : IProcessHistoryCollector
    {
        private IEnumerable<IWatchableProcessInfo> m_ProcessList;
        private ProcessEventCollector m_Collector = new ProcessEventCollector();
        private ProcessBehaviourInfoContainer m_ProcessBehaviourInfoContainer;

        public IProcessHistoryWatcher Container { get { return m_ProcessBehaviourInfoContainer; } }

        public ProcessHistoryCollector()
        {
        }

        public void Init(IEnumerable<IWatchableProcessInfo> selectedProcesses, IEnumerable<IWatchableProcessInfo> allProcesses)
        {
            m_ProcessList = selectedProcesses;
            m_ProcessBehaviourInfoContainer = new ProcessBehaviourInfoContainer(@"c:\etl", allProcesses);
            // init filter
            foreach (IWatchableProcessInfo processInfo in selectedProcesses)
            {
                m_ProcessBehaviourInfoContainer.Filter.Add(processInfo.PID);
            }
            m_Collector.ActionOccurred += new Action<TraceEvent>(m_Collector_ActionOccurred);
        }

        private void m_Collector_ActionOccurred(TraceEvent obj)
        {
            m_ProcessBehaviourInfoContainer.NewAction(new WinPID(obj.ProcessID, obj.ProcessName), new ProcessAction(obj));
        }

        public void Start()
        {
            m_Collector.Start();
        }

        public void Stop()
        {
            m_Collector.Stop();
        }

        public void Process()
        {
            m_Collector.AttachToProcessing();
        }

        private IEnumerable<ILearningPair> GenerateLearningPairs()
        {
            foreach (IWatchableProcessInfo processInfo in m_ProcessList)
            {
                IProcessHistory history = m_ProcessBehaviourInfoContainer[processInfo.PID];
                throw new NotImplementedException();
                //yield return new LearningPair();
            }
            throw new NotImplementedException();
        }
    }
}
