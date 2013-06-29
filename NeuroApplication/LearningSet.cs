using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;

namespace NeuroApplication
{
    class LearningSet
    {
        private IList<HistorySnapshot> Snapshots { get; set; }
        public IList<HistorySnapshot> Learning { get; private set; }
        public IList<HistorySnapshot> Checking { get; private set; }
        public IList<HistorySnapshot> Testing { get; private set; }
        public IList<HistorySnapshot> Target { get; private set; }
        public IList<HistorySnapshot> NonTarget { get; private set; }
        private static Random r = new Random();

        public LearningSet(IList<HistorySnapshot> snapshots, string targetProcessName)
        {
            Snapshots = snapshots;
            Target = new List<HistorySnapshot>();
            NonTarget = new List<HistorySnapshot>();
            Learning = new List<HistorySnapshot>();
            Checking = new List<HistorySnapshot>();
            Testing = new List<HistorySnapshot>();
            foreach (HistorySnapshot snapshot in Snapshots)
            {
                if (snapshot.PID.Name.Contains(targetProcessName))
                {
                    Target.Add(snapshot);
                }
                else
                {
                    NonTarget.Add(snapshot);
                }
                GetList().Add(snapshot);
            }
        }

        private IList<HistorySnapshot> GetList()
        {
            switch(r.Next(3))
            {
                case 0: return Learning;
                case 1: return Checking;
                case 2: return Testing;
            }
            return null;
        }
    }
}
