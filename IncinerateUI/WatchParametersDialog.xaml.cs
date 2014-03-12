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
    public partial class WatchParametersDialog : Window
    {
        public WatchParametersDialogSettings Settings { get; private set; }

        public WatchParametersDialog()
        {
            InitializeComponent();
            Settings = new WatchParametersDialogSettings();
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

    public class WatchParametersDialogSettings : NotifyPropertyChanged
    {
        public IList<WatchStrategy> Strategies { get; private set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
        public WatchStrategy YellowStrategy { get; set; }
        public WatchStrategy RedStrategy { get; set; }

        public WatchParametersDialogSettings()
        {
            P1 = 0.15;
            P2 = 0.4;
            Strategies = new List<WatchStrategy>()
            {
                new WatchStrategy() { Name = "alert" },
                new WatchStrategy() { Name = "terminate" },
                new WatchStrategy() { Name = "terminate10" }
            };
        }
    }

    public class WatchStrategy
    {
        public string Name { get; set; }
    }
}
