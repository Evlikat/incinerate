using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using System.IO;

namespace NeuroApplication.Experiments
{
    class EnvironmentExperiment : ExperimentSample
    {
        string source;
        string results;

        public EnvironmentExperiment(string source, string results)
        {
            this.source = source;
            this.results = results;
        }

        public override void Run()
        {
            ConsoleWriter writer = new ConsoleWriter();
            ResultPrinter resultPrinter = new ResultPrinter(new StreamWriter(results), writer);
            SnapshotLoader loader = new SnapshotLoader(source);
            IList<HistorySnapshot> snapshots = loader.LoadSnapshots();
            ISet<string> names = ProcessNamesExtractor.GetProcessNames(snapshots);

            foreach (string name in names)
            {
                //Console.WriteLine(name);
                ExperimentWatcher experiment = new ExperimentWatcher(writer, 20, name, snapshots);
                experiment.Start();
                resultPrinter.Prefix = name + ",";
                resultPrinter.PrintResult(experiment.Results);
            }
            resultPrinter.Close();
        }
    }
}
