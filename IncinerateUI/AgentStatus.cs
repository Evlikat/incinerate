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

        public string Name{ get; private set; }

        private AgentStatus() { }
    }
}
