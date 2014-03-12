using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    interface IGuardianAgentsPool
    {
        void AddGuardian(Agent agent, string process,
            IStrategy redStrategy, IStrategy yellowStrategy, double p1, double p2);

        bool Stop(string name);

        IEnumerable<AgentReaction> Compute(HistorySnapshot snapshot);

        ICollection<GuardianAgentSession> GetAll();
    }

    class AgentReaction
    {
        public double Res { get; private set; }
        public int PID { get; private set; }
        public IStrategy Strategy { get; private set; }

        public AgentReaction(double res, int pid, IStrategy strategy)
        {
            this.Res = res;
            this.PID = pid;
            this.Strategy = strategy;
        }

        internal void Apply()
        {
            Strategy.Apply(Res, PID);
        }
    }
}
