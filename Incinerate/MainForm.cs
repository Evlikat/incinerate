using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Incinerate.Base;
using System.Diagnostics;
using Incinerate.WatchableProcess;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using NeuroIncinerate.Neuro;

namespace Incinerate
{
    public partial class MainForm : Form
    {
        private string logPath;
        private static string MapString = "no";
        private ProcessInformationCollector mainCollector;
        private StatSaver statSaver;
        private string currentProcess = "";
        private string currentCriteria = "";
        private Series currentCriteriaLine;
        private Dictionary<int, DataPoint> m_mapPoints = new Dictionary<int, DataPoint>();
        ProcessEventCollector processEventCollector = new ProcessEventCollector();

        public MainForm()
        {
            InitializeComponent();
            mainCollector = new ProcessInformationCollector();
            //processInfoViewDataGrid.DataSource = mainCollector.Watcher.Processes;
            statSaver = new StatSaver(mainCollector.Watcher);
        }

        #region events
        private void captureTimer_Tick(object sender, EventArgs e)
        {
            OutputLastInformation();
        }

        private void outputLastInformationButton_Click(object sender, EventArgs e)
        {
            if (!mainCollector.IsCollecting)
            {
                logPath = logPathTextBox.Text;
                mainCollector.CollectInformationInterval = (int)frequencyUpDown.Value;
                mainCollector.StartCollectInformation();
                captureTimer.Start();
            }
            else
            {
                mainCollector.StopCollectInformation();
                captureTimer.Stop();
            }
        }

        private void processSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawChart();
        }

