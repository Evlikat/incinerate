using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace IncinerateTest
{
    [TestFixture]
    class QuadraticEquationTest
    {
        class LearningPair
        {
            public double[] result;
            public double[] input;

            public LearningPair(double a, double b, double c)
            {
                input = new double[] { a, b, c };
                double D = b * b - 4 * a * c;
                if (D < 0)
                {
                    result = new double[] { 0, 1 };
                }
                else
                {
                    result = new double[] { 1, 0 };
                }
            }
        }
        private Random rnd = new Random();

        IEnumerable<LearningPair> GenerateEpoch(int length)
        {
            LearningPair[] result = new LearningPair[length];
            for (int i = 0; i < length; i++)
            {
                //double a = (rnd.NextDouble() - 0.5) * 100;
                //double b = (rnd.NextDouble() - 0.5) * 100;
                //double c = (rnd.NextDouble() - 0.5) * 100;
                int a = rnd.Next(-500, 501);
                int b = rnd.Next(-500, 501);
                int c = rnd.Next(-500, 501);
                yield return new LearningPair(a, b, c);
            }
        }

        [Test]
        public void QuadraticEquation()
        {
            string fileName = @"c:\Temp\network.nen";

            int inputsCount = 3;
            int[] neuronsCount = { 3, 3, 2 };

            ActivationNetwork network;
            try
            {
                network = (ActivationNetwork)ActivationNetwork.Load(fileName);
                Console.WriteLine("Loaded");
            }
            catch
            {
                network = new ActivationNetwork(new SigmoidFunction(), inputsCount, neuronsCount);
                Console.WriteLine("Created");
            }
            
            ISupervisedLearning trainer = new BackPropagationLearning(network);
            IEnumerable<LearningPair> learningPairs = GenerateEpoch(10000);

            foreach (LearningPair pair in learningPairs)
            {
                trainer.Run(pair.input, pair.result);
            }
            network.Save(fileName);

            int success = 0;
            int tryCount = 10000;
            for (int i = 0; i < tryCount; i++)
            {
                success += (Try(network)) ? 1 : 0;
            }
            Console.WriteLine(String.Format("Success count {0} of {1}", success, tryCount));
        }

        private bool Try(ActivationNetwork network)
        {
            int a = rnd.Next(-500, 501);
            int b = rnd.Next(-500, 501);
            int c = rnd.Next(-500, 501);
            LearningPair pair = new LearningPair(a, b, c);
            double[] output = network.Compute(pair.input);
            if (pair.result[0] == 1)
            {
                return output[0] > output[1];
            }
            else
            {
                return output[1] > output[0];
            }
        }
    }
}
