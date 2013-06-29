using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using Incinerate.WatchableProcess;
using System.Threading;

namespace Incinerate.Base
{
    /// <summary>
    /// A watcher class which tracks for process behaviour
    /// </summary>
    [Serializable]
    class ProcessWatcher
    {
        private SyncList<ProcessInfo> m_processInfos = new SyncList<ProcessInfo>();
        private Dictionary<int, VectorN> m_massCenters = new Dictionary<int, VectorN>();
        private VectorN m_massCenter = new VectorN(ProcessInfo.CriterionNames.Length - 1);
        private List<ProcessInfo> m_strangeProcesses = new List<ProcessInfo>();

        public SyncList<ProcessInfo> Processes { get { return m_processInfos; } }
        public Dictionary<int, VectorN> MassCenters { get { return m_massCenters; } }
        public VectorN MassCenter { get { return m_massCenter; } }
        public List<ProcessInfo> StrangeProcesses { get { return m_strangeProcesses; } }

        public ProcessInfo ProcessInfoByPid(int pid)
        {
            foreach (ProcessInfo info in m_processInfos)
                if (info.Pid == pid)
                    return info;

            return null;
        }

        public ProcessInfo this[int index]
        {
            get
            {
                return ProcessInfoByPid(index);
            }
        }

        public ProcessWatcher()
        {

        }
    }
}
