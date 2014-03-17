using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Eventing;

namespace NeuroIncinerate.Neuro
{
    [Serializable]
    public class AffectedKeys
    {
        //
        public ISet<int> AffectedDestinationPorts { get; private set; }
        public ISet<int> AffectedSourcePorts { get; private set; }
        public ISet<string> AffectedRegKeys { get; private set; }
        public ISet<string> AffectedRegValues { get; private set; }

        public AffectedKeys()
        {
            AffectedDestinationPorts = new HashSet<int>();
            AffectedSourcePorts = new HashSet<int>();
            AffectedRegKeys = new HashSet<string>();
            AffectedRegValues = new HashSet<string>();
        }

        public AffectedKeys(TraceEvent traceEvent) : this()
        {
            string keyName = (string)traceEvent.PayloadByName("KeyName");
            if (!String.IsNullOrEmpty(keyName))
            {
                AffectedRegKeys.Add(keyName);
            }
            string valueName = (string)traceEvent.PayloadByName("ValueName");
            if (!String.IsNullOrEmpty(valueName))
            {
                AffectedRegValues.Add(valueName);
            }
            int? sPort = (int?)traceEvent.PayloadByName("sport");
            if (sPort != null)
            {
                AffectedSourcePorts.Add((UInt16)(sPort.Value));
            }
            int? dPort = (int?)traceEvent.PayloadByName("dport");
            if (dPort != null)
            {
                AffectedDestinationPorts.Add((UInt16)(dPort.Value));
            }
        }

        public void UnionWith(AffectedKeys keys)
        {
            AffectedDestinationPorts.UnionWith(keys.AffectedDestinationPorts);
            AffectedSourcePorts.UnionWith(keys.AffectedSourcePorts);
            AffectedRegKeys.UnionWith(keys.AffectedRegKeys);
            AffectedRegValues.UnionWith(keys.AffectedRegValues);
        }

        public override string ToString()
        {
            return (AffectedDestinationPorts.Count == 0 ? "" : "dports=" + String.Join(",", AffectedDestinationPorts))
                + (AffectedSourcePorts.Count == 0 ? "" : "; sports=" + String.Join(",", AffectedSourcePorts))
                + (AffectedRegKeys.Count == 0 ? "" : "; regKeys=" + String.Join(",", AffectedRegKeys))
                + (AffectedRegValues.Count == 0 ? "" : "; regValues=" + String.Join(",", AffectedRegValues));
        }
    }
}
