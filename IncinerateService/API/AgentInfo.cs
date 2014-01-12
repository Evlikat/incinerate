using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace IncinerateService.API
{
    [DataContract]
    public class AgentInfo
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
