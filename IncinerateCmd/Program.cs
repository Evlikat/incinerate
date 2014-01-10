using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IncinerateService.Utils;
using IncinerateService.API;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace IncinerateCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArgs cmdArgs = new CommandLineArgs(args);
            try
            {
                using (ChannelFactory<IIncinerateService> clientFactory = new ChannelFactory<IIncinerateService>())
                {

                    NamedPipeTransportBindingElement transport = new NamedPipeTransportBindingElement();
                    CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), transport);
                    clientFactory.Endpoint.Binding = binding;
                    clientFactory.Endpoint.Address = new EndpointAddress("net.pipe://IncinerateService/incinerate");
                    IIncinerateService customersProxy = clientFactory.CreateChannel();
                    if (cmdArgs.GetAgents)
                    {
                        IList<string> lines = customersProxy.GetAgents();
                        StringBuilder sb = new StringBuilder();
                        foreach (string line in lines)
                        {
                            sb.AppendLine(line);
                        }
                        Console.WriteLine(sb.ToString());
                    }
                    else if (cmdArgs.ProcessIDs != null && cmdArgs.AgentName != null)
                    {
                        string[] splitted = cmdArgs.ProcessIDs.Split(',');
                        IList<int> pids = new List<int>();
                        foreach (string pidStr in splitted)
                        {
                            pids.Add(Int32.Parse(pidStr));
                        }
                        customersProxy.AddLearningAgent(pids, cmdArgs.AgentName);
                    }
                    else if (cmdArgs.Watched != null
                        && cmdArgs.StrategyRed != null && cmdArgs.StrategyYellow != null
                        && cmdArgs.P1 != null && cmdArgs.P2 != null)
                    {
                        customersProxy.Watch(cmdArgs.Watched,
                            cmdArgs.StrategyRed, cmdArgs.StrategyYellow,
                            Double.Parse(cmdArgs.P1), Double.Parse(cmdArgs.P2));
                    }
                    else if (cmdArgs.Stop != null)
                    {
                        customersProxy.Stop();
                    }
                    else
                    {
                        cmdArgs.PrintUsage();
                    }
                    clientFactory.Close();
                }
            }
            catch (CommunicationObjectFaultedException ex)
            {
                Console.WriteLine("Service is not working");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Illegal agruments");
            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine("Service is not running");
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine("Can not connect to service");
            }
        }
    }

    internal class CommandLineArgs : CommandLineOptions
    {
        public CommandLineArgs(string[] args) : base(args) { }

        [Option(Short = "l", Description = "Return list of all learned or learning agents")]
        public bool GetAgents = false;

        [Option(Short = "w", Description = "Start one of agents to watch processes")]
        public string Watched = null;

        [Option(Short = "p", Description = "PIDs of target process sample divided by ',' sign")]
        public string ProcessIDs = null;

        [Option(Short = "a", Description = "Start to learn specified process behaviour")]
        public string AgentName = null;

        [Option(Short = "sr", Description = "Selects a strategy for red line reaching")]
        public string StrategyRed = null;

        [Option(Short = "sy", Description = "Selects a strategy for yellow line reaching")]
        public string StrategyYellow = null;

        [Option(Short = "p1", Description = "Determines yellow threshold")]
        public string P1 = null;

        [Option(Short = "p2", Description = "Determines red threshold")]
        public string P2 = null;

        [Option(Short = "q", Description = "Stops any activity in service")]
        public bool Stop = false;
    }
}
