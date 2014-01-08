﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace IncinerateService.Core
{
    interface IStrategy
    {
        void Apply(int pid);
    }

    class DoNothingStrategy : IStrategy
    {
        public void Apply(int pid) { }
    }

    class AlertStrategy : IStrategy
    {

        public void Apply(int pid)
        {
            Console.WriteLine("Обнаружен запрещенный процесс: {0}", pid);
        }
    }

    class HitTerminateStrategy : IStrategy
    {
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

        public void Apply(int pid)
        {
            hits++;
            Console.WriteLine("Обнаружен запрещенный процесс {0}. hits: {1}", pid, hits);
            if (hits >= m_MaxHits)
            {
                Console.WriteLine("Завершение процесса {0}...", pid);
                Process targetProcess = Process.GetProcessById(pid);
                targetProcess.Kill();
            }
        }
    }
}