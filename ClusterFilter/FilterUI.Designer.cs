namespace ClusterFilter
{
    partial class FilterUI
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
            this.FilterButton = new System.Windows.Forms.Button();
            this.FilterGroup = new System.Windows.Forms.GroupBox();
            this.ToWidthTextBox = new System.Windows.Forms.TextBox();
            this.FromWidthTextBox = new System.Windows.Forms.TextBox();
            this.ToWidthCount = new System.Windows.Forms.Label();
            this.FromWidthLabel = new System.Windows.Forms.Label();
            this.ToBranchCountTextBox = new System.Windows.Forms.TextBox();
            this.FromBranchCountTextBox = new System.Windows.Forms.TextBox();
            this.ToBranchCountLabel = new System.Windows.Forms.Label();
            this.FromBranchCountLabel = new System.Windows.Forms.Label();
            this.ToMaxEnergyTextBox = new System.Windows.Forms.TextBox();
            this.FromMaxEnergyTextBox = new System.Windows.Forms.TextBox();
            this.ToMaxEnergyLabel = new System.Windows.Forms.Label();
            this.FromMaxEnergyLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.BranchCountLabel = new System.Windows.Forms.Label();
            this.MaxEnergyLabel = new System.Windows.Forms.Label();
            this.ConvexitySkeletFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.ToLinearityTextBox = new System.Windows.Forms.TextBox();
            this.FromLinearityTextBox = new System.Windows.Forms.TextBox();
            this.ToConvexityClabel = new System.Windows.Forms.Label();
            this.FromLinearityLabel = new System.Windows.Forms.Label();
            this.ConvexityLabel = new System.Windows.Forms.Label();
            this.PixCountLabel = new System.Windows.Forms.Label();
            this.EnergyLabel = new System.Windows.Forms.Label();
            this.ToPixCountFilterBox = new System.Windows.Forms.TextBox();
            this.ToPixCountLabel = new System.Windows.Forms.Label();
            this.FromPixCountLabel = new System.Windows.Forms.Label();
            this.FromPixCountFilterBox = new System.Windows.Forms.TextBox();
            this.ToEnergyLabel = new System.Windows.Forms.Label();
            this.FromEnergyLabel = new System.Windows.Forms.Label();
            this.ToEnergyFilterBox = new System.Windows.Forms.TextBox();
            this.FromEnergyFilterBox = new System.Windows.Forms.TextBox();
            this.InputFileGroup = new System.Windows.Forms.GroupBox();
            this.OutFileNameIniBox = new System.Windows.Forms.TextBox();
            this.OutIniFileLabel = new System.Windows.Forms.Label();
            this.BrowseFilteredFileButton = new System.Windows.Forms.Button();
            this.Or2Label = new System.Windows.Forms.Label();
            this.InFilePathBox = new System.Windows.Forms.TextBox();
            this.InputFileLabel = new System.Windows.Forms.Label();
            this.ViewClusters = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.MainGroupBox = new System.Windows.Forms.GroupBox();
            this.VertexCountLabel = new System.Windows.Forms.Label();
            this.ToVertexCountTextBox = new System.Windows.Forms.TextBox();
            this.ToVertexCountLabel = new System.Windows.Forms.Label();
            this.FromVertexCountLabel = new System.Windows.Forms.Label();
            this.FromVertexCountTextBox = new System.Windows.Forms.TextBox();
            this.FilterGroup.SuspendLayout();
            this.InputFileGroup.SuspendLayout();
            this.MainGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilterButton
            // 
            this.FilterButton.Location = new System.Drawing.Point(16, 425);
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
            this.FilterGroup.Controls.Add(this.ToVertexCountTextBox);
            this.FilterGroup.Controls.Add(this.ToVertexCountLabel);
            this.FilterGroup.Controls.Add(this.FromVertexCountLabel);
            this.FilterGroup.Controls.Add(this.FromVertexCountTextBox);
            this.FilterGroup.Controls.Add(this.VertexCountLabel);
            this.FilterGroup.Controls.Add(this.ToWidthTextBox);
            this.FilterGroup.Controls.Add(this.FromWidthTextBox);
            this.FilterGroup.Controls.Add(this.ToWidthCount);
            this.FilterGroup.Controls.Add(this.FromWidthLabel);
            this.FilterGroup.Controls.Add(this.ToBranchCountTextBox);
            this.FilterGroup.Controls.Add(this.FromBranchCountTextBox);
            this.FilterGroup.Controls.Add(this.ToBranchCountLabel);
            this.FilterGroup.Controls.Add(this.FromBranchCountLabel);
            this.FilterGroup.Controls.Add(this.ToMaxEnergyTextBox);
            this.FilterGroup.Controls.Add(this.FromMaxEnergyTextBox);
            this.FilterGroup.Controls.Add(this.ToMaxEnergyLabel);
            this.FilterGroup.Controls.Add(this.FromMaxEnergyLabel);
            this.FilterGroup.Controls.Add(this.WidthLabel);
            this.FilterGroup.Controls.Add(this.BranchCountLabel);
            this.FilterGroup.Controls.Add(this.MaxEnergyLabel);
            this.FilterGroup.Controls.Add(this.ConvexitySkeletFilterCheckBox);
            this.FilterGroup.Controls.Add(this.ToLinearityTextBox);
            this.FilterGroup.Controls.Add(this.FromLinearityTextBox);
            this.FilterGroup.Controls.Add(this.ToConvexityClabel);
            this.FilterGroup.Controls.Add(this.FromLinearityLabel);
            this.FilterGroup.Controls.Add(this.ConvexityLabel);
            this.FilterGroup.Controls.Add(this.PixCountLabel);
            this.FilterGroup.Controls.Add(this.EnergyLabel);
            this.FilterGroup.Controls.Add(this.ToPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.ToPixCountLabel);
            this.FilterGroup.Controls.Add(this.FromPixCountLabel);
            this.FilterGroup.Controls.Add(this.FromPixCountFilterBox);
            this.FilterGroup.Controls.Add(this.ToEnergyLabel);
            this.FilterGroup.Controls.Add(this.FromEnergyLabel);
            this.FilterGroup.Controls.Add(this.ToEnergyFilterBox);
            this.FilterGroup.Controls.Add(this.FromEnergyFilterBox);
            this.FilterGroup.Location = new System.Drawing.Point(11, 143);
            this.FilterGroup.Margin = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Name = "FilterGroup";
            this.FilterGroup.Padding = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Size = new System.Drawing.Size(387, 278);
            this.FilterGroup.TabIndex = 6;
            this.FilterGroup.TabStop = false;
            this.FilterGroup.Text = "Select filters";
            // 
            // ToWidthTextBox
            // 
            this.ToWidthTextBox.Location = new System.Drawing.Point(317, 213);
            this.ToWidthTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToWidthTextBox.Name = "ToWidthTextBox";
            this.ToWidthTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToWidthTextBox.TabIndex = 35;
            // 
            // FromWidthTextBox
            // 
            this.FromWidthTextBox.Location = new System.Drawing.Point(207, 217);
            this.FromWidthTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromWidthTextBox.Name = "FromWidthTextBox";
            this.FromWidthTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromWidthTextBox.TabIndex = 34;
            // 
            // ToWidthCount
            // 
            this.ToWidthCount.AutoSize = true;
            this.ToWidthCount.Location = new System.Drawing.Point(278, 220);
            this.ToWidthCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToWidthCount.Name = "ToWidthCount";
            this.ToWidthCount.Size = new System.Drawing.Size(24, 15);
            this.ToWidthCount.TabIndex = 33;
            this.ToWidthCount.Text = "To:";
            // 
            // FromWidthLabel
            // 
            this.FromWidthLabel.AutoSize = true;
            this.FromWidthLabel.Location = new System.Drawing.Point(163, 220);
            this.FromWidthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromWidthLabel.Name = "FromWidthLabel";
            this.FromWidthLabel.Size = new System.Drawing.Size(39, 15);
            this.FromWidthLabel.TabIndex = 32;
            this.FromWidthLabel.Text = "From:";
            // 
            // ToBranchCountTextBox
            // 
            this.ToBranchCountTextBox.Location = new System.Drawing.Point(317, 247);
            this.ToBranchCountTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToBranchCountTextBox.Name = "ToBranchCountTextBox";
            this.ToBranchCountTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToBranchCountTextBox.TabIndex = 31;
            // 
            // FromBranchCountTextBox
            // 
            this.FromBranchCountTextBox.Location = new System.Drawing.Point(207, 247);
            this.FromBranchCountTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromBranchCountTextBox.Name = "FromBranchCountTextBox";
            this.FromBranchCountTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromBranchCountTextBox.TabIndex = 30;
            // 
            // ToBranchCountLabel
            // 
            this.ToBranchCountLabel.AutoSize = true;
            this.ToBranchCountLabel.Location = new System.Drawing.Point(275, 251);
            this.ToBranchCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToBranchCountLabel.Name = "ToBranchCountLabel";
            this.ToBranchCountLabel.Size = new System.Drawing.Size(24, 15);
            this.ToBranchCountLabel.TabIndex = 29;
            this.ToBranchCountLabel.Text = "To:";
            // 
            // FromBranchCountLabel
            // 
            this.FromBranchCountLabel.AutoSize = true;
            this.FromBranchCountLabel.Location = new System.Drawing.Point(160, 251);
            this.FromBranchCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromBranchCountLabel.Name = "FromBranchCountLabel";
            this.FromBranchCountLabel.Size = new System.Drawing.Size(39, 15);
            this.FromBranchCountLabel.TabIndex = 28;
            this.FromBranchCountLabel.Text = "From:";
            // 
            // ToMaxEnergyTextBox
            // 
            this.ToMaxEnergyTextBox.Location = new System.Drawing.Point(317, 65);
            this.ToMaxEnergyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToMaxEnergyTextBox.Name = "ToMaxEnergyTextBox";
            this.ToMaxEnergyTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToMaxEnergyTextBox.TabIndex = 27;
            // 
            // FromMaxEnergyTextBox
            // 
            this.FromMaxEnergyTextBox.Location = new System.Drawing.Point(207, 68);
            this.FromMaxEnergyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromMaxEnergyTextBox.Name = "FromMaxEnergyTextBox";
            this.FromMaxEnergyTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromMaxEnergyTextBox.TabIndex = 26;
            // 
            // ToMaxEnergyLabel
            // 
            this.ToMaxEnergyLabel.AutoSize = true;
            this.ToMaxEnergyLabel.Location = new System.Drawing.Point(275, 71);
            this.ToMaxEnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToMaxEnergyLabel.Name = "ToMaxEnergyLabel";
            this.ToMaxEnergyLabel.Size = new System.Drawing.Size(24, 15);
            this.ToMaxEnergyLabel.TabIndex = 25;
            this.ToMaxEnergyLabel.Text = "To:";
            // 
            // FromMaxEnergyLabel
            // 
            this.FromMaxEnergyLabel.AutoSize = true;
            this.FromMaxEnergyLabel.Location = new System.Drawing.Point(161, 71);
            this.FromMaxEnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromMaxEnergyLabel.Name = "FromMaxEnergyLabel";
            this.FromMaxEnergyLabel.Size = new System.Drawing.Size(39, 15);
            this.FromMaxEnergyLabel.TabIndex = 24;
            this.FromMaxEnergyLabel.Text = "From:";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.WidthLabel.Location = new System.Drawing.Point(8, 217);
            this.WidthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(49, 17);
            this.WidthLabel.TabIndex = 20;
            this.WidthLabel.Text = "Width";
            // 
            // BranchCountLabel
            // 
            this.BranchCountLabel.AutoSize = true;
            this.BranchCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BranchCountLabel.Location = new System.Drawing.Point(8, 247);
            this.BranchCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BranchCountLabel.Name = "BranchCountLabel";
            this.BranchCountLabel.Size = new System.Drawing.Size(101, 17);
            this.BranchCountLabel.TabIndex = 19;
            this.BranchCountLabel.Text = "BranchCount";
            // 
            // MaxEnergyLabel
            // 
            this.MaxEnergyLabel.AutoSize = true;
            this.MaxEnergyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MaxEnergyLabel.Location = new System.Drawing.Point(7, 70);
            this.MaxEnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MaxEnergyLabel.Name = "MaxEnergyLabel";
            this.MaxEnergyLabel.Size = new System.Drawing.Size(87, 17);
            this.MaxEnergyLabel.TabIndex = 18;
            this.MaxEnergyLabel.Text = "MaxEnergy";
            // 
            // ConvexitySkeletFilterCheckBox
            // 
            this.ConvexitySkeletFilterCheckBox.AutoSize = true;
            this.ConvexitySkeletFilterCheckBox.Location = new System.Drawing.Point(92, 196);
            this.ConvexitySkeletFilterCheckBox.Name = "ConvexitySkeletFilterCheckBox";
            this.ConvexitySkeletFilterCheckBox.Size = new System.Drawing.Size(172, 19);
            this.ConvexitySkeletFilterCheckBox.TabIndex = 17;
            this.ConvexitySkeletFilterCheckBox.Text = "Use skeletonized convexity";
            this.ConvexitySkeletFilterCheckBox.UseVisualStyleBackColor = true;
            // 
            // ToLinearityTextBox
            // 
            this.ToLinearityTextBox.Location = new System.Drawing.Point(317, 175);
            this.ToLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToLinearityTextBox.Name = "ToLinearityTextBox";
            this.ToLinearityTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToLinearityTextBox.TabIndex = 16;
            // 
            // FromLinearityTextBox
            // 
            this.FromLinearityTextBox.Location = new System.Drawing.Point(207, 175);
            this.FromLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromLinearityTextBox.Name = "FromLinearityTextBox";
            this.FromLinearityTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromLinearityTextBox.TabIndex = 15;
            // 
            // ToConvexityClabel
            // 
            this.ToConvexityClabel.AutoSize = true;
            this.ToConvexityClabel.Location = new System.Drawing.Point(276, 181);
            this.ToConvexityClabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToConvexityClabel.Name = "ToConvexityClabel";
            this.ToConvexityClabel.Size = new System.Drawing.Size(24, 15);
            this.ToConvexityClabel.TabIndex = 14;
            this.ToConvexityClabel.Text = "To:";
            // 
            // FromLinearityLabel
            // 
            this.FromLinearityLabel.AutoSize = true;
            this.FromLinearityLabel.Location = new System.Drawing.Point(160, 183);
            this.FromLinearityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromLinearityLabel.Name = "FromLinearityLabel";
            this.FromLinearityLabel.Size = new System.Drawing.Size(39, 15);
            this.FromLinearityLabel.TabIndex = 13;
            this.FromLinearityLabel.Text = "From:";
            // 
            // ConvexityLabel
            // 
            this.ConvexityLabel.AutoSize = true;
            this.ConvexityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ConvexityLabel.Location = new System.Drawing.Point(8, 177);
            this.ConvexityLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ConvexityLabel.Name = "ConvexityLabel";
            this.ConvexityLabel.Size = new System.Drawing.Size(89, 17);
            this.ConvexityLabel.TabIndex = 12;
            this.ConvexityLabel.Text = "Convexity()";
            // 
            // PixCountLabel
            // 
            this.PixCountLabel.AutoSize = true;
            this.PixCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PixCountLabel.Location = new System.Drawing.Point(7, 103);
            this.PixCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PixCountLabel.Name = "PixCountLabel";
            this.PixCountLabel.Size = new System.Drawing.Size(84, 17);
            this.PixCountLabel.TabIndex = 10;
            this.PixCountLabel.Text = "PixelCount";
            // 
            // EnergyLabel
            // 
            this.EnergyLabel.AutoSize = true;
            this.EnergyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.EnergyLabel.Location = new System.Drawing.Point(7, 35);
            this.EnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.EnergyLabel.Name = "EnergyLabel";
            this.EnergyLabel.Size = new System.Drawing.Size(96, 17);
            this.EnergyLabel.TabIndex = 9;
            this.EnergyLabel.Text = "TotalEnergy";
            // 
            // ToPixCountFilterBox
            // 
            this.ToPixCountFilterBox.Location = new System.Drawing.Point(317, 102);
            this.ToPixCountFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToPixCountFilterBox.Name = "ToPixCountFilterBox";
            this.ToPixCountFilterBox.Size = new System.Drawing.Size(57, 21);
            this.ToPixCountFilterBox.TabIndex = 8;
            // 
            // ToPixCountLabel
            // 
            this.ToPixCountLabel.AutoSize = true;
            this.ToPixCountLabel.Location = new System.Drawing.Point(274, 105);
            this.ToPixCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToPixCountLabel.Name = "ToPixCountLabel";
            this.ToPixCountLabel.Size = new System.Drawing.Size(24, 15);
            this.ToPixCountLabel.TabIndex = 7;
            this.ToPixCountLabel.Text = "To:";
            // 
            // FromPixCountLabel
            // 
            this.FromPixCountLabel.AutoSize = true;
            this.FromPixCountLabel.Location = new System.Drawing.Point(159, 104);
            this.FromPixCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromPixCountLabel.Name = "FromPixCountLabel";
            this.FromPixCountLabel.Size = new System.Drawing.Size(39, 15);
            this.FromPixCountLabel.TabIndex = 6;
            this.FromPixCountLabel.Text = "From:";
            // 
            // FromPixCountFilterBox
            // 
            this.FromPixCountFilterBox.Location = new System.Drawing.Point(207, 101);
            this.FromPixCountFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromPixCountFilterBox.Name = "FromPixCountFilterBox";
            this.FromPixCountFilterBox.Size = new System.Drawing.Size(57, 21);
            this.FromPixCountFilterBox.TabIndex = 5;
            // 
            // ToEnergyLabel
            // 
            this.ToEnergyLabel.AutoSize = true;
            this.ToEnergyLabel.Location = new System.Drawing.Point(278, 38);
            this.ToEnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToEnergyLabel.Name = "ToEnergyLabel";
            this.ToEnergyLabel.Size = new System.Drawing.Size(24, 15);
            this.ToEnergyLabel.TabIndex = 4;
            this.ToEnergyLabel.Text = "To:";
            // 
            // FromEnergyLabel
            // 
            this.FromEnergyLabel.AutoSize = true;
            this.FromEnergyLabel.Location = new System.Drawing.Point(163, 37);
            this.FromEnergyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromEnergyLabel.Name = "FromEnergyLabel";
            this.FromEnergyLabel.Size = new System.Drawing.Size(39, 15);
            this.FromEnergyLabel.TabIndex = 2;
            this.FromEnergyLabel.Text = "From:";
            // 
            // ToEnergyFilterBox
            // 
            this.ToEnergyFilterBox.Location = new System.Drawing.Point(317, 31);
            this.ToEnergyFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToEnergyFilterBox.Name = "ToEnergyFilterBox";
            this.ToEnergyFilterBox.Size = new System.Drawing.Size(57, 21);
            this.ToEnergyFilterBox.TabIndex = 1;
            // 
            // FromEnergyFilterBox
            // 
            this.FromEnergyFilterBox.Location = new System.Drawing.Point(207, 35);
            this.FromEnergyFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromEnergyFilterBox.Name = "FromEnergyFilterBox";
            this.FromEnergyFilterBox.Size = new System.Drawing.Size(57, 21);
            this.FromEnergyFilterBox.TabIndex = 0;
            // 
            // InputFileGroup
            // 
            this.InputFileGroup.Controls.Add(this.OutFileNameIniBox);
            this.InputFileGroup.Controls.Add(this.OutIniFileLabel);
            this.InputFileGroup.Controls.Add(this.BrowseFilteredFileButton);
            this.InputFileGroup.Controls.Add(this.Or2Label);
            this.InputFileGroup.Controls.Add(this.InFilePathBox);
            this.InputFileGroup.Controls.Add(this.InputFileLabel);
            this.InputFileGroup.Location = new System.Drawing.Point(11, 32);
            this.InputFileGroup.Margin = new System.Windows.Forms.Padding(2);
            this.InputFileGroup.Name = "InputFileGroup";
            this.InputFileGroup.Padding = new System.Windows.Forms.Padding(2);
            this.InputFileGroup.Size = new System.Drawing.Size(387, 107);
            this.InputFileGroup.TabIndex = 8;
            this.InputFileGroup.TabStop = false;
            this.InputFileGroup.Text = "Select input and output file for filters";
            // 
            // OutFileNameIniBox
            // 
            this.OutFileNameIniBox.Location = new System.Drawing.Point(112, 69);
            this.OutFileNameIniBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutFileNameIniBox.Name = "OutFileNameIniBox";
            this.OutFileNameIniBox.Size = new System.Drawing.Size(148, 21);
            this.OutFileNameIniBox.TabIndex = 21;
            // 
            // OutIniFileLabel
            // 
            this.OutIniFileLabel.AutoSize = true;
            this.OutIniFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OutIniFileLabel.Location = new System.Drawing.Point(4, 73);
            this.OutIniFileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.OutIniFileLabel.Name = "OutIniFileLabel";
            this.OutIniFileLabel.Size = new System.Drawing.Size(105, 17);
            this.OutIniFileLabel.TabIndex = 20;
            this.OutIniFileLabel.Text = "Output (.ini) file";
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
            this.Or2Label.Location = new System.Drawing.Point(264, 28);
            this.Or2Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Or2Label.Name = "Or2Label";
            this.Or2Label.Size = new System.Drawing.Size(24, 17);
            this.Or2Label.TabIndex = 16;
            this.Or2Label.Text = "Or";
            // 
            // InFilePathBox
            // 
            this.InFilePathBox.Location = new System.Drawing.Point(112, 26);
            this.InFilePathBox.Margin = new System.Windows.Forms.Padding(2);
            this.InFilePathBox.Name = "InFilePathBox";
            this.InFilePathBox.Size = new System.Drawing.Size(148, 21);
            this.InFilePathBox.TabIndex = 15;
            // 
            // InputFileLabel
            // 
            this.InputFileLabel.AutoSize = true;
            this.InputFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.InputFileLabel.Location = new System.Drawing.Point(6, 26);
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
            // 
            // MainGroupBox
            // 
            this.MainGroupBox.Controls.Add(this.InputFileGroup);
            this.MainGroupBox.Controls.Add(this.FilterGroup);
            this.MainGroupBox.Controls.Add(this.FilterButton);
            this.MainGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MainGroupBox.Location = new System.Drawing.Point(12, 12);
            this.MainGroupBox.Name = "MainGroupBox";
            this.MainGroupBox.Size = new System.Drawing.Size(555, 467);
            this.MainGroupBox.TabIndex = 18;
            this.MainGroupBox.TabStop = false;
            this.MainGroupBox.Text = "Filterer";
            // 
            // VertexCountLabel
            // 
            this.VertexCountLabel.AutoSize = true;
            this.VertexCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.VertexCountLabel.Location = new System.Drawing.Point(8, 145);
            this.VertexCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VertexCountLabel.Name = "VertexCountLabel";
            this.VertexCountLabel.Size = new System.Drawing.Size(96, 17);
            this.VertexCountLabel.TabIndex = 36;
            this.VertexCountLabel.Text = "VertexCount";
            // 
            // ToVertexCountTextBox
            // 
            this.ToVertexCountTextBox.Location = new System.Drawing.Point(317, 143);
            this.ToVertexCountTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToVertexCountTextBox.Name = "ToVertexCountTextBox";
            this.ToVertexCountTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToVertexCountTextBox.TabIndex = 40;
            // 
            // ToVertexCountLabel
            // 
            this.ToVertexCountLabel.AutoSize = true;
            this.ToVertexCountLabel.Location = new System.Drawing.Point(278, 145);
            this.ToVertexCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToVertexCountLabel.Name = "ToVertexCountLabel";
            this.ToVertexCountLabel.Size = new System.Drawing.Size(24, 15);
            this.ToVertexCountLabel.TabIndex = 39;
            this.ToVertexCountLabel.Text = "To:";
            // 
            // FromVertexCountLabel
            // 
            this.FromVertexCountLabel.AutoSize = true;
            this.FromVertexCountLabel.Location = new System.Drawing.Point(163, 144);
            this.FromVertexCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FromVertexCountLabel.Name = "FromVertexCountLabel";
            this.FromVertexCountLabel.Size = new System.Drawing.Size(39, 15);
            this.FromVertexCountLabel.TabIndex = 38;
            this.FromVertexCountLabel.Text = "From:";
            // 
            // FromVertexCountTextBox
            // 
            this.FromVertexCountTextBox.Location = new System.Drawing.Point(207, 141);
            this.FromVertexCountTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromVertexCountTextBox.Name = "FromVertexCountTextBox";
            this.FromVertexCountTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromVertexCountTextBox.TabIndex = 37;
            // 
            // FilterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 962);
            this.Controls.Add(this.MainGroupBox);
            this.Name = "FilterUI";
            this.Text = "ClusterFilter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FilterGroup.ResumeLayout(false);
            this.FilterGroup.PerformLayout();
            this.InputFileGroup.ResumeLayout(false);
            this.InputFileGroup.PerformLayout();
            this.MainGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FilterButton;
        private System.Windows.Forms.GroupBox FilterGroup;
        private System.Windows.Forms.Label PixCountLabel;
        private System.Windows.Forms.Label EnergyLabel;
        private System.Windows.Forms.TextBox ToPixCountFilterBox;
        private System.Windows.Forms.Label ToPixCountLabel;
        private System.Windows.Forms.Label FromPixCountLabel;
        private System.Windows.Forms.TextBox FromPixCountFilterBox;
        private System.Windows.Forms.Label ToEnergyLabel;
        private System.Windows.Forms.Label FromEnergyLabel;
        private System.Windows.Forms.TextBox ToEnergyFilterBox;
        private System.Windows.Forms.TextBox FromEnergyFilterBox;
        private System.Windows.Forms.GroupBox InputFileGroup;
        private System.Windows.Forms.Button BrowseFilteredFileButton;
        private System.Windows.Forms.Label Or2Label;
        private System.Windows.Forms.TextBox InFilePathBox;
        private System.Windows.Forms.Label InputFileLabel;
        private System.Windows.Forms.Button ViewClusters;
        private System.Windows.Forms.Label OutIniFileLabel;
        private System.Windows.Forms.TextBox OutFileNameIniBox;
        private System.Windows.Forms.Label ConvexityLabel;
        private System.Windows.Forms.TextBox ToLinearityTextBox;
        private System.Windows.Forms.TextBox FromLinearityTextBox;
        private System.Windows.Forms.Label ToConvexityClabel;
        private System.Windows.Forms.Label FromLinearityLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox MainGroupBox;
        private System.Windows.Forms.CheckBox ConvexitySkeletFilterCheckBox;
        private System.Windows.Forms.TextBox ToWidthTextBox;
        private System.Windows.Forms.TextBox FromWidthTextBox;
        private System.Windows.Forms.Label ToWidthCount;
        private System.Windows.Forms.Label FromWidthLabel;
        private System.Windows.Forms.TextBox ToBranchCountTextBox;
        private System.Windows.Forms.TextBox FromBranchCountTextBox;
        private System.Windows.Forms.Label ToBranchCountLabel;
        private System.Windows.Forms.Label FromBranchCountLabel;
        private System.Windows.Forms.TextBox ToMaxEnergyTextBox;
        private System.Windows.Forms.TextBox FromMaxEnergyTextBox;
        private System.Windows.Forms.Label ToMaxEnergyLabel;
        private System.Windows.Forms.Label FromMaxEnergyLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label BranchCountLabel;
        private System.Windows.Forms.Label MaxEnergyLabel;
        private System.Windows.Forms.TextBox ToVertexCountTextBox;
        private System.Windows.Forms.Label ToVertexCountLabel;
        private System.Windows.Forms.Label FromVertexCountLabel;
        private System.Windows.Forms.TextBox FromVertexCountTextBox;
        private System.Windows.Forms.Label VertexCountLabel;
    }
}

