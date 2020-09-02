namespace ClusterUI
{
    partial class ClusterUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ClusterHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.showHistogram = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.FilterButton = new System.Windows.Forms.Button();
            this.FilterGroup = new System.Windows.Forms.GroupBox();
            this.ToLinearityTextBox = new System.Windows.Forms.TextBox();
            this.FromLinearityTextBox = new System.Windows.Forms.TextBox();
            this.labelToLinearity = new System.Windows.Forms.Label();
            this.labelFromLinearity = new System.Windows.Forms.Label();
            this.linearityLabel = new System.Windows.Forms.Label();
            this.pixCountLabel = new System.Windows.Forms.Label();
            this.energyLabel = new System.Windows.Forms.Label();
            this.ToPixCountFilterBox = new System.Windows.Forms.TextBox();
            this.labelToPixCount = new System.Windows.Forms.Label();
            this.labelFromPixCount = new System.Windows.Forms.Label();
            this.FromPixCountFilterBox = new System.Windows.Forms.TextBox();
            this.labelToEnergyBox = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFromEnergyBox = new System.Windows.Forms.Label();
            this.ToEnergyFilterBox = new System.Windows.Forms.TextBox();
            this.FromEnergyFilterBox = new System.Windows.Forms.TextBox();
            this.HideHistogramButton = new System.Windows.Forms.Button();
            this.InputFileGroup = new System.Windows.Forms.GroupBox();
            this.OutFileNameIniBox = new System.Windows.Forms.TextBox();
            this.OutIniFileLabel = new System.Windows.Forms.Label();
            this.OutFileNameClBox = new System.Windows.Forms.TextBox();
            this.OutClFileLabel = new System.Windows.Forms.Label();
            this.BrowseFilteredFileButton = new System.Windows.Forms.Button();
            this.Or2Label = new System.Windows.Forms.Label();
            this.InFilePathBox = new System.Windows.Forms.TextBox();
            this.InputFileLabel = new System.Windows.Forms.Label();
            this.ViewClusters = new System.Windows.Forms.Button();
            this.ViewGroup = new System.Windows.Forms.GroupBox();
            this.BrowseViewButton = new System.Windows.Forms.Button();
            this.OrLabel = new System.Windows.Forms.Label();
            this.InViewFilePathBox = new System.Windows.Forms.TextBox();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.skeletonizeButton = new System.Windows.Forms.Button();
            this.showPixHistogramButton = new System.Windows.Forms.Button();
            this.hidePixHistogramButton = new System.Windows.Forms.Button();
            this.ClusterPixHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.winChartViewer = new ChartDirector.WinChartViewer();
            this.View3DButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.FilterGroup.SuspendLayout();
            this.InputFileGroup.SuspendLayout();
            this.ViewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // ClusterHistogram
            // 
            chartArea3.Name = "ChartArea1";
            this.ClusterHistogram.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.ClusterHistogram.Legends.Add(legend3);
            this.ClusterHistogram.Location = new System.Drawing.Point(631, 406);
            this.ClusterHistogram.Name = "ClusterHistogram";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.ClusterHistogram.Series.Add(series3);
            this.ClusterHistogram.Size = new System.Drawing.Size(786, 273);
            this.ClusterHistogram.TabIndex = 0;
            this.ClusterHistogram.Text = "Collection Histogram";
            // 
            // showHistogram
            // 
            this.showHistogram.Location = new System.Drawing.Point(631, 366);
            this.showHistogram.Name = "showHistogram";
            this.showHistogram.Size = new System.Drawing.Size(168, 25);
            this.showHistogram.TabIndex = 1;
            this.showHistogram.Text = "Show Collection Histogram";
            this.showHistogram.UseVisualStyleBackColor = true;
            this.showHistogram.Click += new System.EventHandler(this.ShowHistogramClicked);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Location = new System.Drawing.Point(5, 126);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(100, 32);
            this.PreviousButton.TabIndex = 2;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PrevButtonClicked);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(118, 126);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(105, 32);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButtonClicked);
            // 
            // PictureBox
            // 
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureBox.Location = new System.Drawing.Point(9, 177);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(601, 651);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // FilterButton
            // 
            this.FilterButton.Location = new System.Drawing.Point(632, 300);
            this.FilterButton.Margin = new System.Windows.Forms.Padding(2);
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(104, 42);
            this.FilterButton.TabIndex = 5;
            this.FilterButton.Text = "Process";
            this.FilterButton.UseVisualStyleBackColor = true;
            this.FilterButton.Click += new System.EventHandler(this.ProcessFilterClicked);
            // 
            // FilterGroup
            // 
            this.FilterGroup.Controls.Add(this.ToLinearityTextBox);
            this.FilterGroup.Controls.Add(this.FromLinearityTextBox);
            this.FilterGroup.Controls.Add(this.labelToLinearity);
            this.FilterGroup.Controls.Add(this.labelFromLinearity);
            this.FilterGroup.Controls.Add(this.linearityLabel);
            this.FilterGroup.Controls.Add(this.pixCountLabel);
            this.FilterGroup.Controls.Add(this.energyLabel);
            this.FilterGroup.Controls.Add(this.ToPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.labelToPixCount);
            this.FilterGroup.Controls.Add(this.labelFromPixCount);
            this.FilterGroup.Controls.Add(this.FromPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.labelToEnergyBox);
            this.FilterGroup.Controls.Add(this.label1);
            this.FilterGroup.Controls.Add(this.labelFromEnergyBox);
            this.FilterGroup.Controls.Add(this.ToEnergyFilterBox);
            this.FilterGroup.Controls.Add(this.FromEnergyFilterBox);
            this.FilterGroup.Location = new System.Drawing.Point(631, 145);
            this.FilterGroup.Margin = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Name = "FilterGroup";
            this.FilterGroup.Padding = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Size = new System.Drawing.Size(387, 151);
            this.FilterGroup.TabIndex = 6;
            this.FilterGroup.TabStop = false;
            this.FilterGroup.Text = "Select filters";
            // 
            // ToLinearityTextBox
            // 
            this.ToLinearityTextBox.Location = new System.Drawing.Point(274, 101);
            this.ToLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToLinearityTextBox.Name = "ToLinearityTextBox";
            this.ToLinearityTextBox.Size = new System.Drawing.Size(57, 20);
            this.ToLinearityTextBox.TabIndex = 16;
            // 
            // FromLinearityTextBox
            // 
            this.FromLinearityTextBox.Location = new System.Drawing.Point(158, 101);
            this.FromLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromLinearityTextBox.Name = "FromLinearityTextBox";
            this.FromLinearityTextBox.Size = new System.Drawing.Size(57, 20);
            this.FromLinearityTextBox.TabIndex = 15;
            // 
            // labelToLinearity
            // 
            this.labelToLinearity.AutoSize = true;
            this.labelToLinearity.Location = new System.Drawing.Point(236, 108);
            this.labelToLinearity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToLinearity.Name = "labelToLinearity";
            this.labelToLinearity.Size = new System.Drawing.Size(23, 13);
            this.labelToLinearity.TabIndex = 14;
            this.labelToLinearity.Text = "To:";
            // 
            // labelFromLinearity
            // 
            this.labelFromLinearity.AutoSize = true;
            this.labelFromLinearity.Location = new System.Drawing.Point(121, 108);
            this.labelFromLinearity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromLinearity.Name = "labelFromLinearity";
            this.labelFromLinearity.Size = new System.Drawing.Size(33, 13);
            this.labelFromLinearity.TabIndex = 13;
            this.labelFromLinearity.Text = "From:";
            // 
            // linearityLabel
            // 
            this.linearityLabel.AutoSize = true;
            this.linearityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linearityLabel.Location = new System.Drawing.Point(4, 108);
            this.linearityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linearityLabel.Name = "linearityLabel";
            this.linearityLabel.Size = new System.Drawing.Size(102, 17);
            this.linearityLabel.TabIndex = 12;
            this.linearityLabel.Text = "Convexity(%)";
            // 
            // pixCountLabel
            // 
            this.pixCountLabel.AutoSize = true;
            this.pixCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.pixCountLabel.Location = new System.Drawing.Point(4, 68);
            this.pixCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.pixCountLabel.Name = "pixCountLabel";
            this.pixCountLabel.Size = new System.Drawing.Size(89, 17);
            this.pixCountLabel.TabIndex = 10;
            this.pixCountLabel.Text = "Pixel Count";
            // 
            // energyLabel
            // 
            this.energyLabel.AutoSize = true;
            this.energyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.energyLabel.Location = new System.Drawing.Point(4, 32);
            this.energyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.energyLabel.Name = "energyLabel";
            this.energyLabel.Size = new System.Drawing.Size(121, 17);
            this.energyLabel.TabIndex = 9;
            this.energyLabel.Text = "Energy (in keV)";
            // 
            // ToPixCountFilterBox
            // 
            this.ToPixCountFilterBox.Location = new System.Drawing.Point(274, 68);
            this.ToPixCountFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToPixCountFilterBox.Name = "ToPixCountFilterBox";
            this.ToPixCountFilterBox.Size = new System.Drawing.Size(57, 20);
            this.ToPixCountFilterBox.TabIndex = 8;
            // 
            // labelToPixCount
            // 
            this.labelToPixCount.AutoSize = true;
            this.labelToPixCount.Location = new System.Drawing.Point(236, 71);
            this.labelToPixCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToPixCount.Name = "labelToPixCount";
            this.labelToPixCount.Size = new System.Drawing.Size(23, 13);
            this.labelToPixCount.TabIndex = 7;
            this.labelToPixCount.Text = "To:";
            // 
            // labelFromPixCount
            // 
            this.labelFromPixCount.AutoSize = true;
            this.labelFromPixCount.Location = new System.Drawing.Point(121, 71);
            this.labelFromPixCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromPixCount.Name = "labelFromPixCount";
            this.labelFromPixCount.Size = new System.Drawing.Size(33, 13);
            this.labelFromPixCount.TabIndex = 6;
            this.labelFromPixCount.Text = "From:";
            // 
            // FromPixCountFilterBox
            // 
            this.FromPixCountFilterBox.Location = new System.Drawing.Point(158, 68);
            this.FromPixCountFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromPixCountFilterBox.Name = "FromPixCountFilterBox";
            this.FromPixCountFilterBox.Size = new System.Drawing.Size(57, 20);
            this.FromPixCountFilterBox.TabIndex = 5;
            // 
            // labelToEnergyBox
            // 
            this.labelToEnergyBox.AutoSize = true;
            this.labelToEnergyBox.Location = new System.Drawing.Point(236, 36);
            this.labelToEnergyBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToEnergyBox.Name = "labelToEnergyBox";
            this.labelToEnergyBox.Size = new System.Drawing.Size(23, 13);
            this.labelToEnergyBox.TabIndex = 4;
            this.labelToEnergyBox.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(173, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 3;
            // 
            // labelFromEnergyBox
            // 
            this.labelFromEnergyBox.AutoSize = true;
            this.labelFromEnergyBox.Location = new System.Drawing.Point(121, 35);
            this.labelFromEnergyBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromEnergyBox.Name = "labelFromEnergyBox";
            this.labelFromEnergyBox.Size = new System.Drawing.Size(33, 13);
            this.labelFromEnergyBox.TabIndex = 2;
            this.labelFromEnergyBox.Text = "From:";
            // 
            // ToEnergyFilterBox
            // 
            this.ToEnergyFilterBox.Location = new System.Drawing.Point(274, 33);
            this.ToEnergyFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToEnergyFilterBox.Name = "ToEnergyFilterBox";
            this.ToEnergyFilterBox.Size = new System.Drawing.Size(57, 20);
            this.ToEnergyFilterBox.TabIndex = 1;
            // 
            // FromEnergyFilterBox
            // 
            this.FromEnergyFilterBox.Location = new System.Drawing.Point(158, 32);
            this.FromEnergyFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromEnergyFilterBox.Name = "FromEnergyFilterBox";
            this.FromEnergyFilterBox.Size = new System.Drawing.Size(57, 20);
            this.FromEnergyFilterBox.TabIndex = 0;
            // 
            // HideHistogramButton
            // 
            this.HideHistogramButton.Location = new System.Drawing.Point(818, 366);
            this.HideHistogramButton.Margin = new System.Windows.Forms.Padding(2);
            this.HideHistogramButton.Name = "HideHistogramButton";
            this.HideHistogramButton.Size = new System.Drawing.Size(154, 25);
            this.HideHistogramButton.TabIndex = 7;
            this.HideHistogramButton.Text = "Hide Collection Histogram";
            this.HideHistogramButton.UseVisualStyleBackColor = true;
            this.HideHistogramButton.Click += new System.EventHandler(this.HideHistogramClicked);
            // 
            // InputFileGroup
            // 
            this.InputFileGroup.Controls.Add(this.OutFileNameIniBox);
            this.InputFileGroup.Controls.Add(this.OutIniFileLabel);
            this.InputFileGroup.Controls.Add(this.OutFileNameClBox);
            this.InputFileGroup.Controls.Add(this.OutClFileLabel);
            this.InputFileGroup.Controls.Add(this.BrowseFilteredFileButton);
            this.InputFileGroup.Controls.Add(this.Or2Label);
            this.InputFileGroup.Controls.Add(this.InFilePathBox);
            this.InputFileGroup.Controls.Add(this.InputFileLabel);
            this.InputFileGroup.Location = new System.Drawing.Point(625, 10);
            this.InputFileGroup.Margin = new System.Windows.Forms.Padding(2);
            this.InputFileGroup.Name = "InputFileGroup";
            this.InputFileGroup.Padding = new System.Windows.Forms.Padding(2);
            this.InputFileGroup.Size = new System.Drawing.Size(387, 130);
            this.InputFileGroup.TabIndex = 8;
            this.InputFileGroup.TabStop = false;
            this.InputFileGroup.Text = "Select input and output file for filters";
            // 
            // OutFileNameIniBox
            // 
            this.OutFileNameIniBox.Location = new System.Drawing.Point(103, 92);
            this.OutFileNameIniBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutFileNameIniBox.Name = "OutFileNameIniBox";
            this.OutFileNameIniBox.Size = new System.Drawing.Size(148, 20);
            this.OutFileNameIniBox.TabIndex = 21;
            // 
            // OutIniFileLabel
            // 
            this.OutIniFileLabel.AutoSize = true;
            this.OutIniFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OutIniFileLabel.Location = new System.Drawing.Point(4, 94);
            this.OutIniFileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OutIniFileLabel.Name = "OutIniFileLabel";
            this.OutIniFileLabel.Size = new System.Drawing.Size(105, 17);
            this.OutIniFileLabel.TabIndex = 20;
            this.OutIniFileLabel.Text = "Output (.ini) file";
            // 
            // OutFileNameClBox
            // 
            this.OutFileNameClBox.Location = new System.Drawing.Point(103, 59);
            this.OutFileNameClBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutFileNameClBox.Name = "OutFileNameClBox";
            this.OutFileNameClBox.Size = new System.Drawing.Size(148, 20);
            this.OutFileNameClBox.TabIndex = 19;
            // 
            // OutClFileLabel
            // 
            this.OutClFileLabel.AutoSize = true;
            this.OutClFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OutClFileLabel.Location = new System.Drawing.Point(4, 61);
            this.OutClFileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OutClFileLabel.Name = "OutClFileLabel";
            this.OutClFileLabel.Size = new System.Drawing.Size(101, 17);
            this.OutClFileLabel.TabIndex = 18;
            this.OutClFileLabel.Text = "Output (.cl) file";
            // 
            // BrowseFilteredFileButton
            // 
            this.BrowseFilteredFileButton.Location = new System.Drawing.Point(292, 26);
            this.BrowseFilteredFileButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseFilteredFileButton.Name = "BrowseFilteredFileButton";
            this.BrowseFilteredFileButton.Size = new System.Drawing.Size(81, 24);
            this.BrowseFilteredFileButton.TabIndex = 17;
            this.BrowseFilteredFileButton.Text = "Browse...";
            this.BrowseFilteredFileButton.UseVisualStyleBackColor = true;
            this.BrowseFilteredFileButton.Click += new System.EventHandler(this.BrowseFilterFileButtonClicked);
            // 
            // Or2Label
            // 
            this.Or2Label.AutoSize = true;
            this.Or2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Or2Label.Location = new System.Drawing.Point(254, 28);
            this.Or2Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Or2Label.Name = "Or2Label";
            this.Or2Label.Size = new System.Drawing.Size(24, 17);
            this.Or2Label.TabIndex = 16;
            this.Or2Label.Text = "Or";
            // 
            // InFilePathBox
            // 
            this.InFilePathBox.Location = new System.Drawing.Point(103, 26);
            this.InFilePathBox.Margin = new System.Windows.Forms.Padding(2);
            this.InFilePathBox.Name = "InFilePathBox";
            this.InFilePathBox.Size = new System.Drawing.Size(148, 20);
            this.InFilePathBox.TabIndex = 15;
            // 
            // InputFileLabel
            // 
            this.InputFileLabel.AutoSize = true;
            this.InputFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.InputFileLabel.Location = new System.Drawing.Point(4, 24);
            this.InputFileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.InputFileLabel.Name = "InputFileLabel";
            this.InputFileLabel.Size = new System.Drawing.Size(93, 17);
            this.InputFileLabel.TabIndex = 14;
            this.InputFileLabel.Text = "Input file path";
            // 
            // ViewClusters
            // 
            this.ViewClusters.Location = new System.Drawing.Point(4, 71);
            this.ViewClusters.Margin = new System.Windows.Forms.Padding(2);
            this.ViewClusters.Name = "ViewClusters";
            this.ViewClusters.Size = new System.Drawing.Size(397, 32);
            this.ViewClusters.TabIndex = 9;
            this.ViewClusters.Text = "Load Clusters";
            this.ViewClusters.UseVisualStyleBackColor = true;
            this.ViewClusters.Click += new System.EventHandler(this.LoadClustersClicked);
            // 
            // ViewGroup
            // 
            this.ViewGroup.Controls.Add(this.BrowseViewButton);
            this.ViewGroup.Controls.Add(this.OrLabel);
            this.ViewGroup.Controls.Add(this.InViewFilePathBox);
            this.ViewGroup.Controls.Add(this.FilePathLabel);
            this.ViewGroup.Controls.Add(this.PreviousButton);
            this.ViewGroup.Controls.Add(this.ViewClusters);
            this.ViewGroup.Controls.Add(this.NextButton);
            this.ViewGroup.Location = new System.Drawing.Point(9, 10);
            this.ViewGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Name = "ViewGroup";
            this.ViewGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Size = new System.Drawing.Size(406, 164);
            this.ViewGroup.TabIndex = 10;
            this.ViewGroup.TabStop = false;
            this.ViewGroup.Text = "Select file to view";
            // 
            // BrowseViewButton
            // 
            this.BrowseViewButton.Location = new System.Drawing.Point(274, 41);
            this.BrowseViewButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseViewButton.Name = "BrowseViewButton";
            this.BrowseViewButton.Size = new System.Drawing.Size(81, 20);
            this.BrowseViewButton.TabIndex = 13;
            this.BrowseViewButton.Text = "Browse...";
            this.BrowseViewButton.UseVisualStyleBackColor = true;
            this.BrowseViewButton.Click += new System.EventHandler(this.BrowseViewButtonClicked);
            // 
            // OrLabel
            // 
            this.OrLabel.AutoSize = true;
            this.OrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OrLabel.Location = new System.Drawing.Point(236, 42);
            this.OrLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OrLabel.Name = "OrLabel";
            this.OrLabel.Size = new System.Drawing.Size(24, 17);
            this.OrLabel.TabIndex = 12;
            this.OrLabel.Text = "Or";
            // 
            // InViewFilePathBox
            // 
            this.InViewFilePathBox.Location = new System.Drawing.Point(68, 41);
            this.InViewFilePathBox.Margin = new System.Windows.Forms.Padding(2);
            this.InViewFilePathBox.Name = "InViewFilePathBox";
            this.InViewFilePathBox.Size = new System.Drawing.Size(157, 20);
            this.InViewFilePathBox.TabIndex = 11;
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FilePathLabel.Location = new System.Drawing.Point(4, 41);
            this.FilePathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(66, 17);
            this.FilePathLabel.TabIndex = 10;
            this.FilePathLabel.Text = "File path:";
            // 
            // skeletonizeButton
            // 
            this.skeletonizeButton.Location = new System.Drawing.Point(433, 138);
            this.skeletonizeButton.Name = "skeletonizeButton";
            this.skeletonizeButton.Size = new System.Drawing.Size(102, 28);
            this.skeletonizeButton.TabIndex = 11;
            this.skeletonizeButton.Text = "Skeletonize";
            this.skeletonizeButton.UseVisualStyleBackColor = true;
            this.skeletonizeButton.Click += new System.EventHandler(this.SkeletonizeButtonClicked);
            // 
            // showPixHistogramButton
            // 
            this.showPixHistogramButton.Location = new System.Drawing.Point(638, 697);
            this.showPixHistogramButton.Name = "showPixHistogramButton";
            this.showPixHistogramButton.Size = new System.Drawing.Size(168, 25);
            this.showPixHistogramButton.TabIndex = 13;
            this.showPixHistogramButton.Text = "Show Pixel Histogram";
            this.showPixHistogramButton.UseVisualStyleBackColor = true;
            this.showPixHistogramButton.Click += new System.EventHandler(this.ShowPixHistogramClicked);
            // 
            // hidePixHistogramButton
            // 
            this.hidePixHistogramButton.Location = new System.Drawing.Point(818, 697);
            this.hidePixHistogramButton.Name = "hidePixHistogramButton";
            this.hidePixHistogramButton.Size = new System.Drawing.Size(168, 25);
            this.hidePixHistogramButton.TabIndex = 14;
            this.hidePixHistogramButton.Text = "Hide Pixel Histogram";
            this.hidePixHistogramButton.UseVisualStyleBackColor = true;
            // 
            // ClusterPixHistogram
            // 
            chartArea4.Name = "ChartArea1";
            this.ClusterPixHistogram.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.ClusterPixHistogram.Legends.Add(legend4);
            this.ClusterPixHistogram.Location = new System.Drawing.Point(632, 728);
            this.ClusterPixHistogram.Name = "ClusterPixHistogram";
            this.ClusterPixHistogram.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.ClusterPixHistogram.Series.Add(series4);
            this.ClusterPixHistogram.Size = new System.Drawing.Size(786, 230);
            this.ClusterPixHistogram.TabIndex = 15;
            this.ClusterPixHistogram.Text = "Pixel Histogram";
            // 
            // winChartViewer
            // 
            this.winChartViewer.Location = new System.Drawing.Point(1023, 69);
            this.winChartViewer.Name = "winChartViewer";
            this.winChartViewer.Size = new System.Drawing.Size(395, 320);
            this.winChartViewer.TabIndex = 16;
            this.winChartViewer.TabStop = false;
            // 
            // View3DButton
            // 
            this.View3DButton.Location = new System.Drawing.Point(1023, 5);
            this.View3DButton.Name = "View3DButton";
            this.View3DButton.Size = new System.Drawing.Size(98, 58);
            this.View3DButton.TabIndex = 17;
            this.View3DButton.Text = "View 3D";
            this.View3DButton.UseVisualStyleBackColor = true;
            this.View3DButton.Click += new System.EventHandler(this.View3DClicked);
            // 
            // ClusterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 962);
            this.Controls.Add(this.View3DButton);
            this.Controls.Add(this.winChartViewer);
            this.Controls.Add(this.ClusterPixHistogram);
            this.Controls.Add(this.hidePixHistogramButton);
            this.Controls.Add(this.showPixHistogramButton);
            this.Controls.Add(this.skeletonizeButton);
            this.Controls.Add(this.ViewGroup);
            this.Controls.Add(this.InputFileGroup);
            this.Controls.Add(this.HideHistogramButton);
            this.Controls.Add(this.FilterButton);
            this.Controls.Add(this.FilterGroup);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.showHistogram);
            this.Controls.Add(this.ClusterHistogram);
            this.Name = "ClusterUI";
            this.Text = "ClusterViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.FilterGroup.ResumeLayout(false);
            this.FilterGroup.PerformLayout();
            this.InputFileGroup.ResumeLayout(false);
            this.InputFileGroup.PerformLayout();
            this.ViewGroup.ResumeLayout(false);
            this.ViewGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ClusterHistogram;
        private System.Windows.Forms.Button showHistogram;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Button FilterButton;
        private System.Windows.Forms.GroupBox FilterGroup;
        private System.Windows.Forms.Label pixCountLabel;
        private System.Windows.Forms.Label energyLabel;
        private System.Windows.Forms.TextBox ToPixCountFilterBox;
        private System.Windows.Forms.Label labelToPixCount;
        private System.Windows.Forms.Label labelFromPixCount;
        private System.Windows.Forms.TextBox FromPixCountFilterBox;
        private System.Windows.Forms.Label labelToEnergyBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFromEnergyBox;
        private System.Windows.Forms.TextBox ToEnergyFilterBox;
        private System.Windows.Forms.TextBox FromEnergyFilterBox;
        private System.Windows.Forms.Button HideHistogramButton;
        private System.Windows.Forms.GroupBox InputFileGroup;
        private System.Windows.Forms.TextBox OutFileNameClBox;
        private System.Windows.Forms.Label OutClFileLabel;
        private System.Windows.Forms.Button BrowseFilteredFileButton;
        private System.Windows.Forms.Label Or2Label;
        private System.Windows.Forms.TextBox InFilePathBox;
        private System.Windows.Forms.Label InputFileLabel;
        private System.Windows.Forms.Button ViewClusters;
        private System.Windows.Forms.GroupBox ViewGroup;
        private System.Windows.Forms.Button BrowseViewButton;
        private System.Windows.Forms.Label OrLabel;
        private System.Windows.Forms.TextBox InViewFilePathBox;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.Label OutIniFileLabel;
        private System.Windows.Forms.TextBox OutFileNameIniBox;
        private System.Windows.Forms.Label linearityLabel;
        private System.Windows.Forms.TextBox ToLinearityTextBox;
        private System.Windows.Forms.TextBox FromLinearityTextBox;
        private System.Windows.Forms.Label labelToLinearity;
        private System.Windows.Forms.Label labelFromLinearity;
        private System.Windows.Forms.Button skeletonizeButton;
        private System.Windows.Forms.Button showPixHistogramButton;
        private System.Windows.Forms.Button hidePixHistogramButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart ClusterPixHistogram;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ChartDirector.WinChartViewer winChartViewer;
        private System.Windows.Forms.Button View3DButton;
    }
}

