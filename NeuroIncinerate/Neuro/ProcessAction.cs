using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Eventing;
using System.Runtime.Serialization;

namespace NeuroIncinerate.Neuro
{
    public interface IProcessAction
    {
        string EventName { get; }
        AffectedKeys AffectedKeys { get; }
    }

    [Serializable]
    public class ProcessAction : IProcessAction
    {
        public unsafe ProcessAction(TraceEvent traceEvent)
        {
            EventName = traceEvent.EventName;
            OpcodeName = traceEvent.OpcodeName;
            PayloadNames = traceEvent.PayloadNames;
            //
            AffectedKeys = new AffectedKeys(traceEvent);
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", EventName, OpcodeName);
        }

        public string EventName { get; set; }
        public string OpcodeName { get; set; }
        public string[] PayloadNames { get; set; }
        //
        public AffectedKeys AffectedKeys { get; private set; }
    }
}
