using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Incinerate.WatchableProcess;

namespace Incinerate.Base
{
    class ProcessInformationCollector
    {
        public class SpeedEntry : IComparable
        {
            public int Pid;
            public double Value;
            public string Name;

            public SpeedEntry(int pid, double value, string name)
            {
                this.Pid = pid;
                this.Value = value;
                this.Name = name;
            }

            public int CompareTo(object obj)
            {
                SpeedEntry se = obj as SpeedEntry;
                if(se == null)
                    throw new InvalidCastException();
                if (this.Value < se.Value)
                    return 1;
                if (this.Value > se.Value)
                    return -1;
                return 0;
            }
        }

        Random random = new Random();

        public static int NoProcess = -1;
        private Thread collectorThread;
        private VectorN[] m_Speeds = new VectorN[ProcessInfo.CriterionNames.Length];
        private VectorN[] m_SpeedsWithIgnored = new VectorN[ProcessInfo.CriterionNames.Length];
        private List<SpeedEntry> m_AllSpeeds = new List<SpeedEntry>();

        public int Pid { get; set; }
        public bool IsCollecting { get; set; }
        public ProcessWatcher Watcher { get; set; }
        public VectorN[] Speed { get { return m_Speeds; } }
        public VectorN[] SpeedWithIgnored { get { return m_SpeedsWithIgnored; } }
        public List<SpeedEntry> AllSpeeds { get { return m_AllSpeeds; } }
        public int CollectInformationInterval { get; set; }

        public ProcessInformationCollector()
        {
            Watcher = new ProcessWatcher();
            for (int i = 0; i < ProcessInfo.CriterionNames.Length; i++)
            {
                m_Speeds[i] = new VectorN(ProcessInfo.CriterionNames.Length - 1);
                m_SpeedsWithIgnored[i] = new VectorN(ProcessInfo.CriterionNames.Length - 1);
            }
        }

        public void StartCollectInformation()
        {
            collectorThread = new Thread(new ThreadStart(CollectInformationTask));
            collectorThread.Start();
            IsCollecting = true;
        }

        public void StopCollectInformation()
        {
            if (collectorThread == null) return;
            collectorThread.Abort();
            IsCollecting = false;
        }

        private void CollectInformationTask()
        {
            int currentId = Process.GetCurrentProcess().Id;
            while (true)
            {
                Process[] currentProcesses = Process.GetProcesses();
                // Сбор информации о процессах
                List<ProcessMemento> currentMementos = new List<ProcessMemento>();
                lock (Watcher.Processes)
                {
                    foreach (Process process in currentProcesses)
                    {
                        ProcessMemento memento = new ProcessMemento();
                        memento.Name = process.ProcessName;
                        try
                        {
                            using (PerformanceCounter pcProcess = new PerformanceCounter("Process", "% Processor Time", process.ProcessName))
                            {
                                memento.CpuUsing = new CpuUsingInfo(pcProcess.RawValue);
                            }
                        }
                        catch
                        {
                            memento.CpuUsing = new CpuUsingInfo(0);
                        }
                        memento.MemoryUsing = new MemoryUsingInfo(process.WorkingSet64);
                        memento.ThreadsCount = new ThreadCount(process.Threads.Count);
                        memento.Pid = process.Id;

                        currentMementos.Add(memento);
                    }
                    AddInfo(currentMementos);
                }
                // mass centers
                ComputeMassCenters(currentProcesses);
                Thread.Sleep(CollectInformationInterval);
            }
        }

        private void ComputeMassCenters(Process[] currentProcesses)
        {
            m_Speeds = ComputeMassCenter();
            foreach (Process process in currentProcesses)
            {
                VectorN[] speedsWithout = ComputeMassCenter(process.Id);
                if (process.Id == Pid)
                    m_SpeedsWithIgnored = speedsWithout;

                VectorN result = new VectorN(m_Speeds.Length);
                for (int i = 0; i < m_Speeds.Length; i++)
                {
                    result[i] = ((m_Speeds[i] - speedsWithout[i]) / m_Speeds[i]).Length();
                }

                SpeedEntry se = GetSpeedEntry(process.Id);
                if (se == null)
                {
                    m_AllSpeeds.Add(new SpeedEntry(process.Id, result.Length(), process.ProcessName));
                }
                else
                {
                    se.Value = result.Length();
                }
            }
            m_AllSpeeds.Sort();
        }

        public SpeedEntry GetSpeedEntry(int pid)
        {
            foreach (SpeedEntry se in AllSpeeds)
            {
                if (se.Pid == pid)
                    return se;
            }
            return null;
        }

        private VectorN[] ComputeMassCenter(int pid)
        {
            // Для каждого критерия считаем центр масс (вектор)
            // Текущий критерий - масса
            VectorN[] speeds = new VectorN[ProcessInfo.CriterionNames.Length];
            for (int i = 0; i < speeds.Length; i++)
            {
                speeds[i] = new VectorN(ProcessInfo.CriterionNames.Length - 1);
            }
            Watcher.StrangeProcesses.Clear();
            int critIndex = 0;
            foreach (string massCriteriaName in ProcessInfo.CriterionNames)
            {
                // Центр масс
                VectorN sumVector = new VectorN(ProcessInfo.CriterionNames.Length - 1);
                VectorN prevVector = null;
                if (pid == NoProcess)
                {
                    prevVector = Watcher.MassCenter;
                }
                else
                {
                    if (Watcher.MassCenters.ContainsKey(pid))
                    {
                        prevVector = Watcher.MassCenters[pid];
                    }
                }
                double massSumm = 0;
                foreach (ProcessInfo info in Watcher.Processes)
                {
                    if (info.Pid != pid)
                    {
                        // Calc E(m_i)
                        massSumm += info.Criterions[massCriteriaName].Last().Value;
                        // k - 1 других критериев
                        double mValue = info.Criterions[massCriteriaName].Last().Value;
                        int c = 0;
                        foreach (string criteriaName in ProcessInfo.CriterionNames)
                        {
                            if (criteriaName.Equals(massCriteriaName))
                                continue;
                            
                            sumVector[c++] += info.Criterions[criteriaName].Last().Value * mValue;
                        }
                    }
                }
                sumVector = sumVector / massSumm;
                if (prevVector != null)
                {
                    VectorN speedVector = sumVector;// -prevVector;
                    speeds[critIndex] = speedVector;
                }
                if (pid == NoProcess)
                {
                    Watcher.MassCenter.CopyFrom(sumVector);
                }
                else
                {
                    if (Watcher.MassCenters.ContainsKey(pid))
                    {
                        Watcher.MassCenters[pid].CopyFrom(sumVector);
                    }
                    else
                    {
                        Watcher.MassCenters.Add(pid, sumVector);
                    }
                }
                critIndex++;
            }
            return speeds;
        }

        private VectorN[] ComputeMassCenter()
        {
            return ComputeMassCenter(NoProcess);
        }

        private void AddInfo(IEnumerable<ProcessMemento> mementos)
        {
            foreach (ProcessMemento memento in mementos)
            {
                ProcessInfo info = Watcher.ProcessInfoByPid(memento.Pid);
                if (info != null)
                {
                    info.Update(memento);
                }
                else
                    Watcher.Processes.Add(new ProcessInfo(memento));
            }
        }
    }
}
