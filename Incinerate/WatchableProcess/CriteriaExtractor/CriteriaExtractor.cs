using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incinerate.WatchableProcess.CriteriaExtractor
{
    public abstract class CriteriaExtractor<T> where T : CriteriaMemento
    {
        public abstract T Value { get; protected set; }
    }
}
