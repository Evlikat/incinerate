using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IncinerateUI
{
    class AgentController : INotifyPropertyChanged
    {
        AgentStatus m_Status;

        public string Name { get; set; }
        public AgentStatus Status
        {
            get { return m_Status; }
            set
            {
                m_Status = value;
            }
        }
        public AgentStatus NextStatus { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
