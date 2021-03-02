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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ClusterHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.showHistogram = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.HideHistogramButton = new System.Windows.Forms.Button();
            this.ViewClusters = new System.Windows.Forms.Button();
            this.ViewGroup = new System.Windows.Forms.GroupBox();
            this.ConfigBrowseButton = new System.Windows.Forms.Button();
            this.ConfigOrLabel = new System.Windows.Forms.Label();
            this.ConfigDirTextBox = new System.Windows.Forms.TextBox();
            this.ConfigPathLabel = new System.Windows.Forms.Label();
            this.BrowseViewButton = new System.Windows.Forms.Button();
            this.OrLabel = new System.Windows.Forms.Label();
            this.InViewFilePathBox = new System.Windows.Forms.TextBox();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.ClusterIndexValueLabel = new System.Windows.Forms.Label();
            this.ClusterIndexLabel = new System.Windows.Forms.Label();
            this.skeletonizeButton = new System.Windows.Forms.Button();
            this.showPixHistogramButton = new System.Windows.Forms.Button();
            this.hidePixHistogramButton = new System.Windows.Forms.Button();
            this.ClusterPixHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.winChartViewer = new ChartDirector.WinChartViewer();
            this.View3DButton = new System.Windows.Forms.Button();
            this.RotateUpButton = new System.Windows.Forms.Button();
            this.RotateDownButton = new System.Windows.Forms.Button();
            this.RotateLeftButton = new System.Windows.Forms.Button();
            this.RotateRightButton = new System.Windows.Forms.Button();
            this.NowViewingLabel = new System.Windows.Forms.Label();
            this.ShowBranchesButton = new System.Windows.Forms.Button();
            this.ShowDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.ShowCenterButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ControlGroupBox = new System.Windows.Forms.GroupBox();
            this.FindByIndexTextBox = new System.Windows.Forms.TextBox();
            this.ClusterFindIndexLabel = new System.Windows.Forms.Label();
            this.FindByIndexButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.ViewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).BeginInit();
            this.ShowDetailsGroupBox.SuspendLayout();
            this.ControlGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClusterHistogram
            // 
            chartArea1.Name = "ChartArea1";
            this.ClusterHistogram.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ClusterHistogram.Legends.Add(legend1);
            this.ClusterHistogram.Location = new System.Drawing.Point(687, 467);
            this.ClusterHistogram.Name = "ClusterHistogram";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ClusterHistogram.Series.Add(series1);
            this.ClusterHistogram.Size = new System.Drawing.Size(632, 253);
            this.ClusterHistogram.TabIndex = 0;
            this.ClusterHistogram.Text = "Collection Histogram";
            this.ClusterHistogram.Visible = false;
            // 
            // showHistogram
            // 
            this.showHistogram.Location = new System.Drawing.Point(687, 436);
            this.showHistogram.Name = "showHistogram";
            this.showHistogram.Size = new System.Drawing.Size(168, 25);
            this.showHistogram.TabIndex = 1;
            this.showHistogram.Text = "Show Collection Histogram";
            this.showHistogram.UseVisualStyleBackColor = true;
            this.showHistogram.Click += new System.EventHandler(this.ShowHistogramClicked);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Location = new System.Drawing.Point(6, 146);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(78, 32);
            this.PreviousButton.TabIndex = 2;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PrevButtonClicked);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(93, 146);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(89, 32);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButtonClicked);
            // 
            // PictureBox
            // 
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureBox.Location = new System.Drawing.Point(9, 217);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(610, 653);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // HideHistogramButton
            // 
            this.HideHistogramButton.Location = new System.Drawing.Point(855, 436);
            this.HideHistogramButton.Margin = new System.Windows.Forms.Padding(2);
            this.HideHistogramButton.Name = "HideHistogramButton";
            this.HideHistogramButton.Size = new System.Drawing.Size(154, 25);
            this.HideHistogramButton.TabIndex = 7;
            this.HideHistogramButton.Text = "Hide Collection Histogram";
            this.HideHistogramButton.UseVisualStyleBackColor = true;
            this.HideHistogramButton.Click += new System.EventHandler(this.HideHistogramClicked);
            // 
            // ViewClusters
            // 
            this.ViewClusters.Location = new System.Drawing.Point(0, 121);
            this.ViewClusters.Margin = new System.Windows.Forms.Padding(2);
            this.ViewClusters.Name = "ViewClusters";
            this.ViewClusters.Size = new System.Drawing.Size(381, 32);
            this.ViewClusters.TabIndex = 9;
            this.ViewClusters.Text = "Load Clusters";
            this.ViewClusters.UseVisualStyleBackColor = true;
            this.ViewClusters.Click += new System.EventHandler(this.LoadClustersClicked);
            // 
            // ViewGroup
            // 
            this.ViewGroup.Controls.Add(this.ConfigBrowseButton);
            this.ViewGroup.Controls.Add(this.ConfigOrLabel);
            this.ViewGroup.Controls.Add(this.ConfigDirTextBox);
            this.ViewGroup.Controls.Add(this.ConfigPathLabel);
            this.ViewGroup.Controls.Add(this.BrowseViewButton);
            this.ViewGroup.Controls.Add(this.OrLabel);
            this.ViewGroup.Controls.Add(this.InViewFilePathBox);
            this.ViewGroup.Controls.Add(this.FilePathLabel);
            this.ViewGroup.Controls.Add(this.ViewClusters);
            this.ViewGroup.Location = new System.Drawing.Point(9, 10);
            this.ViewGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Name = "ViewGroup";
            this.ViewGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Size = new System.Drawing.Size(395, 202);
            this.ViewGroup.TabIndex = 10;
            this.ViewGroup.TabStop = false;
            this.ViewGroup.Text = "Select file to view";
            // 
            // ConfigBrowseButton
            // 
            this.ConfigBrowseButton.Location = new System.Drawing.Point(300, 82);
            this.ConfigBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigBrowseButton.Name = "ConfigBrowseButton";
            this.ConfigBrowseButton.Size = new System.Drawing.Size(81, 20);
            this.ConfigBrowseButton.TabIndex = 17;
            this.ConfigBrowseButton.Text = "Browse...";
            this.ConfigBrowseButton.UseVisualStyleBackColor = true;
            this.ConfigBrowseButton.Click += new System.EventHandler(this.BrowseConfigButtonClicked);
            // 
            // ConfigOrLabel
            // 
            this.ConfigOrLabel.AutoSize = true;
            this.ConfigOrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.ConfigOrLabel.Location = new System.Drawing.Point(271, 85);
            this.ConfigOrLabel.Name = "ConfigOrLabel";
            this.ConfigOrLabel.Size = new System.Drawing.Size(24, 17);
            this.ConfigOrLabel.TabIndex = 16;
            this.ConfigOrLabel.Text = "Or";
            // 
            // ConfigDirTextBox
            // 
            this.ConfigDirTextBox.Location = new System.Drawing.Point(109, 82);
            this.ConfigDirTextBox.Name = "ConfigDirTextBox";
            this.ConfigDirTextBox.Size = new System.Drawing.Size(141, 20);
            this.ConfigDirTextBox.TabIndex = 15;
            // 
            // ConfigPathLabel
            // 
            this.ConfigPathLabel.AutoSize = true;
            this.ConfigPathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.ConfigPathLabel.Location = new System.Drawing.Point(3, 82);
            this.ConfigPathLabel.Name = "ConfigPathLabel";
            this.ConfigPathLabel.Size = new System.Drawing.Size(107, 17);
            this.ConfigPathLabel.TabIndex = 14;
            this.ConfigPathLabel.Text = "Config Dir Path:";
            // 
            // BrowseViewButton
            // 
            this.BrowseViewButton.Location = new System.Drawing.Point(300, 40);
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
            this.OrLabel.Location = new System.Drawing.Point(271, 40);
            this.OrLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OrLabel.Name = "OrLabel";
            this.OrLabel.Size = new System.Drawing.Size(24, 17);
            this.OrLabel.TabIndex = 12;
            this.OrLabel.Text = "Or";
            // 
            // InViewFilePathBox
            // 
            this.InViewFilePathBox.Location = new System.Drawing.Point(109, 37);
            this.InViewFilePathBox.Margin = new System.Windows.Forms.Padding(2);
            this.InViewFilePathBox.Name = "InViewFilePathBox";
            this.InViewFilePathBox.Size = new System.Drawing.Size(141, 20);
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
            // ClusterIndexValueLabel
            // 
            this.ClusterIndexValueLabel.AutoSize = true;
            this.ClusterIndexValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClusterIndexValueLabel.Location = new System.Drawing.Point(90, 45);
            this.ClusterIndexValueLabel.Name = "ClusterIndexValueLabel";
            this.ClusterIndexValueLabel.Size = new System.Drawing.Size(16, 17);
            this.ClusterIndexValueLabel.TabIndex = 19;
            this.ClusterIndexValueLabel.Text = "0";
            // 
            // ClusterIndexLabel
            // 
            this.ClusterIndexLabel.AutoSize = true;
            this.ClusterIndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClusterIndexLabel.Location = new System.Drawing.Point(6, 45);
            this.ClusterIndexLabel.Name = "ClusterIndexLabel";
            this.ClusterIndexLabel.Size = new System.Drawing.Size(78, 17);
            this.ClusterIndexLabel.TabIndex = 18;
            this.ClusterIndexLabel.Text = "Cluster No:";
            // 
            // skeletonizeButton
            // 
            this.skeletonizeButton.Location = new System.Drawing.Point(6, 123);
            this.skeletonizeButton.Name = "skeletonizeButton";
            this.skeletonizeButton.Size = new System.Drawing.Size(115, 40);
            this.skeletonizeButton.TabIndex = 11;
            this.skeletonizeButton.Text = "Skeletonize";
            this.skeletonizeButton.UseVisualStyleBackColor = true;
            this.skeletonizeButton.Click += new System.EventHandler(this.SkeletonizeButtonClicked);
            // 
            // showPixHistogramButton
            // 
            this.showPixHistogramButton.Location = new System.Drawing.Point(687, 712);
            this.showPixHistogramButton.Name = "showPixHistogramButton";
            this.showPixHistogramButton.Size = new System.Drawing.Size(168, 25);
            this.showPixHistogramButton.TabIndex = 13;
            this.showPixHistogramButton.Text = "Show Pixel Histogram";
            this.showPixHistogramButton.UseVisualStyleBackColor = true;
            this.showPixHistogramButton.Click += new System.EventHandler(this.ShowPixHistogramClicked);
            // 
            // hidePixHistogramButton
            // 
            this.hidePixHistogramButton.Location = new System.Drawing.Point(917, 712);
            this.hidePixHistogramButton.Name = "hidePixHistogramButton";
            this.hidePixHistogramButton.Size = new System.Drawing.Size(168, 25);
            this.hidePixHistogramButton.TabIndex = 14;
            this.hidePixHistogramButton.Text = "Hide Pixel Histogram";
            this.hidePixHistogramButton.UseVisualStyleBackColor = true;
            this.hidePixHistogramButton.Click += new System.EventHandler(this.HidePixHistogramClicked);
            // 
            // ClusterPixHistogram
            // 
            chartArea2.Name = "ChartArea1";
            this.ClusterPixHistogram.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ClusterPixHistogram.Legends.Add(legend2);
            this.ClusterPixHistogram.Location = new System.Drawing.Point(687, 743);
            this.ClusterPixHistogram.Name = "ClusterPixHistogram";
            this.ClusterPixHistogram.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ClusterPixHistogram.Series.Add(series2);
            this.ClusterPixHistogram.Size = new System.Drawing.Size(632, 220);
            this.ClusterPixHistogram.TabIndex = 15;
            this.ClusterPixHistogram.Visible = false;
            // 
            // winChartViewer
            // 
            this.winChartViewer.Location = new System.Drawing.Point(999, 10);
            this.winChartViewer.Name = "winChartViewer";
            this.winChartViewer.Size = new System.Drawing.Size(415, 389);
            this.winChartViewer.TabIndex = 16;
            this.winChartViewer.TabStop = false;
            // 
            // View3DButton
            // 
            this.View3DButton.Location = new System.Drawing.Point(999, 399);
            this.View3DButton.Name = "View3DButton";
            this.View3DButton.Size = new System.Drawing.Size(115, 41);
            this.View3DButton.TabIndex = 17;
            this.View3DButton.Text = "View 3D";
            this.View3DButton.UseVisualStyleBackColor = true;
            this.View3DButton.Click += new System.EventHandler(this.View3DClicked);
            // 
            // RotateUpButton
            // 
            this.RotateUpButton.Location = new System.Drawing.Point(1272, 397);
            this.RotateUpButton.Name = "RotateUpButton";
            this.RotateUpButton.Size = new System.Drawing.Size(47, 23);
            this.RotateUpButton.TabIndex = 19;
            this.RotateUpButton.Text = "Up";
            this.RotateUpButton.UseVisualStyleBackColor = true;
            this.RotateUpButton.Click += new System.EventHandler(this.Rotate3DPlot);
            // 
            // RotateDownButton
            // 
            this.RotateDownButton.Location = new System.Drawing.Point(1272, 438);
            this.RotateDownButton.Name = "RotateDownButton";
            this.RotateDownButton.Size = new System.Drawing.Size(47, 23);
            this.RotateDownButton.TabIndex = 20;
            this.RotateDownButton.Text = "Down";
            this.RotateDownButton.UseVisualStyleBackColor = true;
            this.RotateDownButton.Click += new System.EventHandler(this.Rotate3DPlot);
            // 
            // RotateLeftButton
            // 
            this.RotateLeftButton.Location = new System.Drawing.Point(1236, 415);
            this.RotateLeftButton.Name = "RotateLeftButton";
            this.RotateLeftButton.Size = new System.Drawing.Size(39, 25);
            this.RotateLeftButton.TabIndex = 21;
            this.RotateLeftButton.Text = "Left";
            this.RotateLeftButton.UseVisualStyleBackColor = true;
            this.RotateLeftButton.Click += new System.EventHandler(this.Rotate3DPlot);
            // 
            // RotateRightButton
            // 
            this.RotateRightButton.Location = new System.Drawing.Point(1316, 417);
            this.RotateRightButton.Name = "RotateRightButton";
            this.RotateRightButton.Size = new System.Drawing.Size(43, 23);
            this.RotateRightButton.TabIndex = 22;
            this.RotateRightButton.Text = "Right";
            this.RotateRightButton.UseVisualStyleBackColor = true;
            this.RotateRightButton.Click += new System.EventHandler(this.Rotate3DPlot);
            // 
            // NowViewingLabel
            // 
            this.NowViewingLabel.AutoSize = true;
            this.NowViewingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NowViewingLabel.Location = new System.Drawing.Point(5, 873);
            this.NowViewingLabel.MaximumSize = new System.Drawing.Size(500, 300);
            this.NowViewingLabel.Name = "NowViewingLabel";
            this.NowViewingLabel.Size = new System.Drawing.Size(124, 44);
            this.NowViewingLabel.TabIndex = 23;
            this.NowViewingLabel.Text = "Now Viewing: \r\n<empty>";
            // 
            // ShowBranchesButton
            // 
            this.ShowBranchesButton.Location = new System.Drawing.Point(127, 123);
            this.ShowBranchesButton.Name = "ShowBranchesButton";
            this.ShowBranchesButton.Size = new System.Drawing.Size(100, 40);
            this.ShowBranchesButton.TabIndex = 24;
            this.ShowBranchesButton.Text = "Show Branches";
            this.ShowBranchesButton.UseVisualStyleBackColor = true;
            this.ShowBranchesButton.Click += new System.EventHandler(this.ViewBranchButtonClicked);
            // 
            // ShowDetailsGroupBox
            // 
            this.ShowDetailsGroupBox.Controls.Add(this.ShowCenterButton);
            this.ShowDetailsGroupBox.Controls.Add(this.button2);
            this.ShowDetailsGroupBox.Controls.Add(this.label1);
            this.ShowDetailsGroupBox.Controls.Add(this.textBox1);
            this.ShowDetailsGroupBox.Controls.Add(this.label2);
            this.ShowDetailsGroupBox.Controls.Add(this.button1);
            this.ShowDetailsGroupBox.Controls.Add(this.ShowBranchesButton);
            this.ShowDetailsGroupBox.Controls.Add(this.skeletonizeButton);
            this.ShowDetailsGroupBox.Location = new System.Drawing.Point(597, 12);
            this.ShowDetailsGroupBox.Name = "ShowDetailsGroupBox";
            this.ShowDetailsGroupBox.Size = new System.Drawing.Size(396, 200);
            this.ShowDetailsGroupBox.TabIndex = 25;
            this.ShowDetailsGroupBox.TabStop = false;
            this.ShowDetailsGroupBox.Text = "Show Details";
            // 
            // ShowCenterButton
            // 
            this.ShowCenterButton.Location = new System.Drawing.Point(233, 123);
            this.ShowCenterButton.Name = "ShowCenterButton";
            this.ShowCenterButton.Size = new System.Drawing.Size(96, 40);
            this.ShowCenterButton.TabIndex = 30;
            this.ShowCenterButton.Text = "Show Center";
            this.ShowCenterButton.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(303, 35);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 20);
            this.button2.TabIndex = 29;
            this.button2.Text = "Browse...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label1.Location = new System.Drawing.Point(274, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 28;
            this.label1.Text = "Or";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(141, 20);
            this.textBox1.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 26;
            this.label2.Text = "Description file:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(384, 32);
            this.button1.TabIndex = 25;
            this.button1.Text = "Load Description File";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ControlGroupBox
            // 
            this.ControlGroupBox.Controls.Add(this.FindByIndexTextBox);
            this.ControlGroupBox.Controls.Add(this.ClusterFindIndexLabel);
            this.ControlGroupBox.Controls.Add(this.FindByIndexButton);
            this.ControlGroupBox.Controls.Add(this.ClusterIndexValueLabel);
            this.ControlGroupBox.Controls.Add(this.PreviousButton);
            this.ControlGroupBox.Controls.Add(this.ClusterIndexLabel);
            this.ControlGroupBox.Controls.Add(this.NextButton);
            this.ControlGroupBox.Location = new System.Drawing.Point(409, 12);
            this.ControlGroupBox.Name = "ControlGroupBox";
            this.ControlGroupBox.Size = new System.Drawing.Size(188, 195);
            this.ControlGroupBox.TabIndex = 26;
            this.ControlGroupBox.TabStop = false;
            this.ControlGroupBox.Text = "Control Panel";
            // 
            // FindByIndexTextBox
            // 
            this.FindByIndexTextBox.Location = new System.Drawing.Point(49, 71);
            this.FindByIndexTextBox.Name = "FindByIndexTextBox";
            this.FindByIndexTextBox.Size = new System.Drawing.Size(52, 20);
            this.FindByIndexTextBox.TabIndex = 22;
            // 
            // ClusterFindIndexLabel
            // 
            this.ClusterFindIndexLabel.AutoSize = true;
            this.ClusterFindIndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClusterFindIndexLabel.Location = new System.Drawing.Point(2, 73);
            this.ClusterFindIndexLabel.Name = "ClusterFindIndexLabel";
            this.ClusterFindIndexLabel.Size = new System.Drawing.Size(45, 17);
            this.ClusterFindIndexLabel.TabIndex = 21;
            this.ClusterFindIndexLabel.Text = "Index:";
            // 
            // FindByIndexButton
            // 
            this.FindByIndexButton.Location = new System.Drawing.Point(107, 69);
            this.FindByIndexButton.Name = "FindByIndexButton";
            this.FindByIndexButton.Size = new System.Drawing.Size(75, 23);
            this.FindByIndexButton.TabIndex = 20;
            this.FindByIndexButton.Text = "Find";
            this.FindByIndexButton.UseVisualStyleBackColor = true;
            this.FindByIndexButton.Click += new System.EventHandler(this.FindClusterByIndexClicked);
            // 
            // ClusterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 962);
            this.Controls.Add(this.ControlGroupBox);
            this.Controls.Add(this.ShowDetailsGroupBox);
            this.Controls.Add(this.NowViewingLabel);
            this.Controls.Add(this.RotateRightButton);
            this.Controls.Add(this.RotateLeftButton);
            this.Controls.Add(this.RotateDownButton);
            this.Controls.Add(this.RotateUpButton);
            this.Controls.Add(this.View3DButton);
            this.Controls.Add(this.winChartViewer);
            this.Controls.Add(this.ClusterPixHistogram);
            this.Controls.Add(this.hidePixHistogramButton);
            this.Controls.Add(this.showPixHistogramButton);
            this.Controls.Add(this.ViewGroup);
            this.Controls.Add(this.HideHistogramButton);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.showHistogram);
            this.Controls.Add(this.ClusterHistogram);
            this.Name = "ClusterUI";
            this.Text = "ClusterViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ViewGroup.ResumeLayout(false);
            this.ViewGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).EndInit();
            this.ShowDetailsGroupBox.ResumeLayout(false);
            this.ShowDetailsGroupBox.PerformLayout();
            this.ControlGroupBox.ResumeLayout(false);
            this.ControlGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ClusterHistogram;
        private System.Windows.Forms.Button showHistogram;
        private System.Windows.Forms.Button PreviousButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Button HideHistogramButton;
        private System.Windows.Forms.Button ViewClusters;
        private System.Windows.Forms.GroupBox ViewGroup;
        private System.Windows.Forms.Button BrowseViewButton;
        private System.Windows.Forms.Label OrLabel;
        private System.Windows.Forms.TextBox InViewFilePathBox;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.Button skeletonizeButton;
        private System.Windows.Forms.Button showPixHistogramButton;
        private System.Windows.Forms.Button hidePixHistogramButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart ClusterPixHistogram;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ChartDirector.WinChartViewer winChartViewer;
        private System.Windows.Forms.Button View3DButton;
        private System.Windows.Forms.Button RotateUpButton;
        private System.Windows.Forms.Button RotateDownButton;
        private System.Windows.Forms.Button RotateLeftButton;
        private System.Windows.Forms.Button RotateRightButton;
        private System.Windows.Forms.Label NowViewingLabel;
        private System.Windows.Forms.Button ShowBranchesButton;
        private System.Windows.Forms.Button ConfigBrowseButton;
        private System.Windows.Forms.Label ConfigOrLabel;
        private System.Windows.Forms.TextBox ConfigDirTextBox;
        private System.Windows.Forms.Label ConfigPathLabel;
        private System.Windows.Forms.GroupBox ShowDetailsGroupBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ShowCenterButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ClusterIndexValueLabel;
        private System.Windows.Forms.Label ClusterIndexLabel;
        private System.Windows.Forms.GroupBox ControlGroupBox;
        private System.Windows.Forms.TextBox FindByIndexTextBox;
        private System.Windows.Forms.Label ClusterFindIndexLabel;
        private System.Windows.Forms.Button FindByIndexButton;
    }
}

