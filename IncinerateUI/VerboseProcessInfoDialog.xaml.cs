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
using IncinerateService.API;
using System.Net;

namespace IncinerateUI
{
    public partial class VerboseProcessInfoDialog : Window
    {
        ProcessVerboseStat m_Stat;

        public VerboseProcessInfoDialog(ProcessVerboseStat stat)
        {
            InitializeComponent();
            this.m_Stat = stat;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FlowDocument doc = new FlowDocument();

            if (m_Stat.AffectedSourceAddresses.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Source Addresses:")));
                foreach (int saddr in m_Stat.AffectedSourceAddresses)
                {
                    doc.Blocks.Add(new Paragraph(new Run(new IPAddress(BitConverter.GetBytes(saddr)).ToString())));
                }
            }

            if (m_Stat.AffectedSourcePorts.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Source Ports:")));
                foreach (int sports in m_Stat.AffectedSourcePorts)
                {
                    doc.Blocks.Add(new Paragraph(new Run("" + sports)));
                }
            }

            if (m_Stat.AffectedDestinationAddresses.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Destination Addresses:")));
                foreach (int daddr in m_Stat.AffectedDestinationAddresses)
                {
                    doc.Blocks.Add(new Paragraph(new Run(new IPAddress(BitConverter.GetBytes(daddr)).ToString())));
                }
            }

            if (m_Stat.AffectedDestinationPorts.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Destination Ports:")));
                foreach (int dports in m_Stat.AffectedDestinationPorts)
                {
                    doc.Blocks.Add(new Paragraph(new Run("" + dports)));
                }
            }

            if (m_Stat.AffectedRegKeys.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Registry Keys:")));
                foreach (string regKey in m_Stat.AffectedRegKeys)
                {
                    doc.Blocks.Add(new Paragraph(new Run(regKey)));
                }
            }

            if (m_Stat.AffectedRegValues.Count > 0)
            {
                doc.Blocks.Add(new Paragraph(new Run(">> Registry Values:")));
                foreach (string regValue in m_Stat.AffectedRegValues)
                {
                    doc.Blocks.Add(new Paragraph(new Run(regValue)));
                }
            }

            this.StatBox.Document = doc;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
