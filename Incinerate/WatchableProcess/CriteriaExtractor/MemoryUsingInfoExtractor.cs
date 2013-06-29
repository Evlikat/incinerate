using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Incinerate.WatchableProcess.CriteriaExtractor
{
    class MemoryUsingInfoExtractor : CriteriaExtractor<MemoryUsingInfo>
    {
        public MemoryUsingInfoExtractor(Process process)
        {
            Value = new MemoryUsingInfo(process.WorkingSet64);
        }

        public override MemoryUsingInfo Value { get; protected set; }
    }
}
