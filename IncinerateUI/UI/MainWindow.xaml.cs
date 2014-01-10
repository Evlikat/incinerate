using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IncinerateUI
{
    public partial class MainWindow : Window
    {
        GeneralSource m_GeneralSource;

        public MainWindow()
        {
            InitializeComponent();
            m_GeneralSource = new GeneralSource();
            // Binding
            this.DataContext = m_GeneralSource;

            m_GeneralSource.Agents.Add(new AgentController
            {
                Name = "Agent1",
                Status = AgentStatus.Watching
            }
            );
            m_GeneralSource.Agents.Add(new AgentController
            {
                Name = "Agent2",
                Status = AgentStatus.Learning
            }
            );
        }

        private void StatusButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
