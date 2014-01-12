using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IncinerateUI
{
    class AgentController : NotifyPropertyChanged
    {
        string m_Name;
        AgentStatus m_Status;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                RaisePropertyChanged("Name");
            }
        }
        public AgentStatus Status
        {
            get { return m_Status; }
            set
            {
                m_Status = value;
                if (m_Status == AgentStatus.Learning)
                {
                    AvailableCommand = new StopLearnCommand();
                }
                else if (m_Status == AgentStatus.Watching)
                {
                    AvailableCommand = new StopWatchCommand();
                }
                else if (m_Status == AgentStatus.Ready)
                {
                    AvailableCommand = new WatchCommand();
                }
                else
                {
                    throw new NotImplementedException();
                }
                RaisePropertyChanged("Status");
                RaisePropertyChanged("AvailableCommand");
            }
        }
        public AgentCommand AvailableCommand { get; private set; }

        public AgentController()
        {
        }
    }
}
