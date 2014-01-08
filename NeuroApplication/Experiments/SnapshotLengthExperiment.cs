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
        string name;
        string results;
        string snapshotPath;

        public SnapshotLengthExperiment(string name, string snapshotPath, string results)
        {
            this.name = name;
            this.results = results;
            this.snapshotPath = snapshotPath;
        }
        
        public override void Run()
        {
            ConsoleWriter writer = new ConsoleWriter();
            ResultPrinter resultPrinter = new ResultPrinter(new StreamWriter(results), writer);
            SnapshotLoader loader = new SnapshotLoader(snapshotPath);
            IList<HistorySnapshot> shapshots = loader.LoadSnapshots();

            for (int snapshotLength = 100; snapshotLength >= 80; snapshotLength = snapshotLength - 10)
            {
                ExperimentWatcher experiment = new ExperimentWatcher(writer, snapshotLength, name, shapshots);
                experiment.Start();
                resultPrinter.Prefix = snapshotLength + ", ";
                resultPrinter.PrintResult(experiment.Results);
            }
            resultPrinter.Close();
        }
    }
}
