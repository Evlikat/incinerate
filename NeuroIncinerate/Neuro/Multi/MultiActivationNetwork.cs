using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace NeuroIncinerate.Neuro.Multi
{
    [Serializable]
    public class MultiActivationNetwork
    {
        [NonSerialized]
        private IDictionary<Type, BackPropagationLearning> m_TrainerMap;

        private IList<Type> EventTypeList { get; set; }
        private IDictionary<Type, ActivationNetwork> NetworkMap { get; set; }

        private IDictionary<Type, BackPropagationLearning> TrainerMap { get { return m_TrainerMap; } }
        private IEventSignificator Significator { get; set; }

        public INetworkTrustRegistry TrustVector { get; set; }
        public string TargetProcessName { get; private set; }

        public MultiActivationNetwork(string targetProcessName)
        {
            TargetProcessName = targetProcessName;
            Significator = new Significator();
            EventTypeList = new List<Type>(Significator.EventTypeList);
            TrustVector = new UniformNetworkTrustVector(EventTypeList.Count);
            NetworkMap = new Dictionary<Type, ActivationNetwork>();
            m_TrainerMap = new Dictionary<Type, BackPropagationLearning>();
            
            foreach (Type type in EventTypeList)
            {
                PutNetworkAndTrainer(type);
            }
        }

        private void PutNetworkAndTrainer(Type eventType)
        {
            int count = Enum.GetValues(eventType).Length;
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), count, new int[] { count, 2 });
            NetworkMap.Add(eventType, network);
            BackPropagationLearning learning = new BackPropagationLearning(network);
            TrainerMap.Add(eventType, learning);
        }

        public void RunTrain(HistorySnapshot snapshot, bool isTargetProcess)
        {
            IDictionary<Type, TypedLearningPair> learningDict = GetPairs(snapshot, isTargetProcess);
            foreach (KeyValuePair<Type, TypedLearningPair> typePairPair in learningDict)
            {
                TrainerMap[typePairPair.Key].Run(typePairPair.Value.Input, typePairPair.Value.Output);
            }
        }

        public IMultiNetworkComputationResult Compute(HistorySnapshot snapshot)
        {
            IDictionary<Type, Input> learningDict = GetPairs(snapshot);
            IList<NetworkComputationResultEntry> results = new List<NetworkComputationResultEntry>();
            foreach (KeyValuePair<Type, Input> typePairPair in learningDict)
            {
                NetworkComputationResultEntry resultEntry =
                    new NetworkComputationResultEntry(
                        NetworkMap[typePairPair.Key].Compute(typePairPair.Value.Values), typePairPair.Key.Name);
                results.Add(resultEntry);
            }
            return new MultiNetworkComputationResult(results, TrustVector);
        }

        private IDictionary<Type, TypedLearningPair> GetPairs(HistorySnapshot snapshot, bool isTargetProcess)
        {
            IDictionary<Type, TypedLearningPair> learningDict = new Dictionary<Type, TypedLearningPair>();
            foreach (IProcessAction action in snapshot.Events)
            {
                Type type = Significator.ToSignificantType(action.EventName);
                if (type == null)
                    continue;

                TypedLearningPair pair;
                if (!learningDict.ContainsKey(type))
                {
                    pair = new TypedLearningPair(type, isTargetProcess);
                    learningDict.Add(type, pair);
                }
                else
                {
                    pair = learningDict[type];
                }
                pair.Modify(Significator.ToSignificant(action.EventName));
            }
            return learningDict;
        }

        private IDictionary<Type, Input> GetPairs(HistorySnapshot snapshot)
        {
            IDictionary<Type, Input> learningDict = new Dictionary<Type, Input>();
            foreach (IProcessAction action in snapshot.Events)
            {
                Type type = Significator.ToSignificantType(action.EventName);
                if (type == null)
                    continue;

                Input input;
                if (!learningDict.ContainsKey(type))
                {
                    input = new Input(Enum.GetValues(type).Length);
                    learningDict.Add(type, input);
                }
                else
                {
                    input = learningDict[type];
                }
                input.Modify(type, Significator.ToSignificant(action.EventName));
            }
            return learningDict;
        }
    }

    class TypedLearningPair : ILearningPair
    {
        private Input input;
        private double[] output = new double[2];
        public Type EventType { get; private set; }
        public int EventNameSpacePower { get; private set; }

        public double[] Input
        {
            get { return input.Values; }
        }

        public double[] Output
        {
            get { return output; }
        }

        public TypedLearningPair(Type eventType, bool isTargetProcess)
        {
            EventType = eventType;
            EventNameSpacePower = Enum.GetValues(eventType).Length;
            input = new Input(EventNameSpacePower);
            output[0] = isTargetProcess ? 1 : 0;
            output[1] = isTargetProcess ? 0 : 1;
        }

        public void Modify(string actionEventName)
        {
            input.Modify(EventType, actionEventName);
        }
    }

    class Input
    {
        double[] m_Values;

        public Input(int EventNameSpacePower)
        {
            this.m_Values = new double[EventNameSpacePower];
        }

        public double[] Values { get { return m_Values; } }

        public void Modify(Type eventType, string actionEventName)
        {
            object eventName = Enum.Parse(eventType, actionEventName);
            m_Values[(int)eventName]++;
        }
    }
}
