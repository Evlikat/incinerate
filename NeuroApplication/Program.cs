using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro.Multi;
using NeuroIncinerate.Neuro;
using System.IO;
using NeuroApplication.Experiments;

namespace NeuroApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ExperimentSample sample = new DifferentProcessesExperiment();
            sample.Run();
        }
    }
}
