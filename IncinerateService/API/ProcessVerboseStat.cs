using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NeuroIncinerate.Neuro;

namespace IncinerateService.API
{
    [DataContract]
    public class ProcessVerboseStat
    {
        [DataMember]
        public IList<int> AffectedDestinationPorts { get; internal set; }
        [DataMember]
        public IList<int> AffectedSourcePorts { get; internal set; }
        [DataMember]
        public IList<int> AffectedDestinationAddresses { get; internal set; }
        [DataMember]
        public IList<int> AffectedSourceAddresses { get; internal set; }
        [DataMember]
        public IList<string> AffectedRegKeys { get; internal set; }
        [DataMember]
        public IList<string> AffectedRegValues { get; internal set; }

        internal ProcessVerboseStat(AffectedKeys affected)
        {
            AffectedDestinationPorts = new List<int>(affected.AffectedDestinationPorts);
            AffectedSourcePorts = new List<int>(affected.AffectedSourcePorts);
            AffectedDestinationAddresses = new List<int>(affected.AffectedDestinationAddresses);
            AffectedSourceAddresses = new List<int>(affected.AffectedSourceAddresses);
            AffectedRegKeys = new List<string>(affected.AffectedRegKeys);
            AffectedRegValues = new List<string>(affected.AffectedRegValues);
        }
    }
}
