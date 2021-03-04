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
            this.AttributeCheckedList = new System.Windows.Forms.CheckedListBox();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.InputTextbox = new System.Windows.Forms.TextBox();
            this.OutputTextbox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InputLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AttributeCheckedList);
            this.groupBox1.Controls.Add(this.ProcessButton);
            this.groupBox1.Controls.Add(this.InputTextbox);
            this.groupBox1.Controls.Add(this.OutputTextbox);
            this.groupBox1.Controls.Add(this.BrowseButton);
            this.groupBox1.Controls.Add(this.OutputLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.InputLabel);
            this.groupBox1.Location = new System.Drawing.Point(29, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 330);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Menu";
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
            "Branches"});
            this.AttributeCheckedList.Location = new System.Drawing.Point(22, 97);
            this.AttributeCheckedList.Name = "AttributeCheckedList";
            this.AttributeCheckedList.Size = new System.Drawing.Size(245, 184);
            this.AttributeCheckedList.TabIndex = 8;
            // 
            // ProcessButton
            // 
            this.ProcessButton.Location = new System.Drawing.Point(320, 266);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(142, 37);
            this.ProcessButton.TabIndex = 7;
            this.ProcessButton.Text = "Process";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.ProcessButtonClicked);
            // 
            // InputTextbox
            // 
            this.InputTextbox.Location = new System.Drawing.Point(98, 27);
            this.InputTextbox.Name = "InputTextbox";
            this.InputTextbox.Size = new System.Drawing.Size(100, 20);
            this.InputTextbox.TabIndex = 6;
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
            this.BrowseButton.Location = new System.Drawing.Point(228, 25);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 4;
            this.BrowseButton.Text = "Browse...";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Or";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox AttributeCheckedList;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.TextBox InputTextbox;
        private System.Windows.Forms.TextBox OutputTextbox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label OutputLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label InputLabel;
    }
}

