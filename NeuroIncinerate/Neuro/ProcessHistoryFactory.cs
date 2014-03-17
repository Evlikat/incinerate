using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    interface IProcessHistoryFactory
    {
        IProcessHistory CreateProcessHistory(IPID processID);
    }

    class ProcessHistoryFactory : IProcessHistoryFactory
    {
        public const int Limit = 50;

        public IProcessHistory CreateProcessHistory(IPID processID)
        {
            return new LimitedProcessHistory(processID, Limit);
        }
    }
}
