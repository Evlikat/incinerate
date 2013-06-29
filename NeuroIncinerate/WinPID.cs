using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate
{
    [Serializable]
    public class WinPID : IPID
    {
        public int PID { get; private set; }
        public string Name { get; private set; }

        public WinPID(int pid, string name)
        {
            this.PID = pid;
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            WinPID other = (WinPID)obj;
            return other.PID == PID;
        }

        public override int GetHashCode()
        {
            return PID.GetHashCode();
        }

        public int CompareTo(IPID other)
        {
            if (other == null || other.GetType() != typeof(WinPID))
            {
                return 1;
            }
            WinPID another = (WinPID) other;
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

        public override string ToString()
        {
            return PID.ToString();
        }
    }
}
