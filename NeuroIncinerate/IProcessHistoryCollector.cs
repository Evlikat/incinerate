using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using System.Collections.ObjectModel;

namespace NeuroIncinerate
{
    public interface IProcessHistoryCollector
    {
        IProcessHistoryWatcher Container { get; }

        void Start();
        void Stop();
        void Process();
        void Init(IEnumerable<IWatchableProcessInfo> selectedProcesses, IEnumerable<IWatchableProcessInfo> allProcesses);
    }
}
