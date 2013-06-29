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
                    else if (cmdArgs.Watched != null)
                    {
                        customersProxy.Watch(cmdArgs.Watched);
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
    }
}
