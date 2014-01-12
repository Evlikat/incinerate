using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncinerateUI
{
    class AgentStatus
    {
        public static AgentStatus Learning = new AgentStatus { Name = "Learning" };
        public static AgentStatus Ready = new AgentStatus { Name = "Ready" };
        public static AgentStatus Watching = new AgentStatus { Name = "Watching" };
        public static AgentStatus Unknown = new AgentStatus { Name = "Unknown" };

        private static IList<AgentStatus> All = new List<AgentStatus>()
        {
            Learning, Ready, Watching, Unknown
        };

        public string Name{ get; private set; }

        private AgentStatus() { }

        public static AgentStatus Parse(string statusName)
        {
            foreach (AgentStatus status in All)
            {
                if (String.Compare(status.Name, statusName) == 0)
                {
                    return status;
                }
            }
            return Unknown;
        }
    }
}
