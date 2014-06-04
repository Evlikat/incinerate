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
            int yes = 0;
            double yesSum = 0.0;
            int no = 0;
            double noSum = 0.0;
            foreach (ComputationFinishedEventArgs e in results)
            {
                foreach (KeyValuePair<IMultiNetworkComputationResult, bool> resultPair in e.Results)
                {
                    double res = resultPair.Key.Result[0] - resultPair.Key.Result[1];
                    if (resultPair.Value)
                    {
                        yes++;
                        yesSum += res;
                    }
                    else
                    {
                        no++;
                        noSum += res;
                    }
                }
            }
            double yesAvg = yesSum / yes;
            double noAvg = noSum / no;

            double part = (double) no / (yes + no);
            double threshold = (part * yesAvg + (1.0 - part) * noAvg);
            double threshold2 = ((no * yesSum) / (yes * (no + yes))) + ((yes * noSum) / (no * (no + yes)));

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
                        if (ExperimentWatcher.WasSuccessed(resultPair.Key, threshold, resultPair.Value))
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
                        if (ExperimentWatcher.WasSuccessed(resultPair.Key, threshold, resultPair.Value))
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
                    Prefix + "{0} of {1}({7} + {8}). Eff = {2:0.00}%. Errors {3} ({4:0.00}%) = {5} + {6}, [th={9:0.0000}, yes={10:0.0000}, no={11:0.0000}]",
                    successCount, total, (double)successCount / total * 100,
                    wrongYes + wrongNo, (double)(wrongYes + wrongNo) / total * 100,
                    wrongNo, wrongYes, expectedNo, expectedYes,
                    threshold, yesAvg, noAvg
                    );
                console.WriteLine(res);
                string resTable = String.Format(
                    Prefix + "{0},{1},{2},{3},{4},{5:0.00}%",
                    total, successYes, successNo, wrongNo, wrongYes, (double)successCount / total * 100
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
