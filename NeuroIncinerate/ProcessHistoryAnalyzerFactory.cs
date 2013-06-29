using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Base;

namespace NeuroIncinerate
{
    public class ProcessHistoryAnalyzerFactory
    {
        public IProcessHistoryCollector NewAnalyzer()
        {
            return new ProcessHistoryCollector();
        }
    }
}
