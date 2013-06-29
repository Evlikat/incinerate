using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.WatchableProcess
{
    [Serializable]
    class MemoryUsingInfo : CriteriaMemento
    {
        public MemoryUsingInfo(long memUsing) : base()
        {
            Value = memUsing;
        }

        public override string GetString()
        {
            return (Value / 1024).ToString() + "k";
        }
        public override double GetMinValue()
        {
            return 0;
        }

        public override double GetMaxValue()
        {
            return Value * 1.25;
        }

        public override double GetInterval()
        {
            return Value / 10;
        }
    }
}
