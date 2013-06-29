using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IncinerateService.Core
{
    interface IAgentStorage
    {
        void SaveAgent(string name, Agent agent);

        Agent LoadAgent(string name);

        IList<string> GetAgentNames();
    }
}
