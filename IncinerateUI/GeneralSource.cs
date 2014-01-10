using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace IncinerateUI
{
    class GeneralSource
    {
        public ObservableCollection<AgentController> Agents { get; set; }

        public GeneralSource()
        {
            Agents = new ObservableCollection<AgentController>();
        }
    }
}