        private void criteriaSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawChart();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            criteriaSelector.Items.Clear();
            foreach (string critName in ProcessInfo.CriterionNames)
            {
                criteriaSelector.Items.Add(critName);
            }       
        }

        private void processSelector_DropDown(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box == null) return;
            box.Items.Clear();
            List<ProcessInfo> processDataValues = new List<ProcessInfo>(mainCollector.Watcher.Processes);
            processDataValues.Sort();
            foreach (ProcessInfo info in processDataValues)
            {
                box.Items.Add(info.Pid.ToString() + " - " + info.Name);
            }
        }

        private void criteriaAxisX_DropDown(object sender, EventArgs e)
        {
            FillCriteriaComboBox(sender as ComboBox);
        }

        private void criteriaAxisY_DropDown(object sender, EventArgs e)
        {
            FillCriteriaComboBox(sender as ComboBox);
        }

        private void criteriaMass_DropDown(object sender, EventArgs e)
        {
            FillCriteriaComboBox(sender as ComboBox);
        }

        private void FillCriteriaComboBox(ComboBox box)
        {
            if (box == null) return;
            box.Items.Clear();
            foreach (string critName in ProcessInfo.CriterionNames)
            {
                box.Items.Add(critName);
            }  
        }

        private void saveStatButton_Click(object sender, EventArgs e)
        {
            // Stops thread if it needs
            bool isCollected = false;
            if (mainCollector.IsCollecting)
            {
                mainCollector.StopCollectInformation();
                isCollected = true;
            }
            if (isCollected) mainCollector.StartCollectInformation();
            // choose file to save
            string name = GetFileToSave();
            if (name.Equals("")) return;
            if (statSaver.SaveInfo(name))
            {
                MessageBox.Show("Данные сохранены", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else 
            {
                MessageBox.Show("Данные не были сохранены", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadStatButton_Click(object sender, EventArgs e)
        {
            string name = GetFileToLoad();
            if (name.Equals("")) return;
            ProcessWatcher l_processWatcher = statSaver.LoadInfo(name) as ProcessWatcher;
            if (l_processWatcher == null)
            {
                MessageBox.Show("Не удалось загрузить данные", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                mainCollector.Watcher = l_processWatcher;
                OutputLastInformation();
                MessageBox.Show("Данные успешно загружены", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainCollector.StopCollectInformation();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.Show();
        }

        private void swapAxisButton_Click(object sender, EventArgs e)
        {
            string tmp = criteriaAxisX.Text;
            criteriaAxisX.Text = criteriaAxisY.Text;
            criteriaAxisY.Text = tmp;
            string tmp2 = xAxisMaximumValue.Text;
            xAxisMaximumValue.Text = yAxisMaximumValue.Text;
            yAxisMaximumValue.Text = tmp2;
            DrawMap();
        }

        private void startNeuroCollectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!processEventCollector.Running)
                {
                    processEventCollector.Start();
                }
                else
                {
                    processEventCollector.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sh*t happends:" + ex.Message);
            }
        }
        #endregion

        // Рисование диаграммы
        private void DrawChart(int iProc, string crit)
        {
            // if selected items has not been changed just add new point
            List<CriteriaMemento> values = new List<CriteriaMemento>(mainCollector.Watcher[iProc].Criterions[crit]);
            CriteriaMemento lastCriteriaMemento = values[values.Count - 1];
            if (currentProcess.Equals(iProc.ToString()) && currentCriteria.Equals(crit))
            {
                currentCriteriaLine.Points.AddXY(currentCriteriaLine.Points.Count, lastCriteriaMemento.Value);
                criteriaChart.ChartAreas[0].AxisY.Maximum = Math.Max(lastCriteriaMemento.GetMaxValue(), criteriaChart.ChartAreas[0].AxisY.Maximum);
                criteriaChart.ChartAreas[0].AxisY.Minimum = Math.Min(lastCriteriaMemento.GetMinValue(), criteriaChart.ChartAreas[0].AxisY.Minimum);
                criteriaChart.ChartAreas[0].AxisY.Interval = lastCriteriaMemento.GetInterval();
            }
            else
            {
                criteriaChart.Series.Clear();
                Series criteriaLine = new Series(iProc + " - " + crit);
                criteriaLine.ChartType = SeriesChartType.Spline;
                criteriaLine.Color = Color.Red;
                int i = 0;
                criteriaChart.ChartAreas[0].AxisY.Maximum = 0;
                criteriaChart.ChartAreas[0].AxisY.Minimum = 0;
                foreach (CriteriaMemento criteria in values)
                {
                    criteriaLine.Points.AddXY(i++, criteria.Value);
                    criteriaChart.ChartAreas[0].AxisY.Maximum = Math.Max(criteria.GetMaxValue(), criteriaChart.ChartAreas[0].AxisY.Maximum);
                    criteriaChart.ChartAreas[0].AxisY.Minimum = Math.Min(criteria.GetMinValue(), criteriaChart.ChartAreas[0].AxisY.Minimum);
                    criteriaChart.ChartAreas[0].AxisY.Interval = criteria.GetInterval();
                }
                criteriaChart.Series.Add(criteriaLine);
                currentCriteriaLine = criteriaLine;
            }
            currentCriteria = crit;
            currentProcess = iProc.ToString();
        }

        // Рисование карты
        private void DrawMap()
        {
            string critX = (string)criteriaAxisX.SelectedItem;
            string critY = (string)criteriaAxisY.SelectedItem;
            string critM = (string)criteriaMass.SelectedItem;
            string filter = filterText.Text;
            if (critX == null || critY == null || critM == null || critX.Trim() == "" || critY.Trim() == "" || critM.Trim() == "") return;
            try
            {
                mapChart.ChartAreas[0].AxisX.Minimum = 0;
                mapChart.ChartAreas[0].AxisX.Maximum = Double.Parse(xAxisMaximumValue.Text);
                mapChart.ChartAreas[0].AxisY.Minimum = 0;
                mapChart.ChartAreas[0].AxisY.Maximum = Double.Parse(yAxisMaximumValue.Text);
            }
            catch (Exception)
            {
                return;
            }
            if (mapChart.Series.Count == 0)
            {
                Series mapSeria = new Series(MapString);
                mapSeria.ChartType = SeriesChartType.Point;
                mapSeria.Color = Color.Black;
                mapSeria.SmartLabelStyle.IsMarkerOverlappingAllowed = false;
                mapSeria.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
                mapChart.Series.Add(mapSeria);
            }
            List<ProcessInfo> processDataValues = new List<ProcessInfo>();
            processDataValues.AddRange(mainCollector.Watcher.Processes);
            //m_mapPoints.Clear();
            Series oldMapSeria = mapChart.Series[MapString];
            oldMapSeria.Points.Clear();
            double xSum = 0;
            double ySum = 0;
            double mSum = 0;
            foreach (ProcessInfo info in processDataValues)
            {
                double xValue = info.Criterions[critX].Last().Value;
                double yValue = info.Criterions[critY].Last().Value;
                double mValue = info.Criterions[critM].Last().Value;
                xSum += xValue * mValue;
                ySum += yValue * mValue;
                mSum += mValue;
                DataPoint point = new DataPoint(xValue, yValue);
                point.Label = String.Format("{0} ({1})", info.Name, info.Pid);
                if (!String.IsNullOrEmpty(filter) && info.Name.Contains(filter))
                {
                    point.Color = Color.Blue;
                    point.LabelForeColor = Color.Blue;
                    point.MarkerStyle = MarkerStyle.Triangle;
                }
                oldMapSeria.Points.Add(point);
            }
            DataPoint xCenterPoint = new DataPoint(xSum / mSum, 0);
            DataPoint yCenterPoint = new DataPoint(0, ySum / mSum);
            DataPoint xyCenterPoint = new DataPoint(xSum / mSum, ySum / mSum);
            xyCenterPoint.Label = xyCenterPoint.XValue + ";" + xyCenterPoint.YValues[0];
            xCenterPoint.Color = Color.Red;
            yCenterPoint.Color = Color.Red;
            xyCenterPoint.Color = Color.Red;
            oldMapSeria.Points.Add(xCenterPoint);
            oldMapSeria.Points.Add(yCenterPoint);
            oldMapSeria.Points.Add(xyCenterPoint);
        }
        
        /// <summary>
        /// Вывод последней полученной информации
        /// </summary>
        private void OutputLastInformation()
        {
            string proc = (string)processSelector.SelectedItem;
            string crit = (string)criteriaSelector.SelectedItem;
            if (proc == null || proc.Length == 0 || crit == null || crit.Length == 0) return;
            int iProc = Int32.Parse(proc.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[0]);
            DrawChart(iProc, crit);
            DrawMap();

            mainCollector.Pid = iProc;
            criteriaSpeedLabel.Text = "";
            List<ProcessInformationCollector.SpeedEntry> list = new List<ProcessInformationCollector.SpeedEntry>(mainCollector.AllSpeeds);
            int max = 20;
            foreach (ProcessInformationCollector.SpeedEntry se in list)
            {
                if(max-- <= 0) 
                    break;
                criteriaSpeedLabel.Text += String.Format("{0} - {1}\n", se.Name, se.Value);
            }
            processCriteriaLabel.Text = "";
            
            if(outputLastInformationCheckBox.Checked)
                SaveToFile(Path.Combine(logPath, "table-" + DateTime.Now.ToString("hhmmss")), criteriaSpeedLabel.Text);

            processCriteriaLabel.Text += mainCollector.Watcher.MassCenters[iProc].ToString() + "\n";
            processCriteriaLabel.Text += mainCollector.Watcher.MassCenter.ToString() + "\n";
            for(int i = 0; i < mainCollector.Speed.Length; i++)
            {
                processCriteriaLabel.Text += String.Format("{0} : {1} - {2} = {3}\n", 
                    ProcessInfo.CriterionNames[i],
                    mainCollector.Speed[i].ToString(),
                    mainCollector.SpeedWithIgnored[i].ToString(),
                    ((mainCollector.Speed[i] - mainCollector.SpeedWithIgnored[i]) / mainCollector.Speed[i]).ToString());
            }
            ProcessInformationCollector.SpeedEntry entry = mainCollector.GetSpeedEntry(iProc);
            if (entry != null)
            {
                processCriteriaLabel.Text += "Total: " + entry.Value;
            }

            if (outputLastInformationCheckBox.Checked)
                SaveToFile(Path.Combine(logPath, "crit-" + DateTime.Now.ToString("hhmmss")), processCriteriaLabel.Text);

            procCountLabel.Text = "Всего процессов: " + mainCollector.Watcher.Processes.Count;
        }

        private void SaveToFile(string filename, string text)
        {
            FileStream fs = new FileStream(filename + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(text);
            sw.Close();
            fs.Close();
        }

        private string GetFileToSave()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "inr";
            sfd.FileName = DateTime.Now.ToLongDateString().Replace(":", "-") + " " + DateTime.Now.ToLongTimeString().Replace(":", "-");
            sfd.Filter = "Incinerator Files|*.inr";
            DialogResult result = sfd.ShowDialog();
            if (result != DialogResult.OK) return "";
            return sfd.FileName;
        }

        private string GetFileToLoad()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "inr";
            ofd.Filter = "Incinerator Files|*.inr";
            DialogResult result = ofd.ShowDialog();
            if (result != DialogResult.OK) return "";
            return ofd.FileName;
        }
    }
}
