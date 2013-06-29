using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.WatchableProcess
{
    [Serializable]
    class CpuUsingInfo : CriteriaMemento
    {
        public CpuUsingInfo(double cpuUsing) : base()
        {
            Value = cpuUsing;
        }

        public override string GetString()
        {
            return Value.ToString("0.00");
        }
        public override double GetMinValue()
        {
            return 0;
        }
        public override double GetMaxValue()
        {
            return Value * 1.2;
        }
        
        public override double GetInterval()
        {
            return Value / 10;
        }
    }
}
