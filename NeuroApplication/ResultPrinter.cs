using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro.Multi;
using System.IO;

namespace NeuroApplication
{
    class ResultPrinter
    {
        private StreamWriter writer;
        private ConsoleWriter console;
        public string Prefix = "";

        public ResultPrinter(StreamWriter writer, ConsoleWriter console)
        {
            this.writer = writer;
            this.console = console;
        }

        public void PrintResult(IList<ComputationFinishedEventArgs> results)
        {
            foreach (ComputationFinishedEventArgs e in results)
            {
                int successCount = 0;
                int total = 0;
                foreach (KeyValuePair<IMultiNetworkComputationResult, bool> resultPair in e.Results)
                {
                    if (ExperimentWatcher.WasSuccessed(resultPair.Key, resultPair.Value))
                        successCount++;

                    total++;
                }
                string res = String.Format(Prefix + "Successes: {0} of {1}: {2:0.00}%", successCount, total, (double)successCount / total * 100);
                writer.WriteLine(res);
                console.WriteLine(res);
            }
        }

        internal void Close()
        {
            writer.Close();
        }
    }
}
