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

namespace IncinerateUI
{
    public partial class LearnSelectDialog : Window
    {
        public LearnSelectDialogSettings Settings { get; private set; } 

        public LearnSelectDialog()
        {
            InitializeComponent();
            Settings = new LearnSelectDialogSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Settings;

            Settings.RuleName = "rule";

            Process[] processes = Process.GetProcesses();
            Settings.Processes = new ObservableCollection<ProcessInfo>(
                processes.Select(
                process => new ProcessInfo() { Name = process.ProcessName, PID = process.Id }).OrderBy(
                process => process.Name));

            Keyboard.Focus(RuleNameTextBox);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.SelectedProcesses.Count == 0)
            {
                MessageBox.Show("Select at least one Process",
                    "No Process selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

    public class LearnSelectDialogSettings
    {
        public ObservableCollection<ProcessInfo> Processes { get; set; }
        public string RuleName { get; set; }
        public List<ProcessInfo> SelectedProcesses
        {
            get
            {
                return new List<ProcessInfo>(Processes.Where(process => process.Selected));
            }
        }
    }

    public class ProcessInfo
    {
        public bool Selected { get; set; }
        public string Name { get; set; }
        public int PID { get; set; }
    }
}
