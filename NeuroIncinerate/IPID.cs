using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate
{
    public interface IPID : IComparable<IPID>
    {
        int PID { get; }
        string Name { get; }
    }
}
