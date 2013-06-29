using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroIncinerate
{
    public interface ILearningPair
    {
        double[] Input { get; }
        double[] Output { get; }
    }
}
