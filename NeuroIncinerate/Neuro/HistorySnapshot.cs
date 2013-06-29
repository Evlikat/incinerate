using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    [Serializable]
    public class HistorySnapshot
    {
        public HistorySnapshot()
        {
        }

        public HistorySnapshot(SnapshotReadyEventArgs args)
        {
            Events = args.Events;
            PID = args.PID;
        }

        public HistorySnapshot(IPID pid, IList<IProcessAction> list)
        {
            PID = pid;
            Events = list;
        }

        public IList<IProcessAction> Events { get; set; }
        public IPID PID { get; set; }

        public void AddEvents(IList<IProcessAction> additionalEvents)
        {
            foreach (IProcessAction action in additionalEvents)
                Events.Add(action);
        }

        public HistorySnapshot Sub(int from, int to)
        {
            IList<IProcessAction> list = new List<IProcessAction>();
            for (int i = from; i < to; i++)
            {
                list.Add(Events[i]);
            }
            return new HistorySnapshot(PID, list);
        }

        public static IEnumerable<HistorySnapshot> Divide(int snapshotLength, HistorySnapshot sourceSnapshot)
        {
            int parts = sourceSnapshot.Events.Count / snapshotLength;
            for (int i = 0; i < parts; i++)
            {
                yield return sourceSnapshot.Sub(i * snapshotLength, (i + 1) * snapshotLength);
            }
        }
    }
}
