using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NLog;

namespace IncinerateService.Core
{
    interface IStrategy
    {
        void Apply(double res, int pid);
    }

    abstract class AbstractStrategy : IStrategy
    {
        protected Logger Log;

        public AbstractStrategy(Logger log)
        {
            this.Log = log;
        }

        public abstract void Apply(double res, int pid);
    }

    class DoNothingStrategy : AbstractStrategy
    {
        public DoNothingStrategy(Logger log) : base(log) { }

        public override void Apply(double res, int pid) { }
    }

    class AlertStrategy : AbstractStrategy
    {
        public AlertStrategy(Logger log) : base(log) { }

        public override void Apply(double res, int pid)
        {
            Log.Warn("Обнаружен запрещенный процесс: {0} [{1:0.000000}]", pid, res);
        }
    }

    class AlarmStrategy : AbstractStrategy
    {
        public AlarmStrategy(Logger log) : base(log) { }

        public override void Apply(double res, int pid)
        {
            Log.Warn("Зафиксирована аномальная активность для: {0} [{1:0.000000}]", pid, res);
        }
    }

    class HitTerminateStrategy : AbstractStrategy
    {
        object m_Sync = new object();
        int hits = 0;
        int m_MaxHits;

        public HitTerminateStrategy(int max, Logger log) : base(log)
        {
            if (max < 1)
            {
                throw new ArgumentOutOfRangeException("max :" + max + ". It should be at least 1");
            }
            m_MaxHits = max;
        }

        public override void Apply(double res, int pid)
        {
            lock (m_Sync)
            {
                hits++;
                Log.Warn("Обнаружен запрещенный процесс {0} [{1:0.000000}]. hits: {2}", pid, res, hits);
                if (hits >= m_MaxHits)
                {
                    Log.Info("Завершение процесса {0}...", pid);
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
