using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    public interface IWatchableProcessInfo
    {
        IPID PID { get; }
        string ProcessName { get; }
        string ToString();
    }
}
