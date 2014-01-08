using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate
{
    [Serializable]
    class IntPID : IPID
    {
        public int PID
        {
            get;
            private set;
        }

        public string Name
        {
            get { return ""; }
        }

        public int CompareTo(IPID other)
        {
            if (other == null || other.GetType() != typeof(IntPID))
            {
                return 1;
            }
            IntPID another = (IntPID)other;
            if (this.PID > another.PID)
            {
                return 1;
            }
            if (this.PID == another.PID)
            {
                return 0;
            }
            return -1;
        }
    }
}
