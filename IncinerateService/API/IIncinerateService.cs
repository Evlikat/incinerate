using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace IncinerateService.API
{
    [ServiceContract]
    public interface IIncinerateService
    {
        [OperationContract]
        void AddLearningAgent(IList<int> pid, string name);

        [OperationContract]
        IList<string> GetAgents();

        [OperationContract]
        void Watch(string name, string strategyRed, string strategyYellow, double p1, double p2);

        [OperationContract]
        void Stop();
    }
}
