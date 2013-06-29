using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IncinerateService.Core
{
    interface IProcessStrategy
    {
        void Execute(int pid);
    }

    class PassStrategy : IProcessStrategy
    {
        public void Execute(int pid)
        {
            // pass
        }
    }

    class KillStrategy : IProcessStrategy
    {
        public void Execute(int pid)
        {
            Process process = Process.GetProcessById(pid);
            process.Kill();
        }
    }
}
