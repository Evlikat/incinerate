using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NeuroIncinerate.Neuro.Multi;
using NeuroIncinerate.Neuro;
using NLog;

namespace NeuroApplication
{
    class Experiment
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public event EventHandler LoadingStarted;
        public event EventHandler LoadingFinished;
        public event EventHandler TrainingStarted;
        public event EventHandler<ProgressEventArgs> SnapshotLoaded;
        public event EventHandler<ProgressEventArgs> Trained;
        public event EventHandler<ProgressEventArgs> ResultObtained;
        public event EventHandler TrainingFinished;
        public event EventHandler ComputationStarted;
        public event EventHandler<ComputationFinishedEventArgs> ComputationFinished;

        private IList<HistorySnapshot> SourceSnapshots;

        private int SnapshotLength { get; set; }
        private string TargetProcessName { get; set; }
        public int SnapshotLoadLimit { get; set; }

        public Experiment(int snapshotLength, string targetProcessName, IList<HistorySnapshot> sourceSnapshots) 
        {
            SnapshotLoadLimit = -1;
            SnapshotLength = snapshotLength;
            TargetProcessName = targetProcessName;
            SourceSnapshots = sourceSnapshots;
        }

        public void Start()
        {
            LoadingStarted(this, new EventArgs());

            MultiActivationNetwork multiNetwork = new MultiActivationNetwork(TargetProcessName);

            IList<HistorySnapshot> snapshots = LoadSnapshots(SourceSnapshots);
            snapshots = Shuffle(snapshots);
            LoadingFinished(this, new EventArgs());

            LearningSet learningSet = new LearningSet(snapshots, TargetProcessName);

            TrainLearningSet(multiNetwork, learningSet.Learning);
            INetworkTrustRegistry trustRegistry = CheckLearningSet(multiNetwork, learningSet.Checking);
            multiNetwork.TrustVector = trustRegistry;
            Log.Info("Applied trust vector: " + trustRegistry.ToString());
            TrainLearningSet(multiNetwork, learningSet.Checking);
            CheckLearningSet(multiNetwork, learningSet.Testing);
        }

        private void TrainLearningSet(MultiActivationNetwork multiNetwork, IList<HistorySnapshot> learningSet)
        {
            int i = 0;
            TrainingStarted(this, new EventArgs());
            //learningSet = RemoveDuplicates(learningSet);
            learningSet = Shuffle(learningSet);
            foreach (HistorySnapshot snapshot in learningSet)
            {
                bool isTargetProcess = snapshot.LegacyProcessName.Contains(multiNetwork.TargetProcessName);
                multiNetwork.RunTrain(snapshot, isTargetProcess);
                Trained(this, new ProgressEventArgs(++i, learningSet.Count));
            }
            TrainingFinished(this, new EventArgs());
        }

        private INetworkTrustRegistry CheckLearningSet(MultiActivationNetwork multiNetwork, IList<HistorySnapshot> checkingSet)
        {
            ComputationStarted(this, new EventArgs());
            IList<KeyValuePair<IMultiNetworkComputationResult, bool>> yesList =
                new List<KeyValuePair<IMultiNetworkComputationResult, bool>>();
            int i = 0;
            INetworkTrustRegistry trustRegistry = new NetworkTrustVector();
            foreach (HistorySnapshot snapshot in checkingSet)
            {
                bool expectedYes = snapshot.LegacyProcessName.Contains(TargetProcessName);
                IMultiNetworkComputationResult result = multiNetwork.Compute(snapshot);
                ResultObtained(this, new ProgressEventArgs(++i, checkingSet.Count));
                trustRegistry.ApplyTrustLevel(result.Results, expectedYes ? 0 : 1);
                yesList.Add(new KeyValuePair<IMultiNetworkComputationResult, bool>(result, expectedYes));
            }
            ComputationFinished(this, new ComputationFinishedEventArgs(yesList));
            return trustRegistry;
        }

        private IList<HistorySnapshot> LoadSnapshots(IList<HistorySnapshot> sourceSnapshots)
        {
            int i = 0;
            int Limit = SnapshotLoadLimit;
            IList<HistorySnapshot> snapshots = new List<HistorySnapshot>();
            foreach (HistorySnapshot sourceSnapshot in sourceSnapshots)
            {
                foreach (HistorySnapshot part in HistorySnapshot.Divide(SnapshotLength, sourceSnapshot))
                {
                    snapshots.Add(part);
                    ++i;
                    if (i == Limit)
                    {
                        SnapshotLoaded(this, new ProgressEventArgs(i, sourceSnapshot.Events.Count));
                        return snapshots;
                    }
                }
                SnapshotLoaded(this, new ProgressEventArgs(i, sourceSnapshot.Events.Count));
            }
            return snapshots;
        }

        private IList<HistorySnapshot> Shuffle(IList<HistorySnapshot> snapshots)
        {
            return new List<HistorySnapshot>(snapshots.OrderBy(a => Guid.NewGuid()));
        }

        private IList<HistorySnapshot> RemoveDuplicates(IList<HistorySnapshot> snapshots)
        {
            IList<HistorySnapshot> result = new List<HistorySnapshot>();
            foreach (HistorySnapshot ss in snapshots)
            {
                bool duplicate = false;
                foreach (HistorySnapshot other in snapshots)
                {
                    if (ss == other)
                    {
                        continue;
                    }
                    if (ss.Duplicates(other) && !other.LegacyProcessName.Equals(ss.LegacyProcessName))
                    {
                        duplicate = true;
                        break;
                    }
                }
                if (!duplicate)
                {
                    result.Add(ss);
                }
            }
            return result;
        }
    }
}
