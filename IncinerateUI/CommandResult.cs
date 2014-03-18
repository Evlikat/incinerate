using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IncinerateService.API;

namespace IncinerateUI
{
    class CommandResult
    {
    }

    class NoResult : CommandResult
    {
    }

    class GetInfoResult : CommandResult
    {
        public IList<AgentInfo> AgentInfos { get; set; }
    }

    class GetStatResult : CommandResult
    {
        public IList<ProcessStatInfo> ProcessStats { get; set; }
    }

    class GetVerboseStatResult : CommandResult
    {
        public ProcessVerboseStat VerboseStat { get; set; }
    }
}
