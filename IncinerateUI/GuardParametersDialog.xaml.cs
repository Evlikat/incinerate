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

namespace IncinerateUI
{
    /// <summary>
    /// Interaction logic for WatchParametersDialog.xaml
    /// </summary>
    public partial class GuardParametersDialog : Window
    {
        public GuardParametersDialogSettings Settings { get; private set; }

        public GuardParametersDialog()
        {
            InitializeComponent();
            Settings = new GuardParametersDialogSettings();
            this.DataContext = Settings;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

    public class GuardParametersDialogSettings : NotifyPropertyChanged
    {
        public IList<GuardStrategy> Strategies { get; private set; }
        public string Process { get; set; }
        public double E1 { get; set; }
        public double E2 { get; set; }
        public GuardStrategy YellowStrategy { get; set; }
        public GuardStrategy RedStrategy { get; set; }

        public GuardParametersDialogSettings()
        {
            Process = "Enter Process name";
            E1 = 0.0;
            E2 = 0.0;
            Strategies = new List<GuardStrategy>()
            {
                new GuardStrategy() { Name = "alarm" },
                new GuardStrategy() { Name = "warning" }
            };
        }
    }

    public class GuardStrategy
    {
        public string Name { get; set; }
    }
}
