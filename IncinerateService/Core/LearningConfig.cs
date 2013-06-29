using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate;

namespace IncinerateService.Core
{
    public class LearningConfig
    {
        public ISet<IPID> NativePids { get; private set; }
        public ISet<IPID> ForeignPids { get; private set; }
        public string Name { get; private set; }

        public LearningConfig(string name, ISet<IPID> nativePids, ISet<IPID> foreignPids)
        {
            Name = name;
            NativePids = nativePids;
            ForeignPids = foreignPids;
        }
    }
}
