namespace Incinerate
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.captureTimer = new System.Windows.Forms.Timer(this.components);
            this.procCountLabel = new System.Windows.Forms.Label();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.lastInfoTab = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.logPathTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.frequencyUpDown = new System.Windows.Forms.NumericUpDown();
            this.loadStatButton = new System.Windows.Forms.Button();
            this.saveStatButton = new System.Windows.Forms.Button();
            this.outputLastInformationCheckBox = new System.Windows.Forms.CheckBox();
            this.outputLastInformationButton = new System.Windows.Forms.Button();
            this.concreteProcessTab = new System.Windows.Forms.TabPage();
            this.processCriteriaLabel = new System.Windows.Forms.Label();
            this.criteriaSpeedLabel = new System.Windows.Forms.Label();
            this.criteriaChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.criteriaSelector = new System.Windows.Forms.ComboBox();
            this.processSelector = new System.Windows.Forms.ComboBox();
            this.mapPage = new System.Windows.Forms.TabPage();
            this.filterText = new System.Windows.Forms.TextBox();
            this.yAxisMaximumValue = new System.Windows.Forms.TextBox();
            this.xAxisMaximumValue = new System.Windows.Forms.TextBox();
            this.swapAxisButton = new System.Windows.Forms.Button();
            this.criteriaMass = new System.Windows.Forms.ComboBox();
            this.criteriaAxisY = new System.Windows.Forms.ComboBox();
            this.criteriaAxisX = new System.Windows.Forms.ComboBox();
            this.mapChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.neuroPage = new System.Windows.Forms.TabPage();
            this.startNeuroCollectButton = new System.Windows.Forms.Button();
            this.mainTabControl.SuspendLayout();
            this.lastInfoTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyUpDown)).BeginInit();
            this.concreteProcessTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.criteriaChart)).BeginInit();
            this.mapPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapChart)).BeginInit();
            this.neuroPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // captureTimer
            // 
            this.captureTimer.Interval = 1000;
            this.captureTimer.Tick += new System.EventHandler(this.captureTimer_Tick);
            // 
            // procCountLabel
            // 
            this.procCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.procCountLabel.AutoSize = true;
            this.procCountLabel.Location = new System.Drawing.Point(12, 440);
            this.procCountLabel.Name = "procCountLabel";
            this.procCountLabel.Size = new System.Drawing.Size(0, 13);
            this.procCountLabel.TabIndex = 2;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTabControl.Controls.Add(this.lastInfoTab);
            this.mainTabControl.Controls.Add(this.concreteProcessTab);
            this.mainTabControl.Controls.Add(this.mapPage);
            this.mainTabControl.Controls.Add(this.neuroPage);
            this.mainTabControl.Location = new System.Drawing.Point(12, 12);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(737, 441);
            this.mainTabControl.TabIndex = 3;
            // 
            // lastInfoTab
            // 
            this.lastInfoTab.Controls.Add(this.label5);
            this.lastInfoTab.Controls.Add(this.logPathTextBox);
            this.lastInfoTab.Controls.Add(this.label4);
            this.lastInfoTab.Controls.Add(this.label3);
            this.lastInfoTab.Controls.Add(this.frequencyUpDown);
            this.lastInfoTab.Controls.Add(this.loadStatButton);
            this.lastInfoTab.Controls.Add(this.saveStatButton);
            this.lastInfoTab.Controls.Add(this.outputLastInformationCheckBox);
            this.lastInfoTab.Controls.Add(this.outputLastInformationButton);
            this.lastInfoTab.Location = new System.Drawing.Point(4, 22);
            this.lastInfoTab.Name = "lastInfoTab";
            this.lastInfoTab.Padding = new System.Windows.Forms.Padding(3);
            this.lastInfoTab.Size = new System.Drawing.Size(729, 415);
            this.lastInfoTab.TabIndex = 0;
            this.lastInfoTab.Text = "Настройка";
            this.lastInfoTab.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "мс";
            // 
            // logPathTextBox
            // 
            this.logPathTextBox.Location = new System.Drawing.Point(84, 66);
            this.logPathTextBox.Name = "logPathTextBox";
            this.logPathTextBox.Size = new System.Drawing.Size(159, 20);
            this.logPathTextBox.TabIndex = 10;
            this.logPathTextBox.Text = "c:\\temp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Частота";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Путь к логу";
            // 
            // frequencyUpDown
            // 
            this.frequencyUpDown.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.frequencyUpDown.Location = new System.Drawing.Point(84, 92);
            this.frequencyUpDown.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.frequencyUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.frequencyUpDown.Name = "frequencyUpDown";
            this.frequencyUpDown.Size = new System.Drawing.Size(159, 20);
            this.frequencyUpDown.TabIndex = 7;
            this.frequencyUpDown.Value = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            // 
            // loadStatButton
            // 
            this.loadStatButton.Location = new System.Drawing.Point(89, 6);
            this.loadStatButton.Name = "loadStatButton";
            this.loadStatButton.Size = new System.Drawing.Size(77, 32);
            this.loadStatButton.TabIndex = 6;
            this.loadStatButton.Text = "Загрузить";
            this.loadStatButton.UseVisualStyleBackColor = true;
            this.loadStatButton.Click += new System.EventHandler(this.loadStatButton_Click);
            // 
            // saveStatButton
            // 
            this.saveStatButton.Location = new System.Drawing.Point(6, 6);
            this.saveStatButton.Name = "saveStatButton";
            this.saveStatButton.Size = new System.Drawing.Size(77, 32);
            this.saveStatButton.TabIndex = 5;
            this.saveStatButton.Text = "Сохранить";
            this.saveStatButton.UseVisualStyleBackColor = true;
            this.saveStatButton.Click += new System.EventHandler(this.saveStatButton_Click);
            // 
            // outputLastInformationCheckBox
            // 
            this.outputLastInformationCheckBox.AutoSize = true;
            this.outputLastInformationCheckBox.Location = new System.Drawing.Point(6, 44);
            this.outputLastInformationCheckBox.Name = "outputLastInformationCheckBox";
            this.outputLastInformationCheckBox.Size = new System.Drawing.Size(90, 17);
            this.outputLastInformationCheckBox.TabIndex = 4;
            this.outputLastInformationCheckBox.Text = "Запись в лог";
            this.outputLastInformationCheckBox.UseVisualStyleBackColor = true;
            // 
            // outputLastInformationButton
            // 
            this.outputLastInformationButton.Location = new System.Drawing.Point(3, 127);
            this.outputLastInformationButton.Name = "outputLastInformationButton";
            this.outputLastInformationButton.Size = new System.Drawing.Size(78, 37);
            this.outputLastInformationButton.TabIndex = 2;
            this.outputLastInformationButton.Text = "Собрать ";
            this.outputLastInformationButton.UseVisualStyleBackColor = true;
            this.outputLastInformationButton.Click += new System.EventHandler(this.outputLastInformationButton_Click);
            // 
            // concreteProcessTab
            // 
            this.concreteProcessTab.Controls.Add(this.processCriteriaLabel);
            this.concreteProcessTab.Controls.Add(this.criteriaSpeedLabel);
            this.concreteProcessTab.Controls.Add(this.criteriaChart);
            this.concreteProcessTab.Controls.Add(this.label2);
            this.concreteProcessTab.Controls.Add(this.label1);
            this.concreteProcessTab.Controls.Add(this.criteriaSelector);
            this.concreteProcessTab.Controls.Add(this.processSelector);
            this.concreteProcessTab.Location = new System.Drawing.Point(4, 22);
            this.concreteProcessTab.Name = "concreteProcessTab";
            this.concreteProcessTab.Padding = new System.Windows.Forms.Padding(3);
            this.concreteProcessTab.Size = new System.Drawing.Size(729, 415);
            this.concreteProcessTab.TabIndex = 1;
            this.concreteProcessTab.Text = "Статистика";
            this.concreteProcessTab.UseVisualStyleBackColor = true;
            // 
            // processCriteriaLabel
            // 
            this.processCriteriaLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.processCriteriaLabel.AutoSize = true;
            this.processCriteriaLabel.Location = new System.Drawing.Point(412, 19);
            this.processCriteriaLabel.Name = "processCriteriaLabel";
            this.processCriteriaLabel.Size = new System.Drawing.Size(25, 13);
            this.processCriteriaLabel.TabIndex = 6;
            this.processCriteriaLabel.Text = "info";
            // 
            // criteriaSpeedLabel
            // 
            this.criteriaSpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.criteriaSpeedLabel.AutoSize = true;
            this.criteriaSpeedLabel.Location = new System.Drawing.Point(545, 19);
            this.criteriaSpeedLabel.Name = "criteriaSpeedLabel";
            this.criteriaSpeedLabel.Size = new System.Drawing.Size(25, 13);
            this.criteriaSpeedLabel.TabIndex = 5;
            this.criteriaSpeedLabel.Text = "info";
            // 
            // criteriaChart
            // 
            this.criteriaChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.criteriaChart.BorderlineColor = System.Drawing.Color.Black;
            this.criteriaChart.BorderlineWidth = 2;
            chartArea1.Name = "ChartArea1";
            this.criteriaChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.criteriaChart.Legends.Add(legend1);
            this.criteriaChart.Location = new System.Drawing.Point(6, 110);
            this.criteriaChart.Name = "criteriaChart";
            this.criteriaChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            this.criteriaChart.Size = new System.Drawing.Size(529, 299);
            this.criteriaChart.TabIndex = 4;
            this.criteriaChart.Text = "chart1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Критерий";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Процесс";
            // 
            // criteriaSelector
            // 
            this.criteriaSelector.FormattingEnabled = true;
            this.criteriaSelector.Location = new System.Drawing.Point(209, 19);
            this.criteriaSelector.Name = "criteriaSelector";
            this.criteriaSelector.Size = new System.Drawing.Size(197, 21);
            this.criteriaSelector.TabIndex = 1;
            this.criteriaSelector.SelectedIndexChanged += new System.EventHandler(this.criteriaSelector_SelectedIndexChanged);
            // 
            // processSelector
            // 
            this.processSelector.FormattingEnabled = true;
            this.processSelector.Items.AddRange(new object[] {
            "213",
            "345"});
            this.processSelector.Location = new System.Drawing.Point(6, 19);
            this.processSelector.Name = "processSelector";
            this.processSelector.Size = new System.Drawing.Size(197, 21);
            this.processSelector.TabIndex = 0;
            this.processSelector.DropDown += new System.EventHandler(this.processSelector_DropDown);
            this.processSelector.SelectedIndexChanged += new System.EventHandler(this.processSelector_SelectedIndexChanged);
            // 
            // mapPage
            // 
            this.mapPage.Controls.Add(this.filterText);
            this.mapPage.Controls.Add(this.yAxisMaximumValue);
            this.mapPage.Controls.Add(this.xAxisMaximumValue);
            this.mapPage.Controls.Add(this.swapAxisButton);
            this.mapPage.Controls.Add(this.criteriaMass);
            this.mapPage.Controls.Add(this.criteriaAxisY);
            this.mapPage.Controls.Add(this.criteriaAxisX);
            this.mapPage.Controls.Add(this.mapChart);
            this.mapPage.Location = new System.Drawing.Point(4, 22);
            this.mapPage.Name = "mapPage";
            this.mapPage.Padding = new System.Windows.Forms.Padding(3);
            this.mapPage.Size = new System.Drawing.Size(729, 415);
            this.mapPage.TabIndex = 2;
            this.mapPage.Text = "Карта";
            this.mapPage.UseVisualStyleBackColor = true;
            // 
            // filterText
            // 
            this.filterText.Location = new System.Drawing.Point(356, 9);
            this.filterText.Name = "filterText";
            this.filterText.Size = new System.Drawing.Size(133, 20);
            this.filterText.TabIndex = 7;
            // 
            // yAxisMaximumValue
            // 
            this.yAxisMaximumValue.Location = new System.Drawing.Point(229, 34);
            this.yAxisMaximumValue.Name = "yAxisMaximumValue";
            this.yAxisMaximumValue.Size = new System.Drawing.Size(121, 20);
            this.yAxisMaximumValue.TabIndex = 6;
            this.yAxisMaximumValue.Text = "10000000";
            // 
            // xAxisMaximumValue
            // 
            this.xAxisMaximumValue.Location = new System.Drawing.Point(7, 34);
            this.xAxisMaximumValue.Name = "xAxisMaximumValue";
            this.xAxisMaximumValue.Size = new System.Drawing.Size(121, 20);
            this.xAxisMaximumValue.TabIndex = 5;
            this.xAxisMaximumValue.Text = "10000000";
            // 
            // swapAxisButton
            // 
            this.swapAxisButton.Location = new System.Drawing.Point(134, 9);
            this.swapAxisButton.Name = "swapAxisButton";
            this.swapAxisButton.Size = new System.Drawing.Size(89, 20);
            this.swapAxisButton.TabIndex = 4;
            this.swapAxisButton.Text = "<->";
            this.swapAxisButton.UseVisualStyleBackColor = true;
            this.swapAxisButton.Click += new System.EventHandler(this.swapAxisButton_Click);
            // 
            // criteriaMass
            // 
            this.criteriaMass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.criteriaMass.FormattingEnabled = true;
            this.criteriaMass.Location = new System.Drawing.Point(602, 8);
            this.criteriaMass.Name = "criteriaMass";
            this.criteriaMass.Size = new System.Drawing.Size(121, 21);
            this.criteriaMass.TabIndex = 3;
            this.criteriaMass.DropDown += new System.EventHandler(this.criteriaMass_DropDown);
            // 
            // criteriaAxisY
            // 
            this.criteriaAxisY.FormattingEnabled = true;
            this.criteriaAxisY.Location = new System.Drawing.Point(229, 7);
            this.criteriaAxisY.Name = "criteriaAxisY";
            this.criteriaAxisY.Size = new System.Drawing.Size(121, 21);
            this.criteriaAxisY.TabIndex = 2;
            this.criteriaAxisY.DropDown += new System.EventHandler(this.criteriaAxisY_DropDown);
            // 
            // criteriaAxisX
            // 
            this.criteriaAxisX.FormattingEnabled = true;
            this.criteriaAxisX.Location = new System.Drawing.Point(7, 7);
            this.criteriaAxisX.Name = "criteriaAxisX";
            this.criteriaAxisX.Size = new System.Drawing.Size(121, 21);
            this.criteriaAxisX.TabIndex = 1;
            this.criteriaAxisX.DropDown += new System.EventHandler(this.criteriaAxisX_DropDown);
            // 
            // mapChart
            // 
            this.mapChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.mapChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.mapChart.Legends.Add(legend2);
            this.mapChart.Location = new System.Drawing.Point(-4, 60);
            this.mapChart.Name = "mapChart";
            this.mapChart.Size = new System.Drawing.Size(737, 355);
            this.mapChart.TabIndex = 0;
            this.mapChart.Text = "chart1";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Incinerator - Process Watcher";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // neuroPage
            // 
            this.neuroPage.Controls.Add(this.startNeuroCollectButton);
            this.neuroPage.Location = new System.Drawing.Point(4, 22);
            this.neuroPage.Name = "neuroPage";
            this.neuroPage.Size = new System.Drawing.Size(729, 415);
            this.neuroPage.TabIndex = 3;
            this.neuroPage.Text = "Нейро";
            this.neuroPage.UseVisualStyleBackColor = true;
            // 
            // startNeuroCollectButton
            // 
            this.startNeuroCollectButton.Location = new System.Drawing.Point(3, 3);
            this.startNeuroCollectButton.Name = "startNeuroCollectButton";
            this.startNeuroCollectButton.Size = new System.Drawing.Size(75, 47);
            this.startNeuroCollectButton.TabIndex = 0;
            this.startNeuroCollectButton.Text = "Старт";
            this.startNeuroCollectButton.UseVisualStyleBackColor = true;
            this.startNeuroCollectButton.Click += new System.EventHandler(this.startNeuroCollectButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 462);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.procCountLabel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Incinerator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainTabControl.ResumeLayout(false);
            this.lastInfoTab.ResumeLayout(false);
            this.lastInfoTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyUpDown)).EndInit();
            this.concreteProcessTab.ResumeLayout(false);
            this.concreteProcessTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.criteriaChart)).EndInit();
            this.mapPage.ResumeLayout(false);
            this.mapPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapChart)).EndInit();
            this.neuroPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer captureTimer;
        private System.Windows.Forms.Label procCountLabel;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage lastInfoTab;
        private System.Windows.Forms.Button outputLastInformationButton;
        private System.Windows.Forms.TabPage concreteProcessTab;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox criteriaSelector;
        private System.Windows.Forms.ComboBox processSelector;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.DataVisualization.Charting.Chart criteriaChart;
        private System.Windows.Forms.CheckBox outputLastInformationCheckBox;
        private System.Windows.Forms.Button loadStatButton;
        private System.Windows.Forms.Button saveStatButton;
        private System.Windows.Forms.TabPage mapPage;
        private System.Windows.Forms.DataVisualization.Charting.Chart mapChart;
        private System.Windows.Forms.ComboBox criteriaMass;
        private System.Windows.Forms.ComboBox criteriaAxisY;
        private System.Windows.Forms.ComboBox criteriaAxisX;
        private System.Windows.Forms.Button swapAxisButton;
        private System.Windows.Forms.TextBox yAxisMaximumValue;
        private System.Windows.Forms.TextBox xAxisMaximumValue;
        private System.Windows.Forms.TextBox filterText;
        private System.Windows.Forms.Label criteriaSpeedLabel;
        private System.Windows.Forms.Label processCriteriaLabel;
        private System.Windows.Forms.TextBox logPathTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown frequencyUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage neuroPage;
        private System.Windows.Forms.Button startNeuroCollectButton;
    }
}

