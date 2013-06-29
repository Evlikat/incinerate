using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using System.IO;

namespace NeuroApplication.Experiments
{
    class SnapshotLengthExperiment : ExperimentSample
    {
        public override void Run()
        {
            ConsoleWriter writer = new ConsoleWriter();
            ResultPrinter resultPrinter = new ResultPrinter(new StreamWriter(@"c:\results.txt"), writer);
            SnapshotLoader loader = new SnapshotLoader(@"c:\etl");
            IList<HistorySnapshot> shapshots = loader.LoadSnapshots();

            for (int snapshotLength = 40; snapshotLength >= 5; snapshotLength = snapshotLength - 5)
            {
                ExperimentWatcher experiment = new ExperimentWatcher(writer, snapshotLength, "explorer", shapshots);
                experiment.Start();
                resultPrinter.Prefix = snapshotLength + ": ";
                resultPrinter.PrintResult(experiment.Results);
            }
            resultPrinter.Close();
            Console.In.Peek();
        }
    }
}
