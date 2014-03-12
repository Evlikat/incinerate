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
        void RemoveLearningAgent(string name);

        [OperationContract]
        IList<AgentInfo> GetAgents();

        [OperationContract]
        void Watch(string name, string strategyRed, string strategyYellow, double p1, double p2);

        [OperationContract]
        void StopWatch(string name);

        [OperationContract]
        void Guard(string name, string process, string strategyRed, string strategyYellow, double e1, double e2);

        [OperationContract]
        void StopGuard(string name);

        [OperationContract]
        void Stop();
    }
}
