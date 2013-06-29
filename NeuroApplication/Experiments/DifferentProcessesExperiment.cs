using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using System.IO;

namespace NeuroApplication.Experiments
{
    class DifferentProcessesExperiment : ExperimentSample
    {
        public override void Run()
        {
            ConsoleWriter writer = new ConsoleWriter();
            ResultPrinter resultPrinter = new ResultPrinter(new StreamWriter(@"c:\results.txt"), writer);
            SnapshotLoader loader = new SnapshotLoader(@"c:\etl");
            IList<HistorySnapshot> snapshots = loader.LoadSnapshots();
            ISet<string> names = ProcessNamesExtractor.GetProcessNames(snapshots);

            foreach (string name in names)
            {
                //Console.WriteLine(name);
                ExperimentWatcher experiment = new ExperimentWatcher(writer, 5, name, snapshots);
                experiment.Start();
                resultPrinter.Prefix = name + ": ";
                resultPrinter.PrintResult(experiment.Results);
            }
            resultPrinter.Close();
            Console.In.Peek();
        }
    }

    class ProcessNamesExtractor
    {
        public static ISet<string> GetProcessNames(IList<HistorySnapshot> snapshots)
        {
            ISet<string> names = new HashSet<string>();
            foreach (HistorySnapshot snapshot in snapshots)
            {
                names.Add(snapshot.PID.Name);
            }
            return names;
        }
    }
}
