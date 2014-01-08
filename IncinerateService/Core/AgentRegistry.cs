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
        Agent m_WatchingAgent;
        IStrategy m_RedStrategy;
        IStrategy m_YellowStrategy;

        public ICollection<Agent> Handle(IPID iPID, IList<IProcessAction> actions)
        {
            HistorySnapshot snapshot = new HistorySnapshot(iPID, actions);
            
            if (m_WatchingAgent != null)
            {
                double result = m_WatchingAgent.Compute(snapshot);
                if (result > P1)
                {
                    if (result > P2)
                    {
                        m_RedStrategy.Apply(iPID.PID);
                    }
                    else
                    {
                        m_YellowStrategy.Apply(iPID.PID);
                    }
                }
            }

            return m_LearningAgents.TrainAll(iPID, snapshot);
        }

        public void CreateLearningAgent(LearningConfig learningConfig)
        {
            if (!m_LearningAgents.IsEmpty())
            {
                return;
            }
            LearningAgent newAgent = new LearningAgent(
                learningConfig.Name,
                learningConfig.NativePids,
                learningConfig.ForeignPids,
                MinPositive,
                MinNegative);
            m_LearningAgents.AddAgent(newAgent);
        }

        public void StopLearning()
        {
            m_LearningAgents.Clear();
        }

        public ICollection<LearningAgent> GetLearningAgents()
        {
            return m_LearningAgents.GetAll();
        }

        public void AddWatcher(Agent agent, IStrategy redStrategy, IStrategy yellowStrategy)
        {
            m_WatchingAgent = agent;
            m_RedStrategy = redStrategy;
            m_YellowStrategy = yellowStrategy;
        }

        public void StopWatch()
        {
            m_WatchingAgent = null;
        }
    }
}
