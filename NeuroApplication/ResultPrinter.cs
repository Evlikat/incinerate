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
                int successYes = 0;
                int successNo = 0;
                int wrongYes = 0;
                int wrongNo = 0;
                int expectedYes = 0;
                int expectedNo = 0;
                int total = 0;
                IList<double> resYes = new List<double>();
                IList<double> resNo = new List<double>();
                foreach (KeyValuePair<IMultiNetworkComputationResult, bool> resultPair in e.Results)
                {
                    if (resultPair.Value)
                    {
                        resYes.Add(resultPair.Key.Result[0] - resultPair.Key.Result[1]);
                        expectedYes++;
                        if (ExperimentWatcher.WasSuccessed(resultPair.Key, resultPair.Value))
                        {
                            successYes++;
                        }
                        else
                        {
                            wrongNo++;
                        }
                    }
                    else
                    {
                        resNo.Add(resultPair.Key.Result[0] - resultPair.Key.Result[1]);
                        expectedNo++;
                        if (ExperimentWatcher.WasSuccessed(resultPair.Key, resultPair.Value))
                        {
                            successNo++;
                        }
                        else
                        {
                            wrongYes++;
                        }
                    }
                    total++;
                }
                int successCount = successNo + successYes;
                string res = String.Format(
                    Prefix + "{0} of {1}({9} + {10}). Eff = {2:0.00}%. Errors {3} ({4:0.00}%) = {5}({6:0.00}%) + {7}({8:0.00}%)",
                    successCount, total, (double)successCount / total * 100,
                    wrongYes + wrongNo, (double)(wrongYes + wrongNo) / total * 100,
                    wrongNo, (double)wrongNo / expectedNo * 100,
                    wrongYes, (double)wrongYes / expectedYes * 100,
                    expectedNo, expectedYes
                    );
                console.WriteLine(res);
                string resTable = String.Format(
                    Prefix + "{0},{1},{2:0.00}%,{5},{6:0.00}%,{7},{8:0.00}%",
                    successCount, total, (double)successCount / total * 100,
                    wrongYes + wrongNo, (double)(wrongYes + wrongNo) / total * 100,
                    wrongNo, (double)wrongNo / expectedNo * 100,
                    wrongYes, (double)wrongYes / expectedYes * 100,
                    expectedNo, expectedYes
                    );
                writer.WriteLine(resTable);
            }
        }

        internal void Close()
        {
            writer.Close();
        }
    }
}
