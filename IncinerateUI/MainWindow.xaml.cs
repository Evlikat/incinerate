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
using IncinerateService.API;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace IncinerateUI
{
    public partial class MainWindow : Window
    {
        GeneralSource m_GeneralSource;
        IncinerateClient m_Client;

        public MainWindow()
        {
            InitializeComponent();
            m_GeneralSource = new GeneralSource();
            m_Client = new IncinerateClient();
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            GetInfoResult result = null;
            try
            {
                result = m_Client.Execute(new GetInfoCommand()) as GetInfoResult;
            }
            catch (ServiceException ex)
            {
                MessageBox.Show("Can not get Agent list. Perhaps service is not running.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Binding
            this.DataContext = m_GeneralSource;

            // Loading
            UpdateAgentList(result.AgentInfos);
        }

        private void UpdateAgentList(IList<AgentInfo> infos)
        {
            ObservableCollection<AgentController> agents
                = new ObservableCollection<AgentController>();
            foreach (AgentInfo info in infos)
            {
                agents.Add(new AgentController
                {
                    Name = info.Name,
                    Status = AgentStatus.Parse(info.Status)
                });
            }
            m_GeneralSource.Agents = agents;
            this.AgentGrid.ItemsSource = m_GeneralSource.Agents;
        }

        private void RefreshAgentList()
        {
            UpdateAgentList((m_Client.Execute(new GetInfoCommand()) as GetInfoResult).AgentInfos);
        }

        private void CmdButton_Click(object sender, RoutedEventArgs e)
        {
            AgentController controller = (sender as Button).DataContext as AgentController;
            controller.AvailableCommand.AgentName = controller.Name;
            Command finalCommand = controller.AvailableCommand;
            if (controller.AvailableCommand is RunCommand)
            {
                WatchParametersDialog dlg = new WatchParametersDialog();
                dlg.Owner = this;
                bool? result = dlg.ShowDialog();
                if (result == true)
                {
                    WatchCommand cmd = new WatchCommand();
                    cmd.AgentName = controller.AvailableCommand.AgentName;
                    cmd.RedStrategy = dlg.Settings.RedStrategy.Name;
                    cmd.YellowStrategy = dlg.Settings.YellowStrategy.Name;
                    cmd.P1 = dlg.Settings.P1;
                    cmd.P2 = dlg.Settings.P2;
                    finalCommand = cmd;
                    
                }
                else
                {
                    return;
                }
            }
            m_Client.Execute(finalCommand);
            RefreshAgentList();
        }

        private void CmdButton_MouseRightClick(object sender, MouseButtonEventArgs e)
        {
            AgentController controller = (sender as Button).DataContext as AgentController;
            controller.AvailableCommand.AgentName = controller.Name;
            Command finalCommand = controller.AvailableCommand;
            if (controller.AvailableCommand is RunCommand)
            {
                GuardParametersDialog dlg = new GuardParametersDialog();
                dlg.Owner = this;
                dlg.Settings.Process = controller.Name;
                bool? result = dlg.ShowDialog();
                if (result == true)
                {
                    GuardCommand cmd = new GuardCommand();
                    cmd.Process = dlg.Settings.Process;
                    cmd.AgentName = controller.AvailableCommand.AgentName;
                    cmd.RedStrategy = dlg.Settings.RedStrategy.Name;
                    cmd.YellowStrategy = dlg.Settings.YellowStrategy.Name;
                    cmd.E1 = dlg.Settings.E1;
                    cmd.E2 = dlg.Settings.E2;
                    finalCommand = cmd;
                }
                else
                {
                    return;
                }
            }
            m_Client.Execute(finalCommand);
            RefreshAgentList();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            LearnSelectDialog dlg = new LearnSelectDialog();
            dlg.Owner = this;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                m_Client.Execute(new LearnCommand()
                {
                    AgentName = dlg.Settings.RuleName,
                    PIDs = new List<int>(
                        dlg.Settings.SelectedProcesses.Select(process => process.PID)
                        )
                });
                RefreshAgentList();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshAgentList();
        }
    }
}
