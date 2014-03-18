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

        int DiskFileActivity { get; }

        int NetActivity { get; }

        int RegistryActivity { get; }

        AffectedKeys AffectedKeys { get; }

        string DynamicName { get; set; }
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

        public int DiskFileActivity { get; private set; }
        public int NetActivity { get; private set; }
        public int RegistryActivity { get; private set; }

        public AffectedKeys AffectedKeys { get; private set; }
        public string DynamicName { get; set; }

        public LimitedProcessHistory(IPID processID, int limit)
        {
            this.m_ProcessID = processID;
            this.m_Limit = limit;
            this.m_List = new IProcessAction[limit];

            this.DiskFileActivity = 0;
            this.NetActivity = 0;
            this.RegistryActivity = 0;

            this.AffectedKeys = new AffectedKeys();
            this.DynamicName = "";
        }

        public void Add(IProcessAction action)
        {
            UpdateStats(action);
            m_List[m_Current] = action;
            m_Current = (m_Current + 1) % m_Limit;
            m_TotalAdded++;
            if (m_TotalAdded % m_Limit == 0)
            {
                if (SnapshotReady != null)
                    SnapshotReady(this, new SnapshotReadyEventArgs(m_List, m_ProcessID));
            }
        }

        public void UpdateStats(IProcessAction action)
        {
            if (action.EventName.StartsWith("DiskIo") || action.EventName.StartsWith("FileIo"))
            {
                DiskFileActivity++;
            }
            else if (action.EventName.StartsWith("TcpIp") || action.EventName.StartsWith("UdpIp"))
            {
                NetActivity++;
            }
            else if (action.EventName.StartsWith("Registry"))
            {
                RegistryActivity++;
            }
            AffectedKeys.UnionWith(action.AffectedKeys);
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
