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
            this.ConvexitySkeletFilterCheckBox = new System.Windows.Forms.CheckBox();
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
            this.labelFromEnergyBox = new System.Windows.Forms.Label();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilterGroup.SuspendLayout();
            this.InputFileGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilterButton
            // 
            this.FilterButton.Location = new System.Drawing.Point(327, 319);
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
            this.FilterGroup.Controls.Add(this.ConvexitySkeletFilterCheckBox);
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
            this.FilterGroup.Controls.Add(this.labelFromEnergyBox);
            this.FilterGroup.Controls.Add(this.ToEnergyFilterBox);
            this.FilterGroup.Controls.Add(this.FromEnergyFilterBox);
            this.FilterGroup.Location = new System.Drawing.Point(11, 143);
            this.FilterGroup.Margin = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Name = "FilterGroup";
            this.FilterGroup.Padding = new System.Windows.Forms.Padding(2);
            this.FilterGroup.Size = new System.Drawing.Size(387, 172);
            this.FilterGroup.TabIndex = 6;
            this.FilterGroup.TabStop = false;
            this.FilterGroup.Text = "Select filters";
            // 
            // ConvexitySkeletFilterCheckBox
            // 
            this.ConvexitySkeletFilterCheckBox.AutoSize = true;
            this.ConvexitySkeletFilterCheckBox.Location = new System.Drawing.Point(39, 128);
            this.ConvexitySkeletFilterCheckBox.Name = "ConvexitySkeletFilterCheckBox";
            this.ConvexitySkeletFilterCheckBox.Size = new System.Drawing.Size(172, 19);
            this.ConvexitySkeletFilterCheckBox.TabIndex = 17;
            this.ConvexitySkeletFilterCheckBox.Text = "Use skeletonized convexity";
            this.ConvexitySkeletFilterCheckBox.UseVisualStyleBackColor = true;
            // 
            // ToLinearityTextBox
            // 
            this.ToLinearityTextBox.Location = new System.Drawing.Point(274, 101);
            this.ToLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToLinearityTextBox.Name = "ToLinearityTextBox";
            this.ToLinearityTextBox.Size = new System.Drawing.Size(57, 21);
            this.ToLinearityTextBox.TabIndex = 16;
            // 
            // FromLinearityTextBox
            // 
            this.FromLinearityTextBox.Location = new System.Drawing.Point(158, 101);
            this.FromLinearityTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromLinearityTextBox.Name = "FromLinearityTextBox";
            this.FromLinearityTextBox.Size = new System.Drawing.Size(57, 21);
            this.FromLinearityTextBox.TabIndex = 15;
            // 
            // labelToLinearity
            // 
            this.labelToLinearity.AutoSize = true;
            this.labelToLinearity.Location = new System.Drawing.Point(236, 108);
            this.labelToLinearity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToLinearity.Name = "labelToLinearity";
            this.labelToLinearity.Size = new System.Drawing.Size(24, 15);
            this.labelToLinearity.TabIndex = 14;
            this.labelToLinearity.Text = "To:";
            // 
            // labelFromLinearity
            // 
            this.labelFromLinearity.AutoSize = true;
            this.labelFromLinearity.Location = new System.Drawing.Point(121, 108);
            this.labelFromLinearity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromLinearity.Name = "labelFromLinearity";
            this.labelFromLinearity.Size = new System.Drawing.Size(39, 15);
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
            this.ToPixCountFilterBox.Size = new System.Drawing.Size(57, 21);
            this.ToPixCountFilterBox.TabIndex = 8;
            // 
            // labelToPixCount
            // 
            this.labelToPixCount.AutoSize = true;
            this.labelToPixCount.Location = new System.Drawing.Point(236, 71);
            this.labelToPixCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToPixCount.Name = "labelToPixCount";
            this.labelToPixCount.Size = new System.Drawing.Size(24, 15);
            this.labelToPixCount.TabIndex = 7;
            this.labelToPixCount.Text = "To:";
            // 
            // labelFromPixCount
            // 
            this.labelFromPixCount.AutoSize = true;
            this.labelFromPixCount.Location = new System.Drawing.Point(121, 71);
            this.labelFromPixCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromPixCount.Name = "labelFromPixCount";
            this.labelFromPixCount.Size = new System.Drawing.Size(39, 15);
            this.labelFromPixCount.TabIndex = 6;
            this.labelFromPixCount.Text = "From:";
            // 
            // FromPixCountFilterBox
            // 
            this.FromPixCountFilterBox.Location = new System.Drawing.Point(158, 68);
            this.FromPixCountFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromPixCountFilterBox.Name = "FromPixCountFilterBox";
            this.FromPixCountFilterBox.Size = new System.Drawing.Size(57, 21);
            this.FromPixCountFilterBox.TabIndex = 5;
            // 
            // labelToEnergyBox
            // 
            this.labelToEnergyBox.AutoSize = true;
            this.labelToEnergyBox.Location = new System.Drawing.Point(236, 36);
            this.labelToEnergyBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelToEnergyBox.Name = "labelToEnergyBox";
            this.labelToEnergyBox.Size = new System.Drawing.Size(24, 15);
            this.labelToEnergyBox.TabIndex = 4;
            this.labelToEnergyBox.Text = "To:";
            // 
            // labelFromEnergyBox
            // 
            this.labelFromEnergyBox.AutoSize = true;
            this.labelFromEnergyBox.Location = new System.Drawing.Point(121, 35);
            this.labelFromEnergyBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFromEnergyBox.Name = "labelFromEnergyBox";
            this.labelFromEnergyBox.Size = new System.Drawing.Size(39, 15);
            this.labelFromEnergyBox.TabIndex = 2;
            this.labelFromEnergyBox.Text = "From:";
            // 
            // ToEnergyFilterBox
            // 
            this.ToEnergyFilterBox.Location = new System.Drawing.Point(274, 33);
            this.ToEnergyFilterBox.Margin = new System.Windows.Forms.Padding(2);
            this.ToEnergyFilterBox.Name = "ToEnergyFilterBox";
            this.ToEnergyFilterBox.Size = new System.Drawing.Size(57, 21);
            this.ToEnergyFilterBox.TabIndex = 1;
            // 
            // FromEnergyFilterBox
            // 
            this.FromEnergyFilterBox.Location = new System.Drawing.Point(158, 32);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.InputFileGroup);
            this.groupBox1.Controls.Add(this.FilterGroup);
            this.groupBox1.Controls.Add(this.FilterButton);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 368);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filterer";
            // 
            // FilterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 962);
            this.Controls.Add(this.groupBox1);
            this.Name = "FilterUI";
            this.Text = "ClusterFilter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FilterGroup.ResumeLayout(false);
            this.FilterGroup.PerformLayout();
            this.InputFileGroup.ResumeLayout(false);
            this.InputFileGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FilterButton;
        private System.Windows.Forms.GroupBox FilterGroup;
        private System.Windows.Forms.Label pixCountLabel;
        private System.Windows.Forms.Label energyLabel;
        private System.Windows.Forms.TextBox ToPixCountFilterBox;
        private System.Windows.Forms.Label labelToPixCount;
        private System.Windows.Forms.Label labelFromPixCount;
        private System.Windows.Forms.TextBox FromPixCountFilterBox;
        private System.Windows.Forms.Label labelToEnergyBox;
        private System.Windows.Forms.Label labelFromEnergyBox;
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
        private System.Windows.Forms.Label linearityLabel;
        private System.Windows.Forms.TextBox ToLinearityTextBox;
        private System.Windows.Forms.TextBox FromLinearityTextBox;
        private System.Windows.Forms.Label labelToLinearity;
        private System.Windows.Forms.Label labelFromLinearity;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ConvexitySkeletFilterCheckBox;
    }
}

