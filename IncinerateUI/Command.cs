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

    class RunCommand : AgentCommand
    {
        public WatchCommand WatchCmd { get; private set; }
        public GuardCommand GuardCmd { get; private set; }

        public RunCommand(WatchCommand watchCmd, GuardCommand guardCmd)
        {
            Name = "Watch / Guard";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            return new NoResult();
        }
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

    class GuardCommand : AgentCommand
    {
        public string Process { get; set; }
        public string RedStrategy { get; set; }
        public string YellowStrategy { get; set; }
        public double E1 { get; set; }
        public double E2 { get; set; }

        public GuardCommand()
        {
            Name = "Guard";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.Guard(AgentName, Process, RedStrategy, YellowStrategy, E1, E2);
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

    class StopGuardCommand : AgentCommand
    {
        public StopGuardCommand()
        {
            Name = "Stop Guard";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            service.StopGuard(AgentName);
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

    class GetProcessStats : AgentCommand
    {
        public GetProcessStats()
        {
            Name = "Get Process Statistics";
        }

        public override CommandResult Execute(IIncinerateService service)
        {
            return new GetStatResult() { ProcessStats = service.GetProcessStats() };
        }
    }
}
