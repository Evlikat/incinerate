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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using IncinerateService.API;

namespace IncinerateUI
{
    partial class DynamicTaskManagerDialog : Window
    {
        IncinerateClient m_Client;
        internal DynamicTaskManagerSettings Settings { get; private set; }

        internal DynamicTaskManagerDialog(IncinerateClient client)
        {
            InitializeComponent();
            m_Client = client;
            Settings = new DynamicTaskManagerSettings();
        }

        private void RefreshList()
        {
            IList<ProcessStatInfo> stats = ((GetStatResult)m_Client.Execute(new GetProcessStats())).ProcessStats;
            this.ProcessGrid.ItemsSource = new ObservableCollection<ProcessStat>(stats.Select(info => new ProcessStat(info)));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshList();
            this.DataContext = Settings;
        }

        private void EndProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcessStat processStat = (ProcessStat)this.ProcessGrid.SelectedItem;
                Process target = Process.GetProcessById(processStat.PID);
                if (target != null)
                {
                    target.Kill();
                }
                else
                {
                    MessageBox.Show("Process was already finished",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not end this process. Permission denied",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }
    }

    internal class DynamicTaskManagerSettings
    {
        public ObservableCollection<ProcessStat> ProcessStats { get; set; }
        public string RuleName { get; set; }
    }

    internal class ProcessStat
    {
        public ProcessStat(ProcessStatInfo info)
        {
            this.Name = info.Name;
            this.PID = info.PID;
            this.DiskFileActivity = info.DiskFileActivity;
            this.NetActivity = info.NetActivity;
            this.RegistryActivity = info.RegistryActivity;
        }

        public string Name { get; private set; }
        public int PID { get; private set; }
        public int DiskFileActivity { get; private set; }
        public int NetActivity { get; private set; }
        public int RegistryActivity { get; private set; }
    }
}
