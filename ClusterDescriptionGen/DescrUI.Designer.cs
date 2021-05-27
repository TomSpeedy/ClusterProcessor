namespace ClusterDescriptionGen
{
    partial class DescrUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.StopProcessingButton = new System.Windows.Forms.Button();
            this.UsedPartitionDataRatioLabel = new System.Windows.Forms.Label();
            this.UsedPartitionDataRatioTextBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ProcessedCountLabel = new System.Windows.Forms.Label();
            this.EndingConditionGroupBox = new System.Windows.Forms.GroupBox();
            this.FirstPartitionEndsRadioButton = new System.Windows.Forms.RadioButton();
            this.LastClassEndsRadioButton = new System.Windows.Forms.RadioButton();
            this.FirstClassEndsRadioButton = new System.Windows.Forms.RadioButton();
            this.SelectAllignLabel = new System.Windows.Forms.Label();
            this.DistributionGroupBox = new System.Windows.Forms.GroupBox();
            this.UnevenDistrRadioButton = new System.Windows.Forms.RadioButton();
            this.EvenDistrRadioButton = new System.Windows.Forms.RadioButton();
            this.ClassPartDistrGroupBox = new System.Windows.Forms.GroupBox();
            this.ParallelClasPartProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.SerialClassPartProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.AllignClassTextBox = new System.Windows.Forms.TextBox();
            this.SelectedInputListView = new System.Windows.Forms.ListView();
            this.SelectedFilesClassName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SelectedFilesColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SelectInputListLabel = new System.Windows.Forms.Label();
            this.AttributeCheckedList = new System.Windows.Forms.CheckedListBox();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.OutputTextbox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InputLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.EndingConditionGroupBox.SuspendLayout();
            this.DistributionGroupBox.SuspendLayout();
            this.ClassPartDistrGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StopProcessingButton);
            this.groupBox1.Controls.Add(this.UsedPartitionDataRatioLabel);
            this.groupBox1.Controls.Add(this.UsedPartitionDataRatioTextBox);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.ProcessedCountLabel);
            this.groupBox1.Controls.Add(this.EndingConditionGroupBox);
            this.groupBox1.Controls.Add(this.SelectAllignLabel);
            this.groupBox1.Controls.Add(this.DistributionGroupBox);
            this.groupBox1.Controls.Add(this.ClassPartDistrGroupBox);
            this.groupBox1.Controls.Add(this.AllignClassTextBox);
            this.groupBox1.Controls.Add(this.SelectedInputListView);
            this.groupBox1.Controls.Add(this.SelectInputListLabel);
            this.groupBox1.Controls.Add(this.AttributeCheckedList);
            this.groupBox1.Controls.Add(this.ProcessButton);
            this.groupBox1.Controls.Add(this.OutputTextbox);
            this.groupBox1.Controls.Add(this.BrowseButton);
            this.groupBox1.Controls.Add(this.OutputLabel);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.InputLabel);
            this.groupBox1.Location = new System.Drawing.Point(29, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1358, 501);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Menu";
            // 
            // StopProcessingButton
            // 
            this.StopProcessingButton.Location = new System.Drawing.Point(610, 394);
            this.StopProcessingButton.Name = "StopProcessingButton";
            this.StopProcessingButton.Size = new System.Drawing.Size(133, 50);
            this.StopProcessingButton.TabIndex = 22;
            this.StopProcessingButton.Text = "Stop Processing";
            this.StopProcessingButton.UseVisualStyleBackColor = true;
            this.StopProcessingButton.Click += new System.EventHandler(this.StopProcessingClicked);
            // 
            // UsedPartitionDataRatioLabel
            // 
            this.UsedPartitionDataRatioLabel.AutoSize = true;
            this.UsedPartitionDataRatioLabel.Location = new System.Drawing.Point(1195, 35);
            this.UsedPartitionDataRatioLabel.Name = "UsedPartitionDataRatioLabel";
            this.UsedPartitionDataRatioLabel.Size = new System.Drawing.Size(99, 13);
            this.UsedPartitionDataRatioLabel.TabIndex = 21;
            this.UsedPartitionDataRatioLabel.Text = "Used Partition Data";
            // 
            // UsedPartitionDataRatioTextBox
            // 
            this.UsedPartitionDataRatioTextBox.Location = new System.Drawing.Point(1198, 59);
            this.UsedPartitionDataRatioTextBox.Name = "UsedPartitionDataRatioTextBox";
            this.UsedPartitionDataRatioTextBox.Size = new System.Drawing.Size(100, 20);
            this.UsedPartitionDataRatioTextBox.TabIndex = 20;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 407);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Check All Attributes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.CheckAllBoxesClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(461, 393);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 51);
            this.button1.TabIndex = 18;
            this.button1.Text = "Remove Selected";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.RemoveSelectedButtonClicked);
            // 
            // ProcessedCountLabel
            // 
            this.ProcessedCountLabel.AutoSize = true;
            this.ProcessedCountLabel.Location = new System.Drawing.Point(294, 413);
            this.ProcessedCountLabel.Name = "ProcessedCountLabel";
            this.ProcessedCountLabel.Size = new System.Drawing.Size(0, 13);
            this.ProcessedCountLabel.TabIndex = 17;
            // 
            // EndingConditionGroupBox
            // 
            this.EndingConditionGroupBox.Controls.Add(this.FirstPartitionEndsRadioButton);
            this.EndingConditionGroupBox.Controls.Add(this.LastClassEndsRadioButton);
            this.EndingConditionGroupBox.Controls.Add(this.FirstClassEndsRadioButton);
            this.EndingConditionGroupBox.Location = new System.Drawing.Point(796, 35);
            this.EndingConditionGroupBox.Name = "EndingConditionGroupBox";
            this.EndingConditionGroupBox.Size = new System.Drawing.Size(200, 100);
            this.EndingConditionGroupBox.TabIndex = 16;
            this.EndingConditionGroupBox.TabStop = false;
            this.EndingConditionGroupBox.Text = "Choose ending condition";
            // 
            // FirstPartitionEndsRadioButton
            // 
            this.FirstPartitionEndsRadioButton.AutoSize = true;
            this.FirstPartitionEndsRadioButton.Location = new System.Drawing.Point(25, 71);
            this.FirstPartitionEndsRadioButton.Name = "FirstPartitionEndsRadioButton";
            this.FirstPartitionEndsRadioButton.Size = new System.Drawing.Size(144, 17);
            this.FirstPartitionEndsRadioButton.TabIndex = 2;
            this.FirstPartitionEndsRadioButton.Text = "On first partition depletion";
            this.FirstPartitionEndsRadioButton.UseVisualStyleBackColor = true;
            // 
            // LastClassEndsRadioButton
            // 
            this.LastClassEndsRadioButton.AutoSize = true;
            this.LastClassEndsRadioButton.Location = new System.Drawing.Point(25, 47);
            this.LastClassEndsRadioButton.Name = "LastClassEndsRadioButton";
            this.LastClassEndsRadioButton.Size = new System.Drawing.Size(131, 17);
            this.LastClassEndsRadioButton.TabIndex = 1;
            this.LastClassEndsRadioButton.Text = "On last class depletion";
            this.LastClassEndsRadioButton.UseVisualStyleBackColor = true;
            // 
            // FirstClassEndsRadioButton
            // 
            this.FirstClassEndsRadioButton.AutoSize = true;
            this.FirstClassEndsRadioButton.Checked = true;
            this.FirstClassEndsRadioButton.Location = new System.Drawing.Point(25, 23);
            this.FirstClassEndsRadioButton.Name = "FirstClassEndsRadioButton";
            this.FirstClassEndsRadioButton.Size = new System.Drawing.Size(131, 17);
            this.FirstClassEndsRadioButton.TabIndex = 0;
            this.FirstClassEndsRadioButton.TabStop = true;
            this.FirstClassEndsRadioButton.Text = "On first class depletion";
            this.FirstClassEndsRadioButton.UseVisualStyleBackColor = true;
            // 
            // SelectAllignLabel
            // 
            this.SelectAllignLabel.AutoSize = true;
            this.SelectAllignLabel.Location = new System.Drawing.Point(1037, 35);
            this.SelectAllignLabel.Name = "SelectAllignLabel";
            this.SelectAllignLabel.Size = new System.Drawing.Size(137, 13);
            this.SelectAllignLabel.TabIndex = 15;
            this.SelectAllignLabel.Text = "Select allign class (optional)";
            // 
            // DistributionGroupBox
            // 
            this.DistributionGroupBox.Controls.Add(this.UnevenDistrRadioButton);
            this.DistributionGroupBox.Controls.Add(this.EvenDistrRadioButton);
            this.DistributionGroupBox.Location = new System.Drawing.Point(346, 30);
            this.DistributionGroupBox.Name = "DistributionGroupBox";
            this.DistributionGroupBox.Size = new System.Drawing.Size(200, 100);
            this.DistributionGroupBox.TabIndex = 3;
            this.DistributionGroupBox.TabStop = false;
            this.DistributionGroupBox.Text = "Choose class distribution";
            // 
            // UnevenDistrRadioButton
            // 
            this.UnevenDistrRadioButton.AutoSize = true;
            this.UnevenDistrRadioButton.Location = new System.Drawing.Point(6, 65);
            this.UnevenDistrRadioButton.Name = "UnevenDistrRadioButton";
            this.UnevenDistrRadioButton.Size = new System.Drawing.Size(182, 17);
            this.UnevenDistrRadioButton.TabIndex = 2;
            this.UnevenDistrRadioButton.Text = "Use proportional class distribution";
            this.UnevenDistrRadioButton.UseVisualStyleBackColor = true;
            // 
            // EvenDistrRadioButton
            // 
            this.EvenDistrRadioButton.AutoSize = true;
            this.EvenDistrRadioButton.Checked = true;
            this.EvenDistrRadioButton.Location = new System.Drawing.Point(6, 29);
            this.EvenDistrRadioButton.Name = "EvenDistrRadioButton";
            this.EvenDistrRadioButton.Size = new System.Drawing.Size(151, 17);
            this.EvenDistrRadioButton.TabIndex = 1;
            this.EvenDistrRadioButton.TabStop = true;
            this.EvenDistrRadioButton.Text = "Use even class distribution";
            this.EvenDistrRadioButton.UseVisualStyleBackColor = true;
            // 
            // ClassPartDistrGroupBox
            // 
            this.ClassPartDistrGroupBox.Controls.Add(this.ParallelClasPartProcessRadioButton);
            this.ClassPartDistrGroupBox.Controls.Add(this.SerialClassPartProcessRadioButton);
            this.ClassPartDistrGroupBox.Location = new System.Drawing.Point(571, 30);
            this.ClassPartDistrGroupBox.Name = "ClassPartDistrGroupBox";
            this.ClassPartDistrGroupBox.Size = new System.Drawing.Size(200, 100);
            this.ClassPartDistrGroupBox.TabIndex = 4;
            this.ClassPartDistrGroupBox.TabStop = false;
            this.ClassPartDistrGroupBox.Text = "Choose class partition processing";
            // 
            // ParallelClasPartProcessRadioButton
            // 
            this.ParallelClasPartProcessRadioButton.AutoSize = true;
            this.ParallelClasPartProcessRadioButton.Checked = true;
            this.ParallelClasPartProcessRadioButton.Location = new System.Drawing.Point(28, 65);
            this.ParallelClasPartProcessRadioButton.Name = "ParallelClasPartProcessRadioButton";
            this.ParallelClasPartProcessRadioButton.Size = new System.Drawing.Size(129, 17);
            this.ParallelClasPartProcessRadioButton.TabIndex = 1;
            this.ParallelClasPartProcessRadioButton.TabStop = true;
            this.ParallelClasPartProcessRadioButton.Text = "Parallel file processing";
            this.ParallelClasPartProcessRadioButton.UseVisualStyleBackColor = true;
            // 
            // SerialClassPartProcessRadioButton
            // 
            this.SerialClassPartProcessRadioButton.AutoSize = true;
            this.SerialClassPartProcessRadioButton.Location = new System.Drawing.Point(28, 28);
            this.SerialClassPartProcessRadioButton.Name = "SerialClassPartProcessRadioButton";
            this.SerialClassPartProcessRadioButton.Size = new System.Drawing.Size(121, 17);
            this.SerialClassPartProcessRadioButton.TabIndex = 0;
            this.SerialClassPartProcessRadioButton.TabStop = true;
            this.SerialClassPartProcessRadioButton.Text = "Serial file processing";
            this.SerialClassPartProcessRadioButton.UseVisualStyleBackColor = true;
            // 
            // AllignClassTextBox
            // 
            this.AllignClassTextBox.Location = new System.Drawing.Point(1049, 59);
            this.AllignClassTextBox.Name = "AllignClassTextBox";
            this.AllignClassTextBox.Size = new System.Drawing.Size(100, 20);
            this.AllignClassTextBox.TabIndex = 14;
            // 
            // SelectedInputListView
            // 
            this.SelectedInputListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SelectedFilesClassName,
            this.SelectedFilesColumnHeader});
            this.SelectedInputListView.HideSelection = false;
            this.SelectedInputListView.LabelEdit = true;
            this.SelectedInputListView.Location = new System.Drawing.Point(297, 179);
            this.SelectedInputListView.Name = "SelectedInputListView";
            this.SelectedInputListView.Size = new System.Drawing.Size(977, 209);
            this.SelectedInputListView.TabIndex = 11;
            this.SelectedInputListView.UseCompatibleStateImageBehavior = false;
            // 
            // SelectedFilesClassName
            // 
            this.SelectedFilesClassName.Text = "Class Name (optional)";
            this.SelectedFilesClassName.Width = 200;
            // 
            // SelectedFilesColumnHeader
            // 
            this.SelectedFilesColumnHeader.Text = "Selected File Path";
            this.SelectedFilesColumnHeader.Width = 400;
            // 
            // SelectInputListLabel
            // 
            this.SelectInputListLabel.AutoSize = true;
            this.SelectInputListLabel.Location = new System.Drawing.Point(306, 154);
            this.SelectInputListLabel.Name = "SelectInputListLabel";
            this.SelectInputListLabel.Size = new System.Drawing.Size(76, 13);
            this.SelectInputListLabel.TabIndex = 10;
            this.SelectInputListLabel.Text = "Selected Input";
            // 
            // AttributeCheckedList
            // 
            this.AttributeCheckedList.FormattingEnabled = true;
            this.AttributeCheckedList.Items.AddRange(new object[] {
            "TotalEnergy",
            "AverageEnergy",
            "MaxEnergy",
            "PixelCount",
            "Convexity",
            "Width",
            "CrosspointCount",
            "VertexCount",
            "RelativeHaloSize",
            "BranchCount",
            "StdOfEnergy",
            "StdOfArrival",
            "RelLowEnergyPixels",
            "Class",
            "ClFile",
            "PxFile",
            "ClIndex",
            "Branches"});
            this.AttributeCheckedList.Location = new System.Drawing.Point(22, 97);
            this.AttributeCheckedList.Name = "AttributeCheckedList";
            this.AttributeCheckedList.Size = new System.Drawing.Size(245, 304);
            this.AttributeCheckedList.TabIndex = 8;
            // 
            // ProcessButton
            // 
            this.ProcessButton.Location = new System.Drawing.Point(857, 407);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(169, 66);
            this.ProcessButton.TabIndex = 7;
            this.ProcessButton.Text = "Process";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.ProcessButtonClicked);
            // 
            // OutputTextbox
            // 
            this.OutputTextbox.Location = new System.Drawing.Point(98, 71);
            this.OutputTextbox.Name = "OutputTextbox";
            this.OutputTextbox.Size = new System.Drawing.Size(100, 20);
            this.OutputTextbox.TabIndex = 5;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(126, 25);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(107, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse and Add...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseProcessButtonClicked);
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(19, 74);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(73, 13);
            this.OutputLabel.TabIndex = 3;
            this.OutputLabel.Text = "Select output:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 1;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(19, 30);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(101, 13);
            this.InputLabel.TabIndex = 0;
            this.InputLabel.Text = "Add input (partition):";
            // 
            // DescrUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 564);
            this.Controls.Add(this.groupBox1);
            this.Name = "DescrUI";
            this.Text = "Description Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.EndingConditionGroupBox.ResumeLayout(false);
            this.EndingConditionGroupBox.PerformLayout();
            this.DistributionGroupBox.ResumeLayout(false);
            this.DistributionGroupBox.PerformLayout();
            this.ClassPartDistrGroupBox.ResumeLayout(false);
            this.ClassPartDistrGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox AttributeCheckedList;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.TextBox OutputTextbox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.ListView SelectedInputListView;
        private System.Windows.Forms.ColumnHeader SelectedFilesClassName;
        private System.Windows.Forms.ColumnHeader SelectedFilesColumnHeader;
        private System.Windows.Forms.Label SelectInputListLabel;
        private System.Windows.Forms.RadioButton EvenDistrRadioButton;
        private System.Windows.Forms.RadioButton UnevenDistrRadioButton;
        private System.Windows.Forms.GroupBox DistributionGroupBox;
        private System.Windows.Forms.GroupBox ClassPartDistrGroupBox;
        private System.Windows.Forms.RadioButton ParallelClasPartProcessRadioButton;
        private System.Windows.Forms.RadioButton SerialClassPartProcessRadioButton;
        private System.Windows.Forms.Label SelectAllignLabel;
        private System.Windows.Forms.TextBox AllignClassTextBox;
        private System.Windows.Forms.GroupBox EndingConditionGroupBox;
        private System.Windows.Forms.RadioButton FirstPartitionEndsRadioButton;
        private System.Windows.Forms.RadioButton LastClassEndsRadioButton;
        private System.Windows.Forms.RadioButton FirstClassEndsRadioButton;
        private System.Windows.Forms.Label ProcessedCountLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label UsedPartitionDataRatioLabel;
        private System.Windows.Forms.TextBox UsedPartitionDataRatioTextBox;
        private System.Windows.Forms.Button StopProcessingButton;
    }
}

