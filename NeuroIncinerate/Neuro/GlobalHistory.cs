using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

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
                string name;
                if (String.IsNullOrEmpty(processID.Name))
                {
                    try
                    {
                        name = Process.GetProcessById(processID.PID).ProcessName;
                    }
                    catch (Exception ex)
                    {
                        name = "UNKNOWN";
                    }
                }
                else
                {
                    name = processID.Name;
                }
                IPID pid = new WinPID(processID.PID, name);
                processHistory = m_ProcessHistoryFactory.CreateProcessHistory(pid);
                processHistory.SnapshotReady += new EventHandler<SnapshotReadyEventArgs>(ProcessHistory_SnapshotReady);
                m_History.Add(pid, processHistory);
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

        public bool ContainsKey(IPID processID)
        {
            return m_History.ContainsKey(processID);
        }

        public IProcessHistory this[IPID processID]
        {
            get { return m_History[processID]; }
        }

        public IEnumerable<IPID> ActivePIDs
        {
            get { return m_History.Keys; }
        }

        public void SetDynamicName(IPID processID, string recognizedAgentName)
        {
            if (m_History.ContainsKey(processID))
            {
                m_History[processID].DynamicName = recognizedAgentName;
            }
        }
    }
}
