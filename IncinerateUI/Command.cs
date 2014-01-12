using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IncinerateService.API;

namespace IncinerateUI
{
    abstract class Command
    {
        public string Name { get; protected set; }

        public abstract CommandResult Execute(IIncinerateService service);
    }

    class GetInfoCommand : Command
    {
        public override CommandResult Execute(IIncinerateService service)
        {
            return new GetInfoResult { AgentInfos = service.GetAgents() };
        }
    }

    abstract class AgentCommand : Command
    {
        public string AgentName { get; set; }
    }

    class WatchCommand : AgentCommand
    {
        public string RedStrategy { get; set; }
        public string YellowStrategy { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }

        public WatchCommand()
        {
            Name = "Watch";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.Watch(AgentName, RedStrategy, YellowStrategy, P1, P2);
            return new NoResult();
        }
    }

    class StopWatchCommand : AgentCommand
    {
        public StopWatchCommand()
        {
            Name = "Stop Watch";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.StopWatch(AgentName);
            return new NoResult();
        }
    }

    class RemoveCommand : AgentCommand
    {
        public RemoveCommand()
        {
            Name = "Remove";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.Stop();
            return new NoResult();
        }
    }

    class LearnCommand : AgentCommand
    {
        public IList<int> PIDs { get; set; }

        public LearnCommand()
        {
            Name = "Learn";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.AddLearningAgent(PIDs, AgentName);
            return new NoResult();
        }
    }

    class StopLearnCommand : AgentCommand
    {
        public StopLearnCommand()
        {
            Name = "Stop Learn";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.RemoveLearningAgent(AgentName);
            return new NoResult();
        }
    }
}
