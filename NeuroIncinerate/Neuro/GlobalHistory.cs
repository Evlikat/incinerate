using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    public class GlobalHistory
    {
        IDictionary<IPID, IProcessHistory> m_History = new Dictionary<IPID, IProcessHistory>();
        IProcessHistoryFactory m_ProcessHistoryFactory = new ProcessHistoryFactory();

        public event EventHandler<SnapshotReadyEventArgs> SnapshotReady;

        public void Add(IPID processID, IProcessAction action)
        {
            if (processID.PID <= 0)
            {
                return;
            }
            IProcessHistory processHistory;
            if (!m_History.ContainsKey(processID))
            {
                processHistory = m_ProcessHistoryFactory.CreateProcessHistory(processID);
                processHistory.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(ProcessHistory_SnapshotReady);
                m_History.Add(processID, processHistory);
            }
            else
            {
                processHistory = m_History[processID];
            }
            processHistory.Add(action);
        }

        void ProcessHistory_SnapshotReady(object sender, SnapshotReadyEventArgs e)
        {
            if (SnapshotReady != null)
                SnapshotReady(sender, e);
        }

        public IProcessHistory this[IPID processID]
        {
            get { return m_History[processID]; }
        }

        public IEnumerable<IPID> ActivePIDs
        {
            get { return m_History.Keys; }
        }
    }
}
