﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IncinerateService.Core
{
    interface IStrategy
    {
        void Apply(double res, int pid);
    }

    class DoNothingStrategy : IStrategy
    {
        public void Apply(double res, int pid) { }
    }

    class AlertStrategy : IStrategy
    {
        public void Apply(double res, int pid)
        {
            Console.WriteLine("Обнаружен запрещенный процесс: {0} [{1:0.000000}]", pid, res);
        }
    }

    class AlarmStrategy : IStrategy
    {
        public void Apply(double res, int pid)
        {
            Console.WriteLine("Зафиксирована аномальная активность для: {0} [{1:0.000000}]", pid, res);
        }
    }

    class HitTerminateStrategy : IStrategy
    {
        object m_Sync;
        int hits = 0;
        int m_MaxHits;

        public HitTerminateStrategy(int max)
        {
            if (max < 1)
            {
                throw new ArgumentOutOfRangeException("max :" + max + ". It should be at least 1");
            }
            m_MaxHits = max;
        }

        public void Apply(double res, int pid)
        {
            lock (m_Sync)
            {
                hits++;
                Console.WriteLine("Обнаружен запрещенный процесс {0} [{1:0.000000}]. hits: {2}", pid, res, hits);
                if (hits >= m_MaxHits)
                {
                    Console.WriteLine("Завершение процесса {0}...", pid);
                    Process targetProcess = Process.GetProcessById(pid);
                    if (targetProcess != null)
                    {
                        targetProcess.Kill();
                    }
                    hits = 0;
                }
            }
        }
    }
}
