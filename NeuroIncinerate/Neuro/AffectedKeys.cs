using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Eventing;
using System.Text.RegularExpressions;

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
        public ISet<int> AffectedDestinationAddresses { get; set; }
        public ISet<int> AffectedSourceAddresses { get; set; }

        public AffectedKeys()
        {
            AffectedDestinationPorts = new HashSet<int>();
            AffectedSourcePorts = new HashSet<int>();
            AffectedRegKeys = new HashSet<string>();
            AffectedRegValues = new HashSet<string>();
            AffectedDestinationAddresses = new HashSet<int>();
            AffectedSourceAddresses = new HashSet<int>();
        }

        public AffectedKeys(TraceEvent traceEvent) : this()
        {
            string keyName = (string)traceEvent.PayloadByName("KeyName");
            if (!String.IsNullOrEmpty(keyName))
            {
                AffectedRegKeys.Add(ReplaceGuids(keyName));
            }
            string valueName = (string)traceEvent.PayloadByName("ValueName");
            if (!String.IsNullOrEmpty(valueName))
            {
                AffectedRegValues.Add(ReplaceGuids(valueName));
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
            if (!traceEvent.EventName.Contains("IPV6"))
            {
                int? sAddr = (int?)traceEvent.PayloadByName("saddr");
                if (sAddr != null)
                {
                    AffectedSourceAddresses.Add(sAddr.Value);
                }
                int? dAddr = (int?)traceEvent.PayloadByName("daddr");
                if (dAddr != null)
                {
                    AffectedDestinationAddresses.Add(dAddr.Value);
                }
            }
        }

        public void UnionWith(AffectedKeys keys)
        {
            if (keys != null)
            {
                AffectedDestinationPorts.UnionWith(keys.AffectedDestinationPorts);
                AffectedSourcePorts.UnionWith(keys.AffectedSourcePorts);
                AffectedDestinationAddresses.UnionWith(keys.AffectedDestinationAddresses);
                AffectedSourceAddresses.UnionWith(keys.AffectedSourceAddresses);
                AffectedRegKeys.UnionWith(keys.AffectedRegKeys);
                AffectedRegValues.UnionWith(keys.AffectedRegValues);
            }
        }

        public override string ToString()
        {
            return (AffectedDestinationPorts.Count == 0 ? "" : "dports=" + String.Join(",", AffectedDestinationPorts))
                + (AffectedSourcePorts.Count == 0 ? "" : "; sports=" + String.Join(",", AffectedSourcePorts))
                + (AffectedDestinationAddresses.Count == 0 ? "" : "; daddresses=" + String.Join(",", AffectedDestinationAddresses))
                + (AffectedSourceAddresses.Count == 0 ? "" : "; saddresses=" + String.Join(",", AffectedSourceAddresses))
                + (AffectedRegKeys.Count == 0 ? "" : "; regKeys=" + String.Join(",", AffectedRegKeys))
                + (AffectedRegValues.Count == 0 ? "" : "; regValues=" + String.Join(",", AffectedRegValues));
        }

        private string ReplaceGuids(string source)
        {
            return Regex.Replace(source, @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", "$$GUID$$", RegexOptions.IgnoreCase);
        }
    }
}
