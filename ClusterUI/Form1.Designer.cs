namespace ClusterUI
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ClusterHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.showHistogram = new System.Windows.Forms.Button();
            this.PreviousButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.FilterButton = new System.Windows.Forms.Button();
            this.FilterGroup = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ToEnergyLabel = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.ClusterHistogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.FilterGroup.SuspendLayout();
            this.InputFileGroup.SuspendLayout();
            this.ViewGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClusterHistogram
            // 
            chartArea2.Name = "ChartArea1";
            this.ClusterHistogram.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ClusterHistogram.Legends.Add(legend2);
            this.ClusterHistogram.Location = new System.Drawing.Point(841, 500);
            this.ClusterHistogram.Margin = new System.Windows.Forms.Padding(4);
            this.ClusterHistogram.Name = "ClusterHistogram";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ClusterHistogram.Series.Add(series2);
            this.ClusterHistogram.Size = new System.Drawing.Size(1048, 336);
            this.ClusterHistogram.TabIndex = 0;
            this.ClusterHistogram.Text = "Histogram";
            // 
            // showHistogram
            // 
            this.showHistogram.Location = new System.Drawing.Point(841, 450);
            this.showHistogram.Margin = new System.Windows.Forms.Padding(4);
            this.showHistogram.Name = "showHistogram";
            this.showHistogram.Size = new System.Drawing.Size(224, 31);
            this.showHistogram.TabIndex = 1;
            this.showHistogram.Text = "Show Histogram";
            this.showHistogram.UseVisualStyleBackColor = true;
            this.showHistogram.Click += new System.EventHandler(this.ShowHistogramClicked);
            // 
            // PreviousButton
            // 
            this.PreviousButton.Location = new System.Drawing.Point(7, 155);
            this.PreviousButton.Margin = new System.Windows.Forms.Padding(4);
            this.PreviousButton.Name = "PreviousButton";
            this.PreviousButton.Size = new System.Drawing.Size(134, 40);
            this.PreviousButton.TabIndex = 2;
            this.PreviousButton.Text = "Previous";
            this.PreviousButton.UseVisualStyleBackColor = true;
            this.PreviousButton.Click += new System.EventHandler(this.PrevButtonClicked);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(158, 155);
            this.NextButton.Margin = new System.Windows.Forms.Padding(4);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(140, 40);
            this.NextButton.TabIndex = 3;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButtonClicked);
            // 
            // PictureBox
            // 
            this.PictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PictureBox.Location = new System.Drawing.Point(13, 220);
            this.PictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(800, 800);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // FilterButton
            // 
            this.FilterButton.Location = new System.Drawing.Point(841, 348);
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(138, 52);
            this.FilterButton.TabIndex = 5;
            this.FilterButton.Text = "Process";
            this.FilterButton.UseVisualStyleBackColor = true;
            this.FilterButton.Click += new System.EventHandler(this.ProcessFilterClicked);
            // 
            // FilterGroup
            // 
            this.FilterGroup.Controls.Add(this.label2);
            this.FilterGroup.Controls.Add(this.ToEnergyLabel);
            this.FilterGroup.Controls.Add(this.ToPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.labelToPixCount);
            this.FilterGroup.Controls.Add(this.labelFromPixCount);
            this.FilterGroup.Controls.Add(this.FromPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.labelToEnergyBox);
            this.FilterGroup.Controls.Add(this.label1);
            this.FilterGroup.Controls.Add(this.labelFromEnergyBox);
            this.FilterGroup.Controls.Add(this.ToEnergyFilterBox);
            this.FilterGroup.Controls.Add(this.FromEnergyFilterBox);
            this.FilterGroup.Location = new System.Drawing.Point(841, 178);
            this.FilterGroup.Name = "FilterGroup";
            this.FilterGroup.Size = new System.Drawing.Size(516, 152);
            this.FilterGroup.TabIndex = 6;
            this.FilterGroup.TabStop = false;
            this.FilterGroup.Text = "Select filters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(6, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Pixel Count";
            // 
            // ToEnergyLabel
            // 
            this.ToEnergyLabel.AutoSize = true;
            this.ToEnergyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ToEnergyLabel.Location = new System.Drawing.Point(6, 57);
            this.ToEnergyLabel.Name = "ToEnergyLabel";
            this.ToEnergyLabel.Size = new System.Drawing.Size(139, 20);
            this.ToEnergyLabel.TabIndex = 9;
            this.ToEnergyLabel.Text = "Energy (in keV)";
            // 
            // ToPixCountFilterBox
            // 
            this.ToPixCountFilterBox.Location = new System.Drawing.Point(365, 98);
            this.ToPixCountFilterBox.Name = "ToPixCountFilterBox";
            this.ToPixCountFilterBox.Size = new System.Drawing.Size(75, 22);
            this.ToPixCountFilterBox.TabIndex = 8;
            // 
            // labelToPixCount
            // 
            this.labelToPixCount.AutoSize = true;
            this.labelToPixCount.Location = new System.Drawing.Point(330, 103);
            this.labelToPixCount.Name = "labelToPixCount";
            this.labelToPixCount.Size = new System.Drawing.Size(29, 17);
            this.labelToPixCount.TabIndex = 7;
            this.labelToPixCount.Text = "To:";
            // 
            // labelFromPixCount
            // 
            this.labelFromPixCount.AutoSize = true;
            this.labelFromPixCount.Location = new System.Drawing.Point(161, 103);
            this.labelFromPixCount.Name = "labelFromPixCount";
            this.labelFromPixCount.Size = new System.Drawing.Size(44, 17);
            this.labelFromPixCount.TabIndex = 6;
            this.labelFromPixCount.Text = "From:";
            // 
            // FromPixCountFilterBox
            // 
            this.FromPixCountFilterBox.Location = new System.Drawing.Point(220, 98);
            this.FromPixCountFilterBox.Name = "FromPixCountFilterBox";
            this.FromPixCountFilterBox.Size = new System.Drawing.Size(75, 22);
            this.FromPixCountFilterBox.TabIndex = 5;
            // 
            // labelToEnergyBox
            // 
            this.labelToEnergyBox.AutoSize = true;
            this.labelToEnergyBox.Location = new System.Drawing.Point(329, 60);
            this.labelToEnergyBox.Name = "labelToEnergyBox";
            this.labelToEnergyBox.Size = new System.Drawing.Size(29, 17);
            this.labelToEnergyBox.TabIndex = 4;
            this.labelToEnergyBox.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 3;
            // 
            // labelFromEnergyBox
            // 
            this.labelFromEnergyBox.AutoSize = true;
            this.labelFromEnergyBox.Location = new System.Drawing.Point(161, 60);
            this.labelFromEnergyBox.Name = "labelFromEnergyBox";
            this.labelFromEnergyBox.Size = new System.Drawing.Size(44, 17);
            this.labelFromEnergyBox.TabIndex = 2;
            this.labelFromEnergyBox.Text = "From:";
            // 
            // ToEnergyFilterBox
            // 
            this.ToEnergyFilterBox.Location = new System.Drawing.Point(365, 57);
            this.ToEnergyFilterBox.Name = "ToEnergyFilterBox";
            this.ToEnergyFilterBox.Size = new System.Drawing.Size(75, 22);
            this.ToEnergyFilterBox.TabIndex = 1;
            // 
            // FromEnergyFilterBox
            // 
            this.FromEnergyFilterBox.Location = new System.Drawing.Point(220, 60);
            this.FromEnergyFilterBox.Name = "FromEnergyFilterBox";
            this.FromEnergyFilterBox.Size = new System.Drawing.Size(75, 22);
            this.FromEnergyFilterBox.TabIndex = 0;
            // 
            // HideHistogramButton
            // 
            this.HideHistogramButton.Location = new System.Drawing.Point(1091, 450);
            this.HideHistogramButton.Name = "HideHistogramButton";
            this.HideHistogramButton.Size = new System.Drawing.Size(205, 31);
            this.HideHistogramButton.TabIndex = 7;
            this.HideHistogramButton.Text = "Hide Histogram";
            this.HideHistogramButton.UseVisualStyleBackColor = true;
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
            this.InputFileGroup.Location = new System.Drawing.Point(833, 12);
            this.InputFileGroup.Name = "InputFileGroup";
            this.InputFileGroup.Size = new System.Drawing.Size(516, 160);
            this.InputFileGroup.TabIndex = 8;
            this.InputFileGroup.TabStop = false;
            this.InputFileGroup.Text = "Select input and output file for filters";
            // 
            // OutFileNameIniBox
            // 
            this.OutFileNameIniBox.Location = new System.Drawing.Point(137, 113);
            this.OutFileNameIniBox.Name = "OutFileNameIniBox";
            this.OutFileNameIniBox.Size = new System.Drawing.Size(196, 22);
            this.OutFileNameIniBox.TabIndex = 21;
            // 
            // OutIniFileLabel
            // 
            this.OutIniFileLabel.AutoSize = true;
            this.OutIniFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OutIniFileLabel.Location = new System.Drawing.Point(6, 116);
            this.OutIniFileLabel.Name = "OutIniFileLabel";
            this.OutIniFileLabel.Size = new System.Drawing.Size(124, 20);
            this.OutIniFileLabel.TabIndex = 20;
            this.OutIniFileLabel.Text = "Output (.ini) file";
            // 
            // OutFileNameClBox
            // 
            this.OutFileNameClBox.Location = new System.Drawing.Point(137, 73);
            this.OutFileNameClBox.Name = "OutFileNameClBox";
            this.OutFileNameClBox.Size = new System.Drawing.Size(196, 22);
            this.OutFileNameClBox.TabIndex = 19;
            // 
            // OutClFileLabel
            // 
            this.OutClFileLabel.AutoSize = true;
            this.OutClFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OutClFileLabel.Location = new System.Drawing.Point(6, 75);
            this.OutClFileLabel.Name = "OutClFileLabel";
            this.OutClFileLabel.Size = new System.Drawing.Size(120, 20);
            this.OutClFileLabel.TabIndex = 18;
            this.OutClFileLabel.Text = "Output (.cl) file";
            // 
            // BrowseFilteredFileButton
            // 
            this.BrowseFilteredFileButton.Location = new System.Drawing.Point(389, 32);
            this.BrowseFilteredFileButton.Name = "BrowseFilteredFileButton";
            this.BrowseFilteredFileButton.Size = new System.Drawing.Size(108, 30);
            this.BrowseFilteredFileButton.TabIndex = 17;
            this.BrowseFilteredFileButton.Text = "Browse...";
            this.BrowseFilteredFileButton.UseVisualStyleBackColor = true;
            this.BrowseFilteredFileButton.Click += new System.EventHandler(this.BrowseFilterFileButtonClicked);
            // 
            // Or2Label
            // 
            this.Or2Label.AutoSize = true;
            this.Or2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Or2Label.Location = new System.Drawing.Point(339, 34);
            this.Or2Label.Name = "Or2Label";
            this.Or2Label.Size = new System.Drawing.Size(28, 20);
            this.Or2Label.TabIndex = 16;
            this.Or2Label.Text = "Or";
            // 
            // InFilePathBox
            // 
            this.InFilePathBox.Location = new System.Drawing.Point(137, 32);
            this.InFilePathBox.Name = "InFilePathBox";
            this.InFilePathBox.Size = new System.Drawing.Size(196, 22);
            this.InFilePathBox.TabIndex = 15;
            // 
            // InputFileLabel
            // 
            this.InputFileLabel.AutoSize = true;
            this.InputFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.InputFileLabel.Location = new System.Drawing.Point(6, 30);
            this.InputFileLabel.Name = "InputFileLabel";
            this.InputFileLabel.Size = new System.Drawing.Size(109, 20);
            this.InputFileLabel.TabIndex = 14;
            this.InputFileLabel.Text = "Input file path";
            // 
            // ViewClusters
            // 
            this.ViewClusters.Location = new System.Drawing.Point(6, 87);
            this.ViewClusters.Name = "ViewClusters";
            this.ViewClusters.Size = new System.Drawing.Size(529, 40);
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
            this.ViewGroup.Location = new System.Drawing.Point(12, 12);
            this.ViewGroup.Name = "ViewGroup";
            this.ViewGroup.Size = new System.Drawing.Size(541, 202);
            this.ViewGroup.TabIndex = 10;
            this.ViewGroup.TabStop = false;
            this.ViewGroup.Text = "Select file to view";
            // 
            // BrowseViewButton
            // 
            this.BrowseViewButton.Location = new System.Drawing.Point(366, 50);
            this.BrowseViewButton.Name = "BrowseViewButton";
            this.BrowseViewButton.Size = new System.Drawing.Size(108, 25);
            this.BrowseViewButton.TabIndex = 13;
            this.BrowseViewButton.Text = "Browse...";
            this.BrowseViewButton.UseVisualStyleBackColor = true;
            this.BrowseViewButton.Click += new System.EventHandler(this.BrowseViewButtonClicked);
            // 
            // OrLabel
            // 
            this.OrLabel.AutoSize = true;
            this.OrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OrLabel.Location = new System.Drawing.Point(315, 52);
            this.OrLabel.Name = "OrLabel";
            this.OrLabel.Size = new System.Drawing.Size(28, 20);
            this.OrLabel.TabIndex = 12;
            this.OrLabel.Text = "Or";
            // 
            // InViewFilePathBox
            // 
            this.InViewFilePathBox.Location = new System.Drawing.Point(90, 50);
            this.InViewFilePathBox.Name = "InViewFilePathBox";
            this.InViewFilePathBox.Size = new System.Drawing.Size(208, 22);
            this.InViewFilePathBox.TabIndex = 11;
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FilePathLabel.Location = new System.Drawing.Point(6, 50);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(78, 20);
            this.FilePathLabel.TabIndex = 10;
            this.FilePathLabel.Text = "File path:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1902, 1033);
            this.Controls.Add(this.ViewGroup);
            this.Controls.Add(this.InputFileGroup);
            this.Controls.Add(this.HideHistogramButton);
            this.Controls.Add(this.FilterButton);
            this.Controls.Add(this.FilterGroup);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.showHistogram);
            this.Controls.Add(this.ClusterHistogram);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ToEnergyLabel;
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
    }
}

