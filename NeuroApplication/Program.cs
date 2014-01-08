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
            //ExperimentSample ex1 = new DifferentProcessesExperiment(@"c:\etl3", @"c:\res3.txt");
            //ex1.Run();
            //ExperimentSample ex2 = new DifferentProcessesExperiment(@"c:\etl2", @"c:\res2.txt");
            //ex2.Run();
            //ExperimentSample ex3 = new DifferentProcessesExperiment(@"c:\etl", @"c:\res.txt");
            //ex3.Run();
            //ExperimentSample exr = new DifferentProcessesExperiment(@"c:\etlr", @"c:\resr.txt");
            //exr.Run();
            //ExperimentSample exo = new DifferentProcessesExperiment(@"c:\etlo", @"c:\reso.txt");
            //exo.Run();
            EnvironmentExperiment envEx = new EnvironmentExperiment(@"c:\etlo", @"c:\res\res.csv");
            envEx.Run();
            //SnapshotLengthExperiment snExp = new SnapshotLengthExperiment("ekrn", @"c:\etlo", @"c:\res\ekrn.csv");
            //snExp.Run();
            Console.In.Peek();
        }
    }
}
