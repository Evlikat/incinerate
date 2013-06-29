using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace NeuroIncinerate.Neuro
{
    public interface IProcessActionListener
    {
        void NewAction(IPID processID, IProcessAction action);
    }

    public interface IProcessHistoryWatcher
    {
        IProcessHistory this[IPID processID] { get; }
        IEnumerable<IPID> ActivePIDs { get; }
        IList<IPID> Filter { get; }
    }

    class ProcessBehaviourInfoContainer : IProcessActionListener, IProcessHistoryWatcher
    {
        private GlobalHistory m_GlobalHistory = new GlobalHistory();
        private IList<IPID> m_Filter = new List<IPID>();
        private IEnumerable<IWatchableProcessInfo> m_ProcessInfos;
        private SnapshotFileSaver m_Saver;

        public ProcessBehaviourInfoContainer(string pathToSnapshotFolder, IEnumerable<IWatchableProcessInfo> processInfos)
        {
            m_ProcessInfos = processInfos;
            m_Saver = new SnapshotFileSaver(pathToSnapshotFolder);
            m_GlobalHistory.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(GlobalHistory_SnapshotReady);
        }

        private void GlobalHistory_SnapshotReady(object sender, SnapshotReadyEventArgs e)
        {
            string processName = GetProcessNameById(e.PID);
            m_Saver.Save(new HistorySnapshot(e), DateTime.Now);
        }

        private string GetProcessNameById(IPID iPID)
        {
            foreach (IWatchableProcessInfo info in m_ProcessInfos)
            {
                if (info.PID.Equals(iPID))
                {
                    return info.ProcessName;
                }
            }
            return null;
        }

        public void NewAction(IPID processID, IProcessAction action)
        {
            if (m_Filter.Count == 0 || m_Filter.Contains(processID))
            {
                m_GlobalHistory.Add(processID, action);
            }
        }

        public IProcessHistory this[IPID processID]
        {
            get { return m_GlobalHistory[processID]; }
        }

        public IEnumerable<IPID> ActivePIDs
        {
            get { return m_GlobalHistory.ActivePIDs; }
        }

        public IList<IPID> Filter
        {
            get
            {
                return m_Filter;
            }
        }
    }
}
