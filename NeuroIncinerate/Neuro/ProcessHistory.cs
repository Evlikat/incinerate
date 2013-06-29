using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    public interface IProcessHistory
    {
        void Add(IProcessAction action);

        event EventHandler<SnapshotReadyEventArgs> SnapshotReady;

        IProcessAction this[int index] { get; }

        int Length { get; }

        long TotalAdded { get; }
    }

    public class SnapshotReadyEventArgs : EventArgs
    {
        public IList<IProcessAction> Events { get; private set; }
        public IPID PID { get; private set; }

        public SnapshotReadyEventArgs(IList<IProcessAction> list, IPID pid)
        {
            this.Events = list;
            this.PID = pid;
        }
    }

    public class LimitedProcessHistory : IProcessHistory
    {
        private IPID m_ProcessID;
        private int m_Limit;
        private IList<IProcessAction> m_List;
        private int m_Current = 0;
        private long m_TotalAdded = 0L;

        public LimitedProcessHistory(IPID processID, int limit)
        {
            this.m_ProcessID = processID;
            this.m_Limit = limit;
            this.m_List = new IProcessAction[limit];
        }

        public void Add(IProcessAction action)
        {
            m_List[m_Current] = action;
            m_Current = (m_Current + 1) % m_Limit;
            m_TotalAdded++;
            if (m_TotalAdded % m_Limit == 0)
            {
                if (SnapshotReady != null)
                    SnapshotReady(this, new SnapshotReadyEventArgs(m_List, m_ProcessID));
            }
        }

        public IProcessAction this[int index]
        {
            get { return m_List[(index + m_Current) % m_Limit]; }
        }

        public event EventHandler<SnapshotReadyEventArgs> SnapshotReady;


        public int Length
        {
            get { return m_List.Count; }
        }

        public long TotalAdded
        {
            get { return m_TotalAdded; }
        }
    }
}
