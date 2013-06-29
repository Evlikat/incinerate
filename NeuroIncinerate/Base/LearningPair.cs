using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate.Neuro;

namespace NeuroIncinerate.Base
{
    public class LearningPair : ILearningPair
    {
        public static int EventNameSpacePower = Enum.GetValues(typeof(EventName)).Length;
        private double[] input;
        private double[] output = new double[2];

        public double[] Input
        {
            get { return input; }
        }

        public double[] Output
        {
            get { return output; }
        }

        public LearningPair(HistorySnapshot snapshot, bool isExplorer)
        {
            input = new double[EventNameSpacePower];
            foreach (IProcessAction action in snapshot.Events)
            {
                EventName eventName = (EventName)Enum.Parse(typeof(EventName), action.EventName);
                input[(int)eventName]++;
            }
            output[isExplorer ? 0 : 1] = 1;
            output[isExplorer ? 1 : 0] = 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Input.Length; i++)
            {
                if (i % 8 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append(Input[i]).Append("\t");
            }
            sb.AppendFormat("=\t{0}\t{1}", output[0], output[1]);
            return sb.ToString();
        }
    }
}
