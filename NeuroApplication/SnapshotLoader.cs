using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;
using System.IO;

namespace NeuroApplication
{
    class SnapshotLoader
    {
        string[] allFileNames;
        SnapshotFileSaver saver;
        public int MaxEvents { get; set; }

        public SnapshotLoader(string pathToSnapshotFolder)
        {
            MaxEvents = Int32.MaxValue;
            saver = new SnapshotFileSaver(pathToSnapshotFolder);
            allFileNames = Directory.GetFiles(pathToSnapshotFolder);
        }

        public IList<HistorySnapshot> LoadSnapshots()
        {
            Console.WriteLine("Loading files...");
            IList<HistorySnapshot> snapshots = new List<HistorySnapshot>();
            int eventsLoaded = 0;
            foreach (string fileName in allFileNames)
            {
                HistorySnapshot snapshot = saver.Load(fileName);
                snapshots.Add(snapshot);
                eventsLoaded += snapshot.Events.Count;
                if (eventsLoaded >= MaxEvents)
                {
                    break;
                }
            }
            Console.WriteLine("Loaded events: {0}", eventsLoaded);
            return snapshots;
        }
    }
}
