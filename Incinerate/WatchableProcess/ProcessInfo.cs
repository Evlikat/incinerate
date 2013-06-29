using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Incinerate.Base;

namespace Incinerate.WatchableProcess
{
    [Serializable]
    class ProcessInfo : IComparable
    {
        private static int MaxStatEntries = 10;

        [NonSerialized]
        public static string[] CriterionNames = { "cpu", "memory", "threads count" };
        public Queue<CriteriaMemento> CPUUsingHistory = new Queue<CriteriaMemento>();
        public Queue<CriteriaMemento> MemoryUsingHistory = new Queue<CriteriaMemento>();
        public Queue<CriteriaMemento> ThreadsCount = new Queue<CriteriaMemento>();
        public Dictionary<string, Queue<CriteriaMemento>> Criterions = new Dictionary<string, Queue<CriteriaMemento>>();
        public VectorN CriterionValues
        {
            get
            {
                VectorN result = new VectorN(CriterionNames.Length);
                int i = 0;
                result[i++] = CPUUsingHistory.Last().Value;
                result[i++] = MemoryUsingHistory.Last().Value;
                result[i++] = ThreadsCount.Last().Value;
                return result;
            }
        }
        
        public string Name { get; set; }
        public int Pid { get; set; }
        public double Cpu 
        {
            get
            {
                CriteriaMemento memento = CPUUsingHistory.Last();
                if (memento != null)
                    return memento.Value;

                return 0;
            }
        }

        public double Memory
        {
            get
            {
                CriteriaMemento memento = MemoryUsingHistory.Last();
                if (memento != null)
                    return memento.Value;

                return 0;
            }
        }

        public double Threads
        {
            get
            {
                CriteriaMemento memento = ThreadsCount.Last();
                if (memento != null)
                    return memento.Value;

                return 0;
            }
        }

        public ProcessInfo()
        {
            int i = 0;
            Criterions.Add(CriterionNames[i++], CPUUsingHistory);
            Criterions.Add(CriterionNames[i++], MemoryUsingHistory);
            Criterions.Add(CriterionNames[i++], ThreadsCount);
        }

        public ProcessInfo(ProcessMemento memento) : this()
        {
            Name = memento.Name;
            Pid = memento.Pid;
            Update(memento);
        }

        public void AddCpuUsingInfo(CpuUsingInfo cpuUsing)
        {
            EnqueueCriteriaMemento(CPUUsingHistory, cpuUsing);
        }

        public void AddMemoryUsingInfo(MemoryUsingInfo memoryUsing)
        {
            EnqueueCriteriaMemento(MemoryUsingHistory, memoryUsing);
        }

        public void AddThreadsCountInfo(ThreadCount memoryUsage)
        {
            EnqueueCriteriaMemento(ThreadsCount, memoryUsage);
        }

        public void Update(ProcessMemento info)
        {
            AddCpuUsingInfo(info.CpuUsing);
            AddMemoryUsingInfo(info.MemoryUsing);
            AddThreadsCountInfo(info.ThreadsCount);
        }

        public int CompareTo(object obj)
        {
            ProcessInfo otherInfo = obj as ProcessInfo;
            return Name.CompareTo(otherInfo.Name);
        }

        private void EnqueueCriteriaMemento(Queue<CriteriaMemento> queue, CriteriaMemento item)
        {
            if (queue.Count == MaxStatEntries)
            {
                queue.Dequeue();
            }
            else
            {
                queue.Enqueue(item);
            }
        }
    }
}
