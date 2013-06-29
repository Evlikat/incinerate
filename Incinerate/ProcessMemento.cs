using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.WatchableProcess
{
    class ProcessMemento
    {
        public CpuUsingInfo CpuUsing { get; set; }
        public MemoryUsingInfo MemoryUsing { get; set; }
        public ThreadCount ThreadsCount { get; set; }
        public string Name { get; set; }
        public int Pid { get; set; }

        public ProcessMemento()
        {
            CpuUsing = new CpuUsingInfo(0);
            MemoryUsing = new MemoryUsingInfo(0);
            ThreadsCount = new ThreadCount(0);
        }
    }
}
