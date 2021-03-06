﻿using System;
using System.Collections.Generic;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;

namespace IncinerateService.Core
{
    class AgentRegistry
    {
        public const int MinPositive = 500;
        public const int MinNegative = 2500;
        public const double P1 = 0.17;
        public const double P2 = 0.5;

        ILearningAgentPool m_LearningAgents = new MultiLearningAgentPool();
        IWatchingAgentsPool m_WatchingAgents = new WatchingAgentPool();
        IGuardianAgentsPool m_GuardianAgents = new GuardianAgentsPool();
        GlobalHistory m_History;

        public AgentRegistry(GlobalHistory history)
        {
            this.m_History = history;
        }

        public ICollection<Agent> Handle(IPID iPID, IList<IProcessAction> actions)
        {
            HistorySnapshot snapshot = new HistorySnapshot(iPID, actions);

            IRecognizedAgent recognized = m_WatchingAgents.Compute(snapshot);
            if (recognized.Apply(iPID.PID))
            {
                m_History.SetDynamicName(iPID, recognized.Name, recognized.MaxRes);
            }

            IEnumerable<AgentReaction> reactions = m_GuardianAgents.Compute(snapshot);
            foreach (AgentReaction reaction in reactions)
            {
                reaction.Apply();
            }
            
            return m_LearningAgents.TrainAll(iPID, snapshot);
        }

        public void CreateLearningAgent(LearningConfig learningConfig)
        {
            LearningAgent newAgent = new LearningAgent(
                learningConfig.Name,
                learningConfig.NativePids,
                learningConfig.ForeignPids,
                MinPositive,
                MinNegative);
            m_LearningAgents.AddAgent(newAgent);
        }

        public bool StopLearning(string name)
        {
            return m_LearningAgents.RemoveAgent(name);
        }

        public ICollection<LearningAgent> GetLearningAgents()
        {
            return m_LearningAgents.GetAll();
        }

        public ICollection<WatchingAgentSession> GetWatchingAgents()
        {
            return m_WatchingAgents.GetAll();
        }

        public ICollection<GuardianAgentSession> GetGuardianAgents()
        {
            return m_GuardianAgents.GetAll();
        }

        public void AddWatcher(Agent agent, IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2)
        {
            m_WatchingAgents.AddWatcher(agent, redStrategy, yellowStrategy, p1, p2);
        }

        public bool StopWatch(string name)
        {
            return m_WatchingAgents.Stop(name);
        }

        public void StopWatchAll()
        {
            m_WatchingAgents.StopAll();
        }

        public void AddGuardian(Agent agent, string processName, IStrategy redStrategy, IStrategy yellowStrategy,
            double p1, double p2)
        {
            m_GuardianAgents.AddGuardian(agent, processName, redStrategy, yellowStrategy, p1, p2);
        }

        public bool StopGuard(string name)
        {
            return m_GuardianAgents.Stop(name);
        }
    }
}
