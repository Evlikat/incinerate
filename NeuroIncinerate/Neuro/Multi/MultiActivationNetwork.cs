﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Neuro;
using AForge.Neuro.Learning;
using NLog;

namespace NeuroIncinerate.Neuro.Multi
{
    [Serializable]
    public class MultiActivationNetwork
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        [NonSerialized]
        private IDictionary<Type, BackPropagationLearning> m_TrainerMap;

        private IList<Type> EventTypeList { get; set; }
        private IDictionary<Type, ActivationNetwork> NetworkMap { get; set; }

        private IDictionary<Type, BackPropagationLearning> TrainerMap { get { return m_TrainerMap; } }
        private IEventSignificator Significator { get; set; }

        public INetworkTrustRegistry TrustVector { get; set; }
        public string TargetProcessName { get; private set; }

        public AffectedKeys LearnedAffectedKeys { get; private set; }

        public MultiActivationNetwork(string targetProcessName)
        {
            TargetProcessName = targetProcessName;
            Significator = new Significator();
            EventTypeList = new List<Type>(Significator.EventTypeList);
            TrustVector = new UniformNetworkTrustVector(EventTypeList.Count);
            NetworkMap = new Dictionary<Type, ActivationNetwork>();
            m_TrainerMap = new Dictionary<Type, BackPropagationLearning>();
            LearnedAffectedKeys = new AffectedKeys();
            
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
            Log.Info((isTargetProcess ? "Target: " : "Non-target: ") + snapshot.ToString());
            IDictionary<Type, TypedLearningPair> learningDict;
            if (isTargetProcess)
            {
                learningDict = GetPairs(snapshot, true);
                LearnedAffectedKeys.UnionWith(snapshot.AffectedKeys);
            }
            else
            {
                //learningDict = new Dictionary<Type, TypedLearningPair>();
                //learningDict = GetPairs(snapshot, false);
                learningDict = GetRandomNegativePairs(snapshot);
            }
            foreach (KeyValuePair<Type, TypedLearningPair> typePairPair in learningDict)
            {
                TrainerMap[typePairPair.Key].Run(Normalize(typePairPair.Value.Input), typePairPair.Value.Output);
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
                        NetworkMap[typePairPair.Key].Compute(
                            Normalize(
                                typePairPair.Value.Values
                                )
                            ),
                            typePairPair.Key.Name);
                results.Add(resultEntry);
            }
            return new MultiNetworkComputationResult(results, TrustVector);
        }

        private double[] Normalize(double[] input)
        {
            double[] normalizedInput = new double[input.Length];
            double sum = 0.0;
            foreach (double d in input)
            {
                sum += d * d;
            }
            double vectorLen = Math.Sqrt(sum);
            for (int i = 0; i < input.Length; i++)
            {
                normalizedInput[i] = input[i] / vectorLen;
            }
            return normalizedInput;
        }

        private IDictionary<Type, TypedLearningPair> GetPairs(HistorySnapshot snapshot, bool isTarget)
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
                    pair = new TypedLearningPair(type, isTarget);
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

        private IDictionary<Type, TypedLearningPair> GetRandomNegativePairs(HistorySnapshot snapshot)
        {
            IDictionary<Type, TypedLearningPair> learningDict = new Dictionary<Type, TypedLearningPair>();
            int count = snapshot.Events.Count;
            int types = Enum.GetNames(typeof(EventName)).Length;
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                string name = ((EventName) rnd.Next(types)).ToString();
                Type type = Significator.ToSignificantType(name);
                if (type == null)
                    continue;

                TypedLearningPair pair;
                if (!learningDict.ContainsKey(type))
                {
                    pair = new TypedLearningPair(type, false);
                    learningDict.Add(type, pair);
                }
                else
                {
                    pair = learningDict[type];
                }
                pair.Modify(Significator.ToSignificant(name));
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
