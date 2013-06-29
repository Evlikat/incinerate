using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NeuroIncinerate.Neuro;
using NeuroIncinerate;
using System.IO;
using NeuroIncinerate.Base;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace IncinerateTest
{
    [TestFixture]
    public class FileSaverTest
    {
        public static void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        [Test]
        public void TestSave()
        {
            string mainPath = @"c:\etl2";
            string[] expFileNames = Directory.GetFiles(mainPath, "*explorer*");
            string[] allFileNames = Directory.GetFiles(mainPath);
            IEnumerable<string> restFileNames = allFileNames.Where(name => !expFileNames.Contains(name));
            Console.WriteLine("{0} of {1} are Explorer", expFileNames.Count(), allFileNames.Count());

            SnapshotFileSaver saver = new SnapshotFileSaver(mainPath);
            IList<LearningPair> pairs = new List<LearningPair>();
            foreach (string fileName in expFileNames)
            {
                HistorySnapshot snapshot = saver.Load(fileName);
                pairs.Add(new LearningPair(snapshot, true));
            }
            foreach (string fileName in restFileNames)
            {
                HistorySnapshot snapshot = saver.Load(fileName);
                pairs.Add(new LearningPair(snapshot, false));
            }
            Console.WriteLine("=== " + pairs.Count + " pairs have been loaded");
            Shuffle(pairs);

            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(),
                LearningPair.EventNameSpacePower, new int[] { 18, 12, 2 });
            network.Randomize();
            BackPropagationLearning trainer = new BackPropagationLearning(network);
            foreach (ILearningPair pair in pairs)
            {
                trainer.Run(pair.Input, pair.Output);
            }

            int successes = 0;
            int error1 = 0;
            int error2 = 0;
            foreach (ILearningPair pair in pairs)
            {
                double[] output = network.Compute(pair.Input);
                Console.WriteLine("ACT: " + output[0] + " " + output[1]);
                Console.WriteLine("EXP: " + pair.Output[0] + " " + pair.Output[1]);
                if (output[0] > output[1])
                    if (pair.Output[0] > pair.Output[1])
                        successes++;
                    else
                        error1++;
                else
                    if (pair.Output[0] < pair.Output[1])
                        successes++;
                    else
                        error2++;
            }
            Console.WriteLine("Success count: {0} of {1}", successes, pairs.Count);
            Console.WriteLine("Error 1: {0} of {1}", error1, pairs.Count);
            Console.WriteLine("Error 2: {0} of {1}", error2, pairs.Count);
        }
    }
}
