using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Incinerate.WatchableProcess.CriteriaExtractor
{
    class CpuUsingInfoExtractor : CriteriaExtractor<CpuUsingInfo>
    {
        public CpuUsingInfoExtractor(string processName)
        {
            using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", processName))
            {
                Value = new CpuUsingInfo(pcProcess.RawValue);
            }
        }

        public override CpuUsingInfo Value { get; protected set; }
    }
}
