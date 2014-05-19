using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate.Neuro
{
    [Serializable]
    public class HistorySnapshot
    {
        public string LegacyProcessName
        {
            get
            {
                if (String.IsNullOrEmpty(ProcessName))
                {
                    return PID.Name;
                }
                return ProcessName;
            }
        }

        public string ProcessName { private get; set; }

        public AffectedKeys AffectedKeys
        {
            get
            {
                AffectedKeys keys = new AffectedKeys();
                foreach (IProcessAction action in Events)
                {
                    keys.UnionWith(action.AffectedKeys);
                }
                return keys;
            }
        }

        public HistorySnapshot()
        {
        }

        public HistorySnapshot(SnapshotReadyEventArgs args)
            : this()
        {
            Events = args.Events;
            PID = args.PID;
        }

        public HistorySnapshot(IPID pid, IList<IProcessAction> list)
            : this()
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
            HistorySnapshot hs = new HistorySnapshot(PID, list);
            hs.ProcessName = this.ProcessName;
            return hs;
        }

        public static IEnumerable<HistorySnapshot> Divide(int snapshotLength, HistorySnapshot sourceSnapshot)
        {
            int parts = sourceSnapshot.Events.Count / snapshotLength;
            for (int i = 0; i < parts; i++)
            {
                yield return sourceSnapshot.Sub(i * snapshotLength, (i + 1) * snapshotLength);
            }
        }

        public override string ToString()
        {            
            IDictionary<String, int> counter = new Dictionary<String, int>();
            foreach (IProcessAction action in Events)
            {
                if (counter.ContainsKey(action.EventName))
                {
                    counter[action.EventName]++;
                }
                else
                {
                    counter.Add(action.EventName, 1);
                }
            }
            return String.Join(",", counter.Select(item => item.Key + "=" + item.Value));
        }
    }
}
