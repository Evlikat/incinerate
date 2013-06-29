using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Incinerate.WatchableProcess.CriteriaExtractor
{
    class ThreadCountExtractor : CriteriaExtractor<ThreadCount>
    {
        public ThreadCountExtractor(Process process)
        {
            Value = new ThreadCount(process.Threads.Count);
        }

        public override ThreadCount Value { get; protected set; }
    }
}
