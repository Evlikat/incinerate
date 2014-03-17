using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IncinerateService.API
{
    [DataContract]
    public class ProcessStatInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int PID { get; set; }
        [DataMember]
        public int DiskFileActivity { get; set; }
        [DataMember]
        public int NetActivity { get; set; }
        [DataMember]
        public int RegistryActivity { get; set; }
    }
}
