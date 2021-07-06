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
            this.ViewGroup = new System.Windows.Forms.GroupBox();
            this.BrowseViewButton = new System.Windows.Forms.Button();
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
            this.ShowAttributesButton = new System.Windows.Forms.Button();
            this.ClassifyButton = new System.Windows.Forms.Button();
            this.ControlGroupBox = new System.Windows.Forms.GroupBox();
            this.FindByIndexTextBox = new System.Windows.Forms.TextBox();
            this.ClusterFindIndexLabel = new System.Windows.Forms.Label();
            this.FindByIndexButton = new System.Windows.Forms.Button();
            this.ZoomLabel = new System.Windows.Forms.Label();
            this.ClassificationGroupBox = new System.Windows.Forms.GroupBox();
            this.ClusterClassLabel = new System.Windows.Forms.Label();
            this.BrowseClassifierButton = new System.Windows.Forms.Button();
            this.LoadClassifierButton = new System.Windows.Forms.Button();
            this.ClassifierLabel = new System.Windows.Forms.Label();
            this.ClassifierTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.ViewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).BeginInit();
            this.ShowDetailsGroupBox.SuspendLayout();
            this.ControlGroupBox.SuspendLayout();
            this.ClassificationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClusterHistogram
            // 
            chartArea1.Name = "ChartArea1";
            this.ClusterHistogram.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ClusterHistogram.Legends.Add(legend1);
            this.ClusterHistogram.Location = new System.Drawing.Point(615, 481);
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
            this.showHistogram.Enabled = false;
            this.showHistogram.Location = new System.Drawing.Point(623, 448);
            this.showHistogram.Name = "showHistogram";
            this.showHistogram.Size = new System.Drawing.Size(168, 25);
            this.showHistogram.TabIndex = 1;
            this.showHistogram.Text = "Show Collection Histogram";
            this.showHistogram.UseVisualStyleBackColor = true;
            this.showHistogram.Click += new System.EventHandler(this.ShowHistogramClicked);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Enabled = false;
            this.PreviousButton.Location = new System.Drawing.Point(5, 131);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(78, 32);
            this.PreviousButton.TabIndex = 2;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PrevButtonClicked);
            // 
            // NextButton
            // 
            this.NextButton.Enabled = false;
            this.NextButton.Location = new System.Drawing.Point(89, 131);
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
            this.PictureBox.Location = new System.Drawing.Point(7, 212);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(610, 653);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // HideHistogramButton
            // 
            this.HideHistogramButton.Enabled = false;
            this.HideHistogramButton.Location = new System.Drawing.Point(799, 448);
            this.HideHistogramButton.Margin = new System.Windows.Forms.Padding(2);
            this.HideHistogramButton.Name = "HideHistogramButton";
            this.HideHistogramButton.Size = new System.Drawing.Size(142, 25);
            this.HideHistogramButton.TabIndex = 7;
            this.HideHistogramButton.Text = "Hide Collection Histogram";
            this.HideHistogramButton.UseVisualStyleBackColor = true;
            this.HideHistogramButton.Click += new System.EventHandler(this.HideHistogramClicked);
            // 
            // ViewGroup
            // 
            this.ViewGroup.Controls.Add(this.BrowseViewButton);
            this.ViewGroup.Location = new System.Drawing.Point(9, 10);
            this.ViewGroup.Margin = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Name = "ViewGroup";
            this.ViewGroup.Padding = new System.Windows.Forms.Padding(2);
            this.ViewGroup.Size = new System.Drawing.Size(395, 197);
            this.ViewGroup.TabIndex = 10;
            this.ViewGroup.TabStop = false;
            this.ViewGroup.Text = "Select file to view";
            // 
            // BrowseViewButton
            // 
            this.BrowseViewButton.Location = new System.Drawing.Point(14, 85);
            this.BrowseViewButton.Margin = new System.Windows.Forms.Padding(2);
            this.BrowseViewButton.Name = "BrowseViewButton";
            this.BrowseViewButton.Size = new System.Drawing.Size(164, 51);
            this.BrowseViewButton.TabIndex = 13;
            this.BrowseViewButton.Text = "Browse...";
            this.BrowseViewButton.UseVisualStyleBackColor = true;
            this.BrowseViewButton.Click += new System.EventHandler(this.BrowseViewButtonClicked);
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
            this.skeletonizeButton.Enabled = false;
            this.skeletonizeButton.Location = new System.Drawing.Point(26, 85);
            this.skeletonizeButton.Name = "skeletonizeButton";
            this.skeletonizeButton.Size = new System.Drawing.Size(181, 46);
            this.skeletonizeButton.TabIndex = 11;
            this.skeletonizeButton.Text = "Skeletonize";
            this.skeletonizeButton.UseVisualStyleBackColor = true;
            this.skeletonizeButton.Click += new System.EventHandler(this.SkeletonizeButtonClicked);
            // 
            // showPixHistogramButton
            // 
            this.showPixHistogramButton.Enabled = false;
            this.showPixHistogramButton.Location = new System.Drawing.Point(623, 723);
            this.showPixHistogramButton.Name = "showPixHistogramButton";
            this.showPixHistogramButton.Size = new System.Drawing.Size(168, 25);
            this.showPixHistogramButton.TabIndex = 13;
            this.showPixHistogramButton.Text = "Show Pixel Histogram";
            this.showPixHistogramButton.UseVisualStyleBackColor = true;
            this.showPixHistogramButton.Click += new System.EventHandler(this.ShowPixHistogramClicked);
            // 
            // hidePixHistogramButton
            // 
            this.hidePixHistogramButton.Enabled = false;
            this.hidePixHistogramButton.Location = new System.Drawing.Point(797, 723);
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
            this.ClusterPixHistogram.Location = new System.Drawing.Point(623, 754);
            this.ClusterPixHistogram.Name = "ClusterPixHistogram";
            this.ClusterPixHistogram.RightToLeft = System.Windows.Forms.RightToLeft.No;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ClusterPixHistogram.Series.Add(series2);
            this.ClusterPixHistogram.Size = new System.Drawing.Size(632, 205);
            this.ClusterPixHistogram.TabIndex = 15;
            this.ClusterPixHistogram.Visible = false;
            // 
            // winChartViewer
            // 
            this.winChartViewer.Location = new System.Drawing.Point(947, 84);
            this.winChartViewer.Name = "winChartViewer";
            this.winChartViewer.Size = new System.Drawing.Size(415, 389);
            this.winChartViewer.TabIndex = 16;
            this.winChartViewer.TabStop = false;
            // 
            // View3DButton
            // 
            this.View3DButton.Enabled = false;
            this.View3DButton.Location = new System.Drawing.Point(947, 12);
            this.View3DButton.Name = "View3DButton";
            this.View3DButton.Size = new System.Drawing.Size(115, 41);
            this.View3DButton.TabIndex = 17;
            this.View3DButton.Text = "View 3D";
            this.View3DButton.UseVisualStyleBackColor = true;
            this.View3DButton.Click += new System.EventHandler(this.View3DClicked);
            // 
            // RotateUpButton
            // 
            this.RotateUpButton.Enabled = false;
            this.RotateUpButton.Location = new System.Drawing.Point(1118, 12);
            this.RotateUpButton.Name = "RotateUpButton";
            this.RotateUpButton.Size = new System.Drawing.Size(47, 22);
            this.RotateUpButton.TabIndex = 19;
            this.RotateUpButton.Text = "Up";
            this.RotateUpButton.UseVisualStyleBackColor = true;
            this.RotateUpButton.Click += new System.EventHandler(this.Rotate3DPlot);
            this.RotateUpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldDown);
            this.RotateUpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldUp);
            // 
            // RotateDownButton
            // 
            this.RotateDownButton.Enabled = false;
            this.RotateDownButton.Location = new System.Drawing.Point(1118, 53);
            this.RotateDownButton.Name = "RotateDownButton";
            this.RotateDownButton.Size = new System.Drawing.Size(47, 22);
            this.RotateDownButton.TabIndex = 20;
            this.RotateDownButton.Text = "Down";
            this.RotateDownButton.UseVisualStyleBackColor = true;
            this.RotateDownButton.Click += new System.EventHandler(this.Rotate3DPlot);
            this.RotateDownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldDown);
            this.RotateDownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldUp);
            // 
            // RotateLeftButton
            // 
            this.RotateLeftButton.Enabled = false;
            this.RotateLeftButton.Location = new System.Drawing.Point(1082, 30);
            this.RotateLeftButton.Name = "RotateLeftButton";
            this.RotateLeftButton.Size = new System.Drawing.Size(39, 24);
            this.RotateLeftButton.TabIndex = 21;
            this.RotateLeftButton.Text = "Left";
            this.RotateLeftButton.UseVisualStyleBackColor = true;
            this.RotateLeftButton.Click += new System.EventHandler(this.Rotate3DPlot);
            this.RotateLeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldDown);
            this.RotateLeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldUp);
            // 
            // RotateRightButton
            // 
            this.RotateRightButton.Enabled = false;
            this.RotateRightButton.Location = new System.Drawing.Point(1162, 32);
            this.RotateRightButton.Name = "RotateRightButton";
            this.RotateRightButton.Size = new System.Drawing.Size(43, 22);
            this.RotateRightButton.TabIndex = 22;
            this.RotateRightButton.Text = "Right";
            this.RotateRightButton.UseVisualStyleBackColor = true;
            this.RotateRightButton.Click += new System.EventHandler(this.Rotate3DPlot);
            this.RotateRightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldDown);
            this.RotateRightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rotate3DPlotHoldUp);
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
            this.ShowBranchesButton.Enabled = false;
            this.ShowBranchesButton.Location = new System.Drawing.Point(26, 150);
            this.ShowBranchesButton.Name = "ShowBranchesButton";
            this.ShowBranchesButton.Size = new System.Drawing.Size(181, 45);
            this.ShowBranchesButton.TabIndex = 24;
            this.ShowBranchesButton.Text = "Show Branches";
            this.ShowBranchesButton.UseVisualStyleBackColor = true;
            this.ShowBranchesButton.Click += new System.EventHandler(this.ViewBranchButtonClicked);
            // 
            // ShowDetailsGroupBox
            // 
            this.ShowDetailsGroupBox.Controls.Add(this.ShowAttributesButton);
            this.ShowDetailsGroupBox.Controls.Add(this.ShowBranchesButton);
            this.ShowDetailsGroupBox.Controls.Add(this.skeletonizeButton);
            this.ShowDetailsGroupBox.Location = new System.Drawing.Point(623, 12);
            this.ShowDetailsGroupBox.Name = "ShowDetailsGroupBox";
            this.ShowDetailsGroupBox.Size = new System.Drawing.Size(319, 239);
            this.ShowDetailsGroupBox.TabIndex = 25;
            this.ShowDetailsGroupBox.TabStop = false;
            this.ShowDetailsGroupBox.Text = "Show Details";
            // 
            // ShowAttributesButton
            // 
            this.ShowAttributesButton.Enabled = false;
            this.ShowAttributesButton.Location = new System.Drawing.Point(26, 24);
            this.ShowAttributesButton.Name = "ShowAttributesButton";
            this.ShowAttributesButton.Size = new System.Drawing.Size(181, 48);
            this.ShowAttributesButton.TabIndex = 25;
            this.ShowAttributesButton.Text = "Show Attributes";
            this.ShowAttributesButton.UseVisualStyleBackColor = true;
            this.ShowAttributesButton.Click += new System.EventHandler(this.ShowAttributesClicked);
            // 
            // ClassifyButton
            // 
            this.ClassifyButton.Enabled = false;
            this.ClassifyButton.Location = new System.Drawing.Point(164, 70);
            this.ClassifyButton.Name = "ClassifyButton";
            this.ClassifyButton.Size = new System.Drawing.Size(115, 48);
            this.ClassifyButton.TabIndex = 30;
            this.ClassifyButton.Text = "Classify";
            this.ClassifyButton.UseVisualStyleBackColor = true;
            this.ClassifyButton.Click += new System.EventHandler(this.ClassifyButtonClicked);
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
            this.FindByIndexTextBox.Location = new System.Drawing.Point(57, 71);
            this.FindByIndexTextBox.Name = "FindByIndexTextBox";
            this.FindByIndexTextBox.Size = new System.Drawing.Size(52, 20);
            this.FindByIndexTextBox.TabIndex = 22;
            // 
            // ClusterFindIndexLabel
            // 
            this.ClusterFindIndexLabel.AutoSize = true;
            this.ClusterFindIndexLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClusterFindIndexLabel.Location = new System.Drawing.Point(6, 72);
            this.ClusterFindIndexLabel.Name = "ClusterFindIndexLabel";
            this.ClusterFindIndexLabel.Size = new System.Drawing.Size(45, 17);
            this.ClusterFindIndexLabel.TabIndex = 21;
            this.ClusterFindIndexLabel.Text = "Index:";
            // 
            // FindByIndexButton
            // 
            this.FindByIndexButton.Enabled = false;
            this.FindByIndexButton.Location = new System.Drawing.Point(115, 69);
            this.FindByIndexButton.Name = "FindByIndexButton";
            this.FindByIndexButton.Size = new System.Drawing.Size(67, 23);
            this.FindByIndexButton.TabIndex = 20;
            this.FindByIndexButton.Text = "Find";
            this.FindByIndexButton.UseVisualStyleBackColor = true;
            this.FindByIndexButton.Click += new System.EventHandler(this.FindClusterByIndexClicked);
            // 
            // ZoomLabel
            // 
            this.ZoomLabel.AutoSize = true;
            this.ZoomLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ZoomLabel.Location = new System.Drawing.Point(451, 878);
            this.ZoomLabel.Name = "ZoomLabel";
            this.ZoomLabel.Size = new System.Drawing.Size(74, 20);
            this.ZoomLabel.TabIndex = 28;
            this.ZoomLabel.Text = "Zoom: 1x";
            // 
            // ClassificationGroupBox
            // 
            this.ClassificationGroupBox.Controls.Add(this.ClusterClassLabel);
            this.ClassificationGroupBox.Controls.Add(this.BrowseClassifierButton);
            this.ClassificationGroupBox.Controls.Add(this.LoadClassifierButton);
            this.ClassificationGroupBox.Controls.Add(this.ClassifierLabel);
            this.ClassificationGroupBox.Controls.Add(this.ClassifierTextBox);
            this.ClassificationGroupBox.Controls.Add(this.ClassifyButton);
            this.ClassificationGroupBox.Location = new System.Drawing.Point(623, 270);
            this.ClassificationGroupBox.Name = "ClassificationGroupBox";
            this.ClassificationGroupBox.Size = new System.Drawing.Size(318, 172);
            this.ClassificationGroupBox.TabIndex = 31;
            this.ClassificationGroupBox.TabStop = false;
            this.ClassificationGroupBox.Text = "Classification";
            // 
            // ClusterClassLabel
            // 
            this.ClusterClassLabel.AutoSize = true;
            this.ClusterClassLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ClusterClassLabel.Location = new System.Drawing.Point(23, 137);
            this.ClusterClassLabel.Name = "ClusterClassLabel";
            this.ClusterClassLabel.Size = new System.Drawing.Size(123, 18);
            this.ClusterClassLabel.TabIndex = 35;
            this.ClusterClassLabel.Text = "Calculated Class:";
            // 
            // BrowseClassifierButton
            // 
            this.BrowseClassifierButton.Enabled = false;
            this.BrowseClassifierButton.Location = new System.Drawing.Point(227, 25);
            this.BrowseClassifierButton.Name = "BrowseClassifierButton";
            this.BrowseClassifierButton.Size = new System.Drawing.Size(75, 22);
            this.BrowseClassifierButton.TabIndex = 34;
            this.BrowseClassifierButton.Text = "Browse";
            this.BrowseClassifierButton.UseVisualStyleBackColor = true;
            this.BrowseClassifierButton.Click += new System.EventHandler(this.BrowseClassifierClicked);
            // 
            // LoadClassifierButton
            // 
            this.LoadClassifierButton.Enabled = false;
            this.LoadClassifierButton.Location = new System.Drawing.Point(16, 70);
            this.LoadClassifierButton.Name = "LoadClassifierButton";
            this.LoadClassifierButton.Size = new System.Drawing.Size(134, 48);
            this.LoadClassifierButton.TabIndex = 33;
            this.LoadClassifierButton.Text = "Load Classifier";
            this.LoadClassifierButton.UseVisualStyleBackColor = true;
            this.LoadClassifierButton.Click += new System.EventHandler(this.LoadClassifierClicked);
            // 
            // ClassifierLabel
            // 
            this.ClassifierLabel.AutoSize = true;
            this.ClassifierLabel.Location = new System.Drawing.Point(13, 28);
            this.ClassifierLabel.Name = "ClassifierLabel";
            this.ClassifierLabel.Size = new System.Drawing.Size(86, 13);
            this.ClassifierLabel.TabIndex = 32;
            this.ClassifierLabel.Text = "Choose classifier";
            // 
            // ClassifierTextBox
            // 
            this.ClassifierTextBox.Location = new System.Drawing.Point(115, 25);
            this.ClassifierTextBox.Name = "ClassifierTextBox";
            this.ClassifierTextBox.Size = new System.Drawing.Size(100, 20);
            this.ClassifierTextBox.TabIndex = 31;
            // 
            // ClusterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 962);
            this.Controls.Add(this.ZoomLabel);
            this.Controls.Add(this.ClassificationGroupBox);
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
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ViewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClusterPixHistogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer)).EndInit();
            this.ShowDetailsGroupBox.ResumeLayout(false);
            this.ControlGroupBox.ResumeLayout(false);
            this.ControlGroupBox.PerformLayout();
            this.ClassificationGroupBox.ResumeLayout(false);
            this.ClassificationGroupBox.PerformLayout();
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
        private System.Windows.Forms.GroupBox ViewGroup;
        private System.Windows.Forms.Button BrowseViewButton;
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
        private System.Windows.Forms.GroupBox ShowDetailsGroupBox;
        private System.Windows.Forms.Button ShowAttributesButton;
        private System.Windows.Forms.Button ClassifyButton;
        private System.Windows.Forms.Label ClusterIndexValueLabel;
        private System.Windows.Forms.Label ClusterIndexLabel;
        private System.Windows.Forms.GroupBox ControlGroupBox;
        private System.Windows.Forms.TextBox FindByIndexTextBox;
        private System.Windows.Forms.Label ClusterFindIndexLabel;
        private System.Windows.Forms.Button FindByIndexButton;
        //private System.Windows.Forms.Label ClusterClassLabel;
        private System.Windows.Forms.Label ZoomLabel;
        private System.Windows.Forms.GroupBox ClassificationGroupBox;
        private System.Windows.Forms.Label ClassifierLabel;
        private System.Windows.Forms.TextBox ClassifierTextBox;
        private System.Windows.Forms.Button BrowseClassifierButton;
        private System.Windows.Forms.Button LoadClassifierButton;
        private System.Windows.Forms.Label ClusterClassLabel;
    }
}

