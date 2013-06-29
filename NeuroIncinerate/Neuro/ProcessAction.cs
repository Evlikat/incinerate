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
    }

    [Serializable]
    public class ProcessAction : IProcessAction
    {
        public unsafe ProcessAction(TraceEvent traceEvent)
        {
            EventName = traceEvent.EventName;
            OpcodeName = traceEvent.OpcodeName;
            PayloadNames = traceEvent.PayloadNames;
            PointerSize = traceEvent.PointerSize;
            ProcessorNumber = traceEvent.ProcessorNumber;
            ProviderGuid = traceEvent.ProviderGuid;
            ProviderName = traceEvent.ProviderName;
            TaskName = traceEvent.TaskName;
            ThreadID = traceEvent.ThreadID;
            TimeStamp = traceEvent.TimeStamp;
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}", EventName, OpcodeName);
        }

        public string EventName { get; set; }
        public string OpcodeName { get; set; }
        public string[] PayloadNames { get; set; }
        public int PointerSize { get; set; }
        public int ProcessorNumber { get; set; }
        public Guid ProviderGuid { get; set; }
        public string ProviderName { get; set; }
        public string TaskName { get; set; }
        public int ThreadID { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
