using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace IncinerateUI
{
    class GeneralSource : NotifyPropertyChanged
    {
        ObservableCollection<AgentController> m_Agents;
        public ObservableCollection<AgentController> Agents
        {
            get { return m_Agents; }
            set
            {
                m_Agents = value;
                RaisePropertyChanged("Agents");
            }
        }
        
        public GeneralSource()
        {
            m_Agents = new ObservableCollection<AgentController>();
        }
    }
}
