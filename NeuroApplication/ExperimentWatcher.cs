using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro.Multi;
using NeuroIncinerate.Neuro;

namespace NeuroApplication
{

    class ExperimentWatcher
    {
        public ConsoleWriter Writer { get; private set; }
        public Experiment ExperimentInstance { get; private set; }

        private IList<ComputationFinishedEventArgs> m_Results = new List<ComputationFinishedEventArgs>();

        public IList<ComputationFinishedEventArgs> Results { get { return m_Results; } }

        public ExperimentWatcher(ConsoleWriter writer, int snapshotLength, string targetProcessName, IList<HistorySnapshot> sourceSnapshots)
        {
            Writer = writer;
            Writer.LineIndex++;
            ExperimentInstance = new Experiment(snapshotLength, targetProcessName, sourceSnapshots);
            ExperimentInstance.ComputationStarted += new EventHandler(ExperimentInstance_ComputationStarted);
            ExperimentInstance.LoadingStarted += new EventHandler(ExperimentInstance_LoadingStarted);
            ExperimentInstance.LoadingFinished += new EventHandler(ExperimentInstance_LoadingFinished);
            ExperimentInstance.SnapshotLoaded += new EventHandler<ProgressEventArgs>(ExperimentInstance_SnapshotLoaded);
            ExperimentInstance.Trained += new EventHandler<ProgressEventArgs>(ExperimentInstance_Trained);
            ExperimentInstance.TrainingFinished += new EventHandler(ExperimentInstance_TrainingFinished);
            ExperimentInstance.TrainingStarted += new EventHandler(ExperimentInstance_TrainingStarted);
            ExperimentInstance.ResultObtained += new EventHandler<ProgressEventArgs>(ExperimentInstance_ResultObtained);
            ExperimentInstance.ComputationFinished += new EventHandler<ComputationFinishedEventArgs>(ExperimentInstance_ComputationFinished);
        }

        public void Start()
        {
            ExperimentInstance.Start();
        }

        public void PrintResult()
        {
            foreach (ComputationFinishedEventArgs e in m_Results)
            {
                int successYes = 0;
                int successNo = 0;
                int wrongYes = 0;
                int wrongNo = 0;
                int total = 0;
                foreach (KeyValuePair<IMultiNetworkComputationResult, bool> resultPair in e.Results)
                {
                    if (resultPair.Value)
                    {
                        if (WasSuccessed(resultPair.Key, resultPair.Value))
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
                        if (WasSuccessed(resultPair.Key, resultPair.Value))
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
                Writer.WriteLine("Successes: {0} ({5}/{6}) of {1}: {2:0.00}%. Wrong 'No': {3}. Wrong 'Yes': {4}",
                    successCount, total, (double)successCount / total * 100,
                    wrongNo, wrongYes, successNo, successYes);
            }
        }

        void ExperimentInstance_ResultObtained(object sender, ProgressEventArgs e)
        {
            Writer.WriteAt(String.Format("Computed: {0} of {1} [{2:0.00}%]", e.Count, e.TotalCount, (double)e.Count / e.TotalCount * 100), 0, Writer.LineIndex);
        }

        void ExperimentInstance_LoadingFinished(object sender, EventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("");
        }

        void ExperimentInstance_ComputationFinished(object sender, ComputationFinishedEventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("");
            m_Results.Add(e);
        }

        void ExperimentInstance_TrainingStarted(object sender, EventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("Training...");
        }

        void ExperimentInstance_TrainingFinished(object sender, EventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("");
            Writer.WriteLine("Trained");
        }

        void ExperimentInstance_Trained(object sender, ProgressEventArgs e)
        {
            Writer.WriteAt(String.Format("Trained: {0} of {1} [{2:0.00}%]", e.Count, e.TotalCount, (double)e.Count / e.TotalCount * 100), 0, Writer.LineIndex);
        }

        void ExperimentInstance_SnapshotLoaded(object sender, ProgressEventArgs e)
        {
            Writer.WriteAt(String.Format("Loaded: {0} of {1} [{2:0.00}%]", e.Count, e.TotalCount, (double)e.Count / e.TotalCount * 100), 0, Writer.LineIndex);
        }

        void ExperimentInstance_LoadingStarted(object sender, EventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("Loading...");
        }

        void ExperimentInstance_ComputationStarted(object sender, EventArgs e)
        {
            Writer.LineIndex++;
            Writer.WriteLine("Computing...");
        }

        public static bool WasSuccessed(IMultiNetworkComputationResult result, bool expectedYes)
        {
            return (result.Result[0] - result.Result[1] > 0.1) ^ !expectedYes;
        }
    }
}
