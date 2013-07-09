﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Diagnostics.Eventing;

namespace NeuroIncinerate.Neuro
{
    public class ProcessEventCollector
    {
        public bool Running { get; private set; }
        public static string SessionName = "NT Kernel Logger";
        private TraceEventSession m_TraceEventSession;
        private ETWTraceEventSource m_TraceEventSource;
        public event Action<TraceEvent> ActionOccurred;
        public ProcessEventCollector()
        {
            Running = false;
        }

        public void Start()
        {
            try
            {
                m_TraceEventSession = new TraceEventSession(SessionName, null);
                m_TraceEventSession.EnableKernelProvider(GetKeyEvents());

                m_TraceEventSource = new ETWTraceEventSource(SessionName, TraceEventSourceType.Session);
                m_TraceEventSource.Kernel.All += new Action<TraceEvent>(Kernel_All);

                Running = true;
            }
            catch (Exception ex)
            {
                Stop();
                throw;
            }
        }

        private KernelTraceEventParser.Keywords GetKeyEvents()
        {
            return KernelTraceEventParser.Keywords.DiskFileIO | 
                KernelTraceEventParser.Keywords.NetworkTCPIP |
                KernelTraceEventParser.Keywords.ProcessCounters |
                KernelTraceEventParser.Keywords.DiskIO |
                KernelTraceEventParser.Keywords.DiskIOInit |
                KernelTraceEventParser.Keywords.SplitIO |
                KernelTraceEventParser.Keywords.Driver |
                KernelTraceEventParser.Keywords.FileIO |
                KernelTraceEventParser.Keywords.Registry;
        }

        private unsafe void Kernel_All(TraceEvent obj)
        {
            ActionOccurred(obj);
        }

        public void Stop()
        {
            Running = false;
            if (m_TraceEventSource != null) { m_TraceEventSource.Close(); }
        }

        public void AttachToProcessing()
        {
            while (Running)
            {
                try
                {
                    Running = m_TraceEventSource.Process();
                }
                catch
                {
                }
            }
        }
    }
}
