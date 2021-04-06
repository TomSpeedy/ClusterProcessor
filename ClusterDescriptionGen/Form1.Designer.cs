namespace ClusterDescriptionGen
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AddConfigFileButton = new System.Windows.Forms.Button();
            this.SelectedInputListView = new System.Windows.Forms.ListView();
            this.SelectedFilesClassName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SelectedFilesColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ConfigFilesInputColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SelectInputListLabel = new System.Windows.Forms.Label();
            this.AttributeCheckedList = new System.Windows.Forms.CheckedListBox();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.OutputTextbox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InputLabel = new System.Windows.Forms.Label();
            this.EvenDistrRadioButton = new System.Windows.Forms.RadioButton();
            this.UnevenDistrRadioButton = new System.Windows.Forms.RadioButton();
            this.DistributionGroupBox = new System.Windows.Forms.GroupBox();
            this.ClassPartDistrGroupBox = new System.Windows.Forms.GroupBox();
            this.ParallelClasPartProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.SerialClassPartProcessRadioButton = new System.Windows.Forms.RadioButton();
            this.AllignClassTextBox = new System.Windows.Forms.TextBox();
            this.SelectAllignLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.DistributionGroupBox.SuspendLayout();
            this.ClassPartDistrGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SelectAllignLabel);
            this.groupBox1.Controls.Add(this.DistributionGroupBox);
            this.groupBox1.Controls.Add(this.ClassPartDistrGroupBox);
            this.groupBox1.Controls.Add(this.AllignClassTextBox);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.AddConfigFileButton);
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(995, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 37);
            this.button1.TabIndex = 13;
            this.button1.Text = "Copy Last Config Path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CopyLastPathButtonClicked);
            // 
            // AddConfigFileButton
            // 
            this.AddConfigFileButton.Location = new System.Drawing.Point(204, 25);
            this.AddConfigFileButton.Name = "AddConfigFileButton";
            this.AddConfigFileButton.Size = new System.Drawing.Size(101, 23);
            this.AddConfigFileButton.TabIndex = 12;
            this.AddConfigFileButton.Text = "Add Config File";
            this.AddConfigFileButton.UseVisualStyleBackColor = true;
            this.AddConfigFileButton.Click += new System.EventHandler(this.BrowseConfigButtonClicked);
            // 
            // SelectedInputListView
            // 
            this.SelectedInputListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SelectedFilesClassName,
            this.SelectedFilesColumnHeader,
            this.ConfigFilesInputColumn});
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
            this.SelectedFilesClassName.Width = 100;
            // 
            // SelectedFilesColumnHeader
            // 
            this.SelectedFilesColumnHeader.Text = "Selected File Path";
            this.SelectedFilesColumnHeader.Width = 400;
            // 
            // ConfigFilesInputColumn
            // 
            this.ConfigFilesInputColumn.Text = "Config File Path";
            this.ConfigFilesInputColumn.Width = 400;
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
            "Branches"});
            this.AttributeCheckedList.Location = new System.Drawing.Point(22, 97);
            this.AttributeCheckedList.Name = "AttributeCheckedList";
            this.AttributeCheckedList.Size = new System.Drawing.Size(245, 319);
            this.AttributeCheckedList.TabIndex = 8;
            // 
            // ProcessButton
            // 
            this.ProcessButton.Location = new System.Drawing.Point(809, 424);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(142, 37);
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
            this.BrowseButton.Location = new System.Drawing.Point(91, 25);
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
            this.InputLabel.Size = new System.Drawing.Size(66, 13);
            this.InputLabel.TabIndex = 0;
            this.InputLabel.Text = "Select input:";
            // 
            // EvenDistrRadioButton
            // 
            this.EvenDistrRadioButton.AutoSize = true;
            this.EvenDistrRadioButton.Location = new System.Drawing.Point(6, 29);
            this.EvenDistrRadioButton.Name = "EvenDistrRadioButton";
            this.EvenDistrRadioButton.Size = new System.Drawing.Size(151, 17);
            this.EvenDistrRadioButton.TabIndex = 1;
            this.EvenDistrRadioButton.TabStop = true;
            this.EvenDistrRadioButton.Text = "Use even class distribution";
            this.EvenDistrRadioButton.UseVisualStyleBackColor = true;
            // 
            // UnevenDistrRadioButton
            // 
            this.UnevenDistrRadioButton.AutoSize = true;
            this.UnevenDistrRadioButton.Checked = true;
            this.UnevenDistrRadioButton.Location = new System.Drawing.Point(6, 65);
            this.UnevenDistrRadioButton.Name = "UnevenDistrRadioButton";
            this.UnevenDistrRadioButton.Size = new System.Drawing.Size(182, 17);
            this.UnevenDistrRadioButton.TabIndex = 2;
            this.UnevenDistrRadioButton.TabStop = true;
            this.UnevenDistrRadioButton.Text = "Use proportional class distribution";
            this.UnevenDistrRadioButton.UseVisualStyleBackColor = true;
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
            this.AllignClassTextBox.Location = new System.Drawing.Point(809, 58);
            this.AllignClassTextBox.Name = "AllignClassTextBox";
            this.AllignClassTextBox.Size = new System.Drawing.Size(100, 20);
            this.AllignClassTextBox.TabIndex = 14;
            // 
            // SelectAllignLabel
            // 
            this.SelectAllignLabel.AutoSize = true;
            this.SelectAllignLabel.Location = new System.Drawing.Point(806, 35);
            this.SelectAllignLabel.Name = "SelectAllignLabel";
            this.SelectAllignLabel.Size = new System.Drawing.Size(137, 13);
            this.SelectAllignLabel.TabIndex = 15;
            this.SelectAllignLabel.Text = "Select allign class (optional)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 564);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Button AddConfigFileButton;
        private System.Windows.Forms.ColumnHeader ConfigFilesInputColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton EvenDistrRadioButton;
        private System.Windows.Forms.RadioButton UnevenDistrRadioButton;
        private System.Windows.Forms.GroupBox DistributionGroupBox;
        private System.Windows.Forms.GroupBox ClassPartDistrGroupBox;
        private System.Windows.Forms.RadioButton ParallelClasPartProcessRadioButton;
        private System.Windows.Forms.RadioButton SerialClassPartProcessRadioButton;
        private System.Windows.Forms.Label SelectAllignLabel;
        private System.Windows.Forms.TextBox AllignClassTextBox;
    }
}

