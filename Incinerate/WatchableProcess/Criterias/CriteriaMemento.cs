using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.WatchableProcess
{
    [Serializable]
    public abstract class CriteriaMemento
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        
        public CriteriaMemento()
        {
            Date = DateTime.Now;
        }

        public abstract string GetString();
        public abstract double GetMinValue();
        public abstract double GetMaxValue();
        public abstract double GetInterval();
    }
}
