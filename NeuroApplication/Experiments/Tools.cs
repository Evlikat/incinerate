using System.Collections.Generic;
using NeuroIncinerate.Neuro;

namespace NeuroApplication.Experiments
{
    class ProcessNamesExtractor
    {
        public static ISet<string> GetProcessNames(IList<HistorySnapshot> snapshots)
        {
            ISet<string> names = new HashSet<string>();
            foreach (HistorySnapshot snapshot in snapshots)
            {
                names.Add(snapshot.LegacyProcessName);
            }
            return names;
        }
    }
}