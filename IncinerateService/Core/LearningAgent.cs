﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeuroIncinerate;
using NeuroIncinerate.Neuro;
using NLog;

namespace IncinerateService.Core
{
    class LearningAgent
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();

        public const int MinPositiveTrained = 500;
        public const int MinNegativeTrained = 1500;
        enum State
        {
            Learning,
            Examing,
            Ready
        }

        State m_State = State.Learning;
        Agent m_Agent;
        int m_MinPositive;
        int m_MinNegative;
        int m_PositiveTrained = 0;
        int m_NegativeTrained = 0;

        public bool Ready
        {
            get { return m_State == State.Ready; }
        }

        public bool IsNative(IPID pid)
        {
            return m_Agent.Native(pid.PID);
        }

        public bool IsForeign(IPID pid)
        {
            return m_Agent.Foreign(pid.PID);
        }

        public LearningAgent(string name, ISet<IPID> native, ISet<IPID> foreign, int minPositive, int minNegative)
        {
            this.m_Agent = new Agent(name, native, foreign);
            this.m_MinPositive = minPositive;
            this.m_MinNegative = minNegative;
        }

        public void Train(HistorySnapshot snapshot, bool isTargetProcess)
        {
            m_Agent.Train(snapshot, isTargetProcess);
            if (isTargetProcess)
            {
                m_PositiveTrained++;
                if (m_PositiveTrained % 100 == 0)
                {
                    Log.Info("Обучено \"своих\" {0}", m_PositiveTrained);
                }
            }
            else
            {
                m_NegativeTrained++;
                if (m_NegativeTrained % 100 == 0)
                {
                    Log.Info("Обучено \"чужих\" {0}", m_NegativeTrained);
                }
            }
            if (m_PositiveTrained >= m_MinPositive && m_NegativeTrained >= m_MinNegative)
            {
                // I'm ready, master!
                m_State = State.Ready;
            }
        }

        public Agent TurnToAgent()
        {
            if (m_State == State.Ready)
            {
                return m_Agent;
            }
            // You are not prepared!
            throw new InvalidOperationException("Agent is not ready yet");
        }

        public string Name { get { return m_Agent.Name; } }
    }
}
