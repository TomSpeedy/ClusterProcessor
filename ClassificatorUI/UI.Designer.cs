
namespace ClassificatorUI
{
    partial class ClassifierUI
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
            this.TrainingGroupBox = new System.Windows.Forms.GroupBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.BrowseTrainedModelButton = new System.Windows.Forms.Button();
            this.TrainedModelTextBox = new System.Windows.Forms.TextBox();
            this.TrainedModelLabel = new System.Windows.Forms.Label();
            this.SeedLabel = new System.Windows.Forms.Label();
            this.SeedTextBox = new System.Windows.Forms.TextBox();
            this.MinAccuracyLabel = new System.Windows.Forms.Label();
            this.MinAccuracyTextBox = new System.Windows.Forms.TextBox();
            this.MaxRepetitionCountLabel = new System.Windows.Forms.Label();
            this.MaxRepetitionTextBox = new System.Windows.Forms.TextBox();
            this.BrowseClassifierConfigButton = new System.Windows.Forms.Button();
            this.BrowseTrainJsonButton = new System.Windows.Forms.Button();
            this.TrainClassifierButton = new System.Windows.Forms.Button();
            this.ClassifierConfigTextBox = new System.Windows.Forms.TextBox();
            this.ClassifierCalibration = new System.Windows.Forms.Label();
            this.TrainJsonFileLabel = new System.Windows.Forms.Label();
            this.TrainJsonFileTextBox = new System.Windows.Forms.TextBox();
            this.ApplicationGroupBox = new System.Windows.Forms.GroupBox();
            this.MergedClassifierLabel = new System.Windows.Forms.Label();
            this.MergedClassifierNameTextBox = new System.Windows.Forms.TextBox();
            this.MergeClassifiersButton = new System.Windows.Forms.Button();
            this.SplitClassLabelLv3 = new System.Windows.Forms.Label();
            this.SplitClassLv3TextBox = new System.Windows.Forms.TextBox();
            this.SplitClassLabelLv2 = new System.Windows.Forms.Label();
            this.SplitClassLv2TextBox = new System.Windows.Forms.TextBox();
            this.SplitClassLabelLv1 = new System.Windows.Forms.Label();
            this.SplitClassLv1TextBox = new System.Windows.Forms.TextBox();
            this.BrowseModelLv3Button = new System.Windows.Forms.Button();
            this.Level3TrainedModelLabel = new System.Windows.Forms.Label();
            this.TrainedLv3TextBox = new System.Windows.Forms.TextBox();
            this.BrowseModelLv2Button = new System.Windows.Forms.Button();
            this.Level2TrainedModelLabel = new System.Windows.Forms.Label();
            this.TrainedLv2TextBox = new System.Windows.Forms.TextBox();
            this.BrowseModelLv1Button = new System.Windows.Forms.Button();
            this.Level1TrainedModelLabel = new System.Windows.Forms.Label();
            this.TrainedLv1TextBox = new System.Windows.Forms.TextBox();
            this.BrowseRootModelButton = new System.Windows.Forms.Button();
            this.RootTrainedLabel = new System.Windows.Forms.Label();
            this.RootTrainedModelTextBox = new System.Windows.Forms.TextBox();
            this.OutputDirLabel = new System.Windows.Forms.Label();
            this.OutputDirTextBox = new System.Windows.Forms.TextBox();
            this.BrowseOutputDirButton = new System.Windows.Forms.Button();
            this.OutputFileNameLabel = new System.Windows.Forms.Label();
            this.OutpuFileNameTextBox = new System.Windows.Forms.TextBox();
            this.BrowseDirCombinedButton = new System.Windows.Forms.Button();
            this.CombinedClassifierDirTextBox = new System.Windows.Forms.TextBox();
            this.CombinedClassifierDirLabel = new System.Windows.Forms.Label();
            this.TrainingGroupBox.SuspendLayout();
            this.ApplicationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrainingGroupBox
            // 
            this.TrainingGroupBox.Controls.Add(this.OutpuFileNameTextBox);
            this.TrainingGroupBox.Controls.Add(this.OutputFileNameLabel);
            this.TrainingGroupBox.Controls.Add(this.BrowseOutputDirButton);
            this.TrainingGroupBox.Controls.Add(this.OutputDirTextBox);
            this.TrainingGroupBox.Controls.Add(this.OutputDirLabel);
            this.TrainingGroupBox.Controls.Add(this.StopButton);
            this.TrainingGroupBox.Controls.Add(this.BrowseTrainedModelButton);
            this.TrainingGroupBox.Controls.Add(this.TrainedModelTextBox);
            this.TrainingGroupBox.Controls.Add(this.TrainedModelLabel);
            this.TrainingGroupBox.Controls.Add(this.SeedLabel);
            this.TrainingGroupBox.Controls.Add(this.SeedTextBox);
            this.TrainingGroupBox.Controls.Add(this.MinAccuracyLabel);
            this.TrainingGroupBox.Controls.Add(this.MinAccuracyTextBox);
            this.TrainingGroupBox.Controls.Add(this.MaxRepetitionCountLabel);
            this.TrainingGroupBox.Controls.Add(this.MaxRepetitionTextBox);
            this.TrainingGroupBox.Controls.Add(this.BrowseClassifierConfigButton);
            this.TrainingGroupBox.Controls.Add(this.BrowseTrainJsonButton);
            this.TrainingGroupBox.Controls.Add(this.TrainClassifierButton);
            this.TrainingGroupBox.Controls.Add(this.ClassifierConfigTextBox);
            this.TrainingGroupBox.Controls.Add(this.ClassifierCalibration);
            this.TrainingGroupBox.Controls.Add(this.TrainJsonFileLabel);
            this.TrainingGroupBox.Controls.Add(this.TrainJsonFileTextBox);
            this.TrainingGroupBox.Location = new System.Drawing.Point(12, 12);
            this.TrainingGroupBox.Name = "TrainingGroupBox";
            this.TrainingGroupBox.Size = new System.Drawing.Size(847, 206);
            this.TrainingGroupBox.TabIndex = 0;
            this.TrainingGroupBox.TabStop = false;
            this.TrainingGroupBox.Text = "Single Classifier Training";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(165, 177);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(129, 23);
            this.StopButton.TabIndex = 19;
            this.StopButton.Text = "Stop the training";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButtonClicked);
            // 
            // BrowseTrainedModelButton
            // 
            this.BrowseTrainedModelButton.Location = new System.Drawing.Point(657, 132);
            this.BrowseTrainedModelButton.Name = "BrowseTrainedModelButton";
            this.BrowseTrainedModelButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseTrainedModelButton.TabIndex = 18;
            this.BrowseTrainedModelButton.Text = "Browse";
            this.BrowseTrainedModelButton.UseVisualStyleBackColor = true;
            this.BrowseTrainedModelButton.Click += new System.EventHandler(this.BrowseClassifiersClicked);
            // 
            // TrainedModelTextBox
            // 
            this.TrainedModelTextBox.Location = new System.Drawing.Point(526, 135);
            this.TrainedModelTextBox.Name = "TrainedModelTextBox";
            this.TrainedModelTextBox.Size = new System.Drawing.Size(100, 20);
            this.TrainedModelTextBox.TabIndex = 17;
            // 
            // TrainedModelLabel
            // 
            this.TrainedModelLabel.AutoSize = true;
            this.TrainedModelLabel.Location = new System.Drawing.Point(399, 142);
            this.TrainedModelLabel.Name = "TrainedModelLabel";
            this.TrainedModelLabel.Size = new System.Drawing.Size(121, 13);
            this.TrainedModelLabel.TabIndex = 15;
            this.TrainedModelLabel.Text = "Trained Model (optional)";
            // 
            // SeedLabel
            // 
            this.SeedLabel.AutoSize = true;
            this.SeedLabel.Location = new System.Drawing.Point(399, 101);
            this.SeedLabel.Name = "SeedLabel";
            this.SeedLabel.Size = new System.Drawing.Size(32, 13);
            this.SeedLabel.TabIndex = 12;
            this.SeedLabel.Text = "Seed";
            // 
            // SeedTextBox
            // 
            this.SeedTextBox.Location = new System.Drawing.Point(526, 94);
            this.SeedTextBox.Name = "SeedTextBox";
            this.SeedTextBox.Size = new System.Drawing.Size(100, 20);
            this.SeedTextBox.TabIndex = 11;
            // 
            // MinAccuracyLabel
            // 
            this.MinAccuracyLabel.AutoSize = true;
            this.MinAccuracyLabel.Location = new System.Drawing.Point(399, 64);
            this.MinAccuracyLabel.Name = "MinAccuracyLabel";
            this.MinAccuracyLabel.Size = new System.Drawing.Size(89, 13);
            this.MinAccuracyLabel.TabIndex = 10;
            this.MinAccuracyLabel.Text = "Minimal accuracy";
            // 
            // MinAccuracyTextBox
            // 
            this.MinAccuracyTextBox.Location = new System.Drawing.Point(526, 61);
            this.MinAccuracyTextBox.Name = "MinAccuracyTextBox";
            this.MinAccuracyTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinAccuracyTextBox.TabIndex = 9;
            // 
            // MaxRepetitionCountLabel
            // 
            this.MaxRepetitionCountLabel.AutoSize = true;
            this.MaxRepetitionCountLabel.Location = new System.Drawing.Point(399, 27);
            this.MaxRepetitionCountLabel.Name = "MaxRepetitionCountLabel";
            this.MaxRepetitionCountLabel.Size = new System.Drawing.Size(121, 13);
            this.MaxRepetitionCountLabel.TabIndex = 8;
            this.MaxRepetitionCountLabel.Text = "Maximal repetition count";
            // 
            // MaxRepetitionTextBox
            // 
            this.MaxRepetitionTextBox.Location = new System.Drawing.Point(526, 24);
            this.MaxRepetitionTextBox.Name = "MaxRepetitionTextBox";
            this.MaxRepetitionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxRepetitionTextBox.TabIndex = 7;
            // 
            // BrowseClassifierConfigButton
            // 
            this.BrowseClassifierConfigButton.Location = new System.Drawing.Point(296, 59);
            this.BrowseClassifierConfigButton.Name = "BrowseClassifierConfigButton";
            this.BrowseClassifierConfigButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseClassifierConfigButton.TabIndex = 6;
            this.BrowseClassifierConfigButton.Text = "Browse";
            this.BrowseClassifierConfigButton.UseVisualStyleBackColor = true;
            this.BrowseClassifierConfigButton.Click += new System.EventHandler(this.BrowseJsonFieldsClicked);
            // 
            // BrowseTrainJsonButton
            // 
            this.BrowseTrainJsonButton.Location = new System.Drawing.Point(296, 24);
            this.BrowseTrainJsonButton.Name = "BrowseTrainJsonButton";
            this.BrowseTrainJsonButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseTrainJsonButton.TabIndex = 5;
            this.BrowseTrainJsonButton.Text = "Browse";
            this.BrowseTrainJsonButton.UseVisualStyleBackColor = true;
            this.BrowseTrainJsonButton.Click += new System.EventHandler(this.BrowseJsonFieldsClicked);
            // 
            // TrainClassifierButton
            // 
            this.TrainClassifierButton.Location = new System.Drawing.Point(6, 177);
            this.TrainClassifierButton.Name = "TrainClassifierButton";
            this.TrainClassifierButton.Size = new System.Drawing.Size(139, 23);
            this.TrainClassifierButton.TabIndex = 4;
            this.TrainClassifierButton.Text = "Train Classifier";
            this.TrainClassifierButton.UseVisualStyleBackColor = true;
            this.TrainClassifierButton.Click += new System.EventHandler(this.TrainSimpleClassifierClicked);
            // 
            // ClassifierConfigTextBox
            // 
            this.ClassifierConfigTextBox.Location = new System.Drawing.Point(165, 59);
            this.ClassifierConfigTextBox.Name = "ClassifierConfigTextBox";
            this.ClassifierConfigTextBox.Size = new System.Drawing.Size(100, 20);
            this.ClassifierConfigTextBox.TabIndex = 3;
            // 
            // ClassifierCalibration
            // 
            this.ClassifierCalibration.AutoSize = true;
            this.ClassifierCalibration.Location = new System.Drawing.Point(6, 62);
            this.ClassifierCalibration.Name = "ClassifierCalibration";
            this.ClassifierCalibration.Size = new System.Drawing.Size(96, 13);
            this.ClassifierCalibration.TabIndex = 2;
            this.ClassifierCalibration.Text = "Classifier config file";
            // 
            // TrainJsonFileLabel
            // 
            this.TrainJsonFileLabel.AutoSize = true;
            this.TrainJsonFileLabel.Location = new System.Drawing.Point(6, 29);
            this.TrainJsonFileLabel.Name = "TrainJsonFileLabel";
            this.TrainJsonFileLabel.Size = new System.Drawing.Size(89, 13);
            this.TrainJsonFileLabel.TabIndex = 1;
            this.TrainJsonFileLabel.Text = "Training Json File";
            // 
            // TrainJsonFileTextBox
            // 
            this.TrainJsonFileTextBox.Location = new System.Drawing.Point(165, 26);
            this.TrainJsonFileTextBox.Name = "TrainJsonFileTextBox";
            this.TrainJsonFileTextBox.Size = new System.Drawing.Size(100, 20);
            this.TrainJsonFileTextBox.TabIndex = 0;
            // 
            // ApplicationGroupBox
            // 
            this.ApplicationGroupBox.Controls.Add(this.BrowseDirCombinedButton);
            this.ApplicationGroupBox.Controls.Add(this.CombinedClassifierDirTextBox);
            this.ApplicationGroupBox.Controls.Add(this.CombinedClassifierDirLabel);
            this.ApplicationGroupBox.Controls.Add(this.MergedClassifierLabel);
            this.ApplicationGroupBox.Controls.Add(this.MergedClassifierNameTextBox);
            this.ApplicationGroupBox.Controls.Add(this.MergeClassifiersButton);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLabelLv3);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLv3TextBox);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLabelLv2);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLv2TextBox);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLabelLv1);
            this.ApplicationGroupBox.Controls.Add(this.SplitClassLv1TextBox);
            this.ApplicationGroupBox.Controls.Add(this.BrowseModelLv3Button);
            this.ApplicationGroupBox.Controls.Add(this.Level3TrainedModelLabel);
            this.ApplicationGroupBox.Controls.Add(this.TrainedLv3TextBox);
            this.ApplicationGroupBox.Controls.Add(this.BrowseModelLv2Button);
            this.ApplicationGroupBox.Controls.Add(this.Level2TrainedModelLabel);
            this.ApplicationGroupBox.Controls.Add(this.TrainedLv2TextBox);
            this.ApplicationGroupBox.Controls.Add(this.BrowseModelLv1Button);
            this.ApplicationGroupBox.Controls.Add(this.Level1TrainedModelLabel);
            this.ApplicationGroupBox.Controls.Add(this.TrainedLv1TextBox);
            this.ApplicationGroupBox.Controls.Add(this.BrowseRootModelButton);
            this.ApplicationGroupBox.Controls.Add(this.RootTrainedLabel);
            this.ApplicationGroupBox.Controls.Add(this.RootTrainedModelTextBox);
            this.ApplicationGroupBox.Location = new System.Drawing.Point(12, 242);
            this.ApplicationGroupBox.Name = "ApplicationGroupBox";
            this.ApplicationGroupBox.Size = new System.Drawing.Size(847, 289);
            this.ApplicationGroupBox.TabIndex = 1;
            this.ApplicationGroupBox.TabStop = false;
            this.ApplicationGroupBox.Text = "Classifier Combining";
            // 
            // MergedClassifierLabel
            // 
            this.MergedClassifierLabel.AutoSize = true;
            this.MergedClassifierLabel.Location = new System.Drawing.Point(6, 214);
            this.MergedClassifierLabel.Name = "MergedClassifierLabel";
            this.MergedClassifierLabel.Size = new System.Drawing.Size(129, 13);
            this.MergedClassifierLabel.TabIndex = 24;
            this.MergedClassifierLabel.Text = "Combined Classifier Name";
            // 
            // MergedClassifierNameTextBox
            // 
            this.MergedClassifierNameTextBox.Location = new System.Drawing.Point(165, 211);
            this.MergedClassifierNameTextBox.Name = "MergedClassifierNameTextBox";
            this.MergedClassifierNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.MergedClassifierNameTextBox.TabIndex = 23;
            // 
            // MergeClassifiersButton
            // 
            this.MergeClassifiersButton.Location = new System.Drawing.Point(6, 246);
            this.MergeClassifiersButton.Name = "MergeClassifiersButton";
            this.MergeClassifiersButton.Size = new System.Drawing.Size(159, 37);
            this.MergeClassifiersButton.TabIndex = 22;
            this.MergeClassifiersButton.Text = "Combine Classifiers";
            this.MergeClassifiersButton.UseVisualStyleBackColor = true;
            this.MergeClassifiersButton.Click += new System.EventHandler(this.MergeClassifiersClicked);
            // 
            // SplitClassLabelLv3
            // 
            this.SplitClassLabelLv3.AutoSize = true;
            this.SplitClassLabelLv3.Location = new System.Drawing.Point(327, 102);
            this.SplitClassLabelLv3.Name = "SplitClassLabelLv3";
            this.SplitClassLabelLv3.Size = new System.Drawing.Size(55, 13);
            this.SplitClassLabelLv3.TabIndex = 21;
            this.SplitClassLabelLv3.Text = "SplitClass:";
            // 
            // SplitClassLv3TextBox
            // 
            this.SplitClassLv3TextBox.Location = new System.Drawing.Point(402, 98);
            this.SplitClassLv3TextBox.Name = "SplitClassLv3TextBox";
            this.SplitClassLv3TextBox.Size = new System.Drawing.Size(100, 20);
            this.SplitClassLv3TextBox.TabIndex = 20;
            // 
            // SplitClassLabelLv2
            // 
            this.SplitClassLabelLv2.AutoSize = true;
            this.SplitClassLabelLv2.Location = new System.Drawing.Point(327, 64);
            this.SplitClassLabelLv2.Name = "SplitClassLabelLv2";
            this.SplitClassLabelLv2.Size = new System.Drawing.Size(58, 13);
            this.SplitClassLabelLv2.TabIndex = 19;
            this.SplitClassLabelLv2.Text = "Split Class:";
            // 
            // SplitClassLv2TextBox
            // 
            this.SplitClassLv2TextBox.Location = new System.Drawing.Point(402, 61);
            this.SplitClassLv2TextBox.Name = "SplitClassLv2TextBox";
            this.SplitClassLv2TextBox.Size = new System.Drawing.Size(100, 20);
            this.SplitClassLv2TextBox.TabIndex = 18;
            // 
            // SplitClassLabelLv1
            // 
            this.SplitClassLabelLv1.AutoSize = true;
            this.SplitClassLabelLv1.Location = new System.Drawing.Point(327, 28);
            this.SplitClassLabelLv1.Name = "SplitClassLabelLv1";
            this.SplitClassLabelLv1.Size = new System.Drawing.Size(58, 13);
            this.SplitClassLabelLv1.TabIndex = 17;
            this.SplitClassLabelLv1.Text = "Split Class:";
            // 
            // SplitClassLv1TextBox
            // 
            this.SplitClassLv1TextBox.Location = new System.Drawing.Point(402, 25);
            this.SplitClassLv1TextBox.Name = "SplitClassLv1TextBox";
            this.SplitClassLv1TextBox.Size = new System.Drawing.Size(100, 20);
            this.SplitClassLv1TextBox.TabIndex = 16;
            // 
            // BrowseModelLv3Button
            // 
            this.BrowseModelLv3Button.Location = new System.Drawing.Point(236, 131);
            this.BrowseModelLv3Button.Name = "BrowseModelLv3Button";
            this.BrowseModelLv3Button.Size = new System.Drawing.Size(75, 23);
            this.BrowseModelLv3Button.TabIndex = 15;
            this.BrowseModelLv3Button.Text = "Browse";
            this.BrowseModelLv3Button.UseVisualStyleBackColor = true;
            this.BrowseModelLv3Button.Click += new System.EventHandler(this.BrowseClassifiersClicked);
            // 
            // Level3TrainedModelLabel
            // 
            this.Level3TrainedModelLabel.AutoSize = true;
            this.Level3TrainedModelLabel.Location = new System.Drawing.Point(6, 137);
            this.Level3TrainedModelLabel.Name = "Level3TrainedModelLabel";
            this.Level3TrainedModelLabel.Size = new System.Drawing.Size(101, 13);
            this.Level3TrainedModelLabel.TabIndex = 14;
            this.Level3TrainedModelLabel.Text = "Trained Model Lvl 3";
            // 
            // TrainedLv3TextBox
            // 
            this.TrainedLv3TextBox.Location = new System.Drawing.Point(113, 134);
            this.TrainedLv3TextBox.Name = "TrainedLv3TextBox";
            this.TrainedLv3TextBox.Size = new System.Drawing.Size(100, 20);
            this.TrainedLv3TextBox.TabIndex = 13;
            // 
            // BrowseModelLv2Button
            // 
            this.BrowseModelLv2Button.Location = new System.Drawing.Point(236, 97);
            this.BrowseModelLv2Button.Name = "BrowseModelLv2Button";
            this.BrowseModelLv2Button.Size = new System.Drawing.Size(75, 23);
            this.BrowseModelLv2Button.TabIndex = 12;
            this.BrowseModelLv2Button.Text = "Browse";
            this.BrowseModelLv2Button.UseVisualStyleBackColor = true;
            this.BrowseModelLv2Button.Click += new System.EventHandler(this.BrowseClassifiersClicked);
            // 
            // Level2TrainedModelLabel
            // 
            this.Level2TrainedModelLabel.AutoSize = true;
            this.Level2TrainedModelLabel.Location = new System.Drawing.Point(6, 100);
            this.Level2TrainedModelLabel.Name = "Level2TrainedModelLabel";
            this.Level2TrainedModelLabel.Size = new System.Drawing.Size(101, 13);
            this.Level2TrainedModelLabel.TabIndex = 11;
            this.Level2TrainedModelLabel.Text = "Trained Model Lvl 2";
            // 
            // TrainedLv2TextBox
            // 
            this.TrainedLv2TextBox.Location = new System.Drawing.Point(113, 97);
            this.TrainedLv2TextBox.Name = "TrainedLv2TextBox";
            this.TrainedLv2TextBox.Size = new System.Drawing.Size(100, 20);
            this.TrainedLv2TextBox.TabIndex = 10;
            // 
            // BrowseModelLv1Button
            // 
            this.BrowseModelLv1Button.Location = new System.Drawing.Point(236, 60);
            this.BrowseModelLv1Button.Name = "BrowseModelLv1Button";
            this.BrowseModelLv1Button.Size = new System.Drawing.Size(75, 23);
            this.BrowseModelLv1Button.TabIndex = 9;
            this.BrowseModelLv1Button.Text = "Browse";
            this.BrowseModelLv1Button.UseVisualStyleBackColor = true;
            this.BrowseModelLv1Button.Click += new System.EventHandler(this.BrowseClassifiersClicked);
            // 
            // Level1TrainedModelLabel
            // 
            this.Level1TrainedModelLabel.AutoSize = true;
            this.Level1TrainedModelLabel.Location = new System.Drawing.Point(6, 66);
            this.Level1TrainedModelLabel.Name = "Level1TrainedModelLabel";
            this.Level1TrainedModelLabel.Size = new System.Drawing.Size(101, 13);
            this.Level1TrainedModelLabel.TabIndex = 8;
            this.Level1TrainedModelLabel.Text = "Trained Model Lvl 1";
            // 
            // TrainedLv1TextBox
            // 
            this.TrainedLv1TextBox.Location = new System.Drawing.Point(113, 63);
            this.TrainedLv1TextBox.Name = "TrainedLv1TextBox";
            this.TrainedLv1TextBox.Size = new System.Drawing.Size(100, 20);
            this.TrainedLv1TextBox.TabIndex = 7;
            // 
            // BrowseRootModelButton
            // 
            this.BrowseRootModelButton.Location = new System.Drawing.Point(236, 23);
            this.BrowseRootModelButton.Name = "BrowseRootModelButton";
            this.BrowseRootModelButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseRootModelButton.TabIndex = 6;
            this.BrowseRootModelButton.Text = "Browse";
            this.BrowseRootModelButton.UseVisualStyleBackColor = true;
            this.BrowseRootModelButton.Click += new System.EventHandler(this.BrowseClassifiersClicked);
            // 
            // RootTrainedLabel
            // 
            this.RootTrainedLabel.AutoSize = true;
            this.RootTrainedLabel.Location = new System.Drawing.Point(6, 29);
            this.RootTrainedLabel.Name = "RootTrainedLabel";
            this.RootTrainedLabel.Size = new System.Drawing.Size(101, 13);
            this.RootTrainedLabel.TabIndex = 3;
            this.RootTrainedLabel.Text = "Root Trained Model";
            // 
            // RootTrainedModelTextBox
            // 
            this.RootTrainedModelTextBox.Location = new System.Drawing.Point(113, 26);
            this.RootTrainedModelTextBox.Name = "RootTrainedModelTextBox";
            this.RootTrainedModelTextBox.Size = new System.Drawing.Size(100, 20);
            this.RootTrainedModelTextBox.TabIndex = 2;
            // 
            // OutputDirLabel
            // 
            this.OutputDirLabel.AutoSize = true;
            this.OutputDirLabel.Location = new System.Drawing.Point(6, 97);
            this.OutputDirLabel.Name = "OutputDirLabel";
            this.OutputDirLabel.Size = new System.Drawing.Size(98, 13);
            this.OutputDirLabel.TabIndex = 20;
            this.OutputDirLabel.Text = "Output file directory";
            // 
            // OutputDirTextBox
            // 
            this.OutputDirTextBox.Location = new System.Drawing.Point(165, 94);
            this.OutputDirTextBox.Name = "OutputDirTextBox";
            this.OutputDirTextBox.Size = new System.Drawing.Size(100, 20);
            this.OutputDirTextBox.TabIndex = 23;
            // 
            // BrowseOutputDirButton
            // 
            this.BrowseOutputDirButton.Location = new System.Drawing.Point(296, 91);
            this.BrowseOutputDirButton.Name = "BrowseOutputDirButton";
            this.BrowseOutputDirButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseOutputDirButton.TabIndex = 24;
            this.BrowseOutputDirButton.Text = "Browse";
            this.BrowseOutputDirButton.UseVisualStyleBackColor = true;
            this.BrowseOutputDirButton.Click += new System.EventHandler(this.BrowseOutputFolderClicked);
            // 
            // OutputFileNameLabel
            // 
            this.OutputFileNameLabel.AutoSize = true;
            this.OutputFileNameLabel.Location = new System.Drawing.Point(11, 132);
            this.OutputFileNameLabel.Name = "OutputFileNameLabel";
            this.OutputFileNameLabel.Size = new System.Drawing.Size(84, 13);
            this.OutputFileNameLabel.TabIndex = 25;
            this.OutputFileNameLabel.Text = "Output file name";
            // 
            // OutpuFileNameTextBox
            // 
            this.OutpuFileNameTextBox.Location = new System.Drawing.Point(165, 129);
            this.OutpuFileNameTextBox.Name = "OutpuFileNameTextBox";
            this.OutpuFileNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.OutpuFileNameTextBox.TabIndex = 26;
            // 
            // BrowseDirCombinedButton
            // 
            this.BrowseDirCombinedButton.Location = new System.Drawing.Point(284, 177);
            this.BrowseDirCombinedButton.Name = "BrowseDirCombinedButton";
            this.BrowseDirCombinedButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseDirCombinedButton.TabIndex = 27;
            this.BrowseDirCombinedButton.Text = "Browse";
            this.BrowseDirCombinedButton.UseVisualStyleBackColor = true;
            this.BrowseDirCombinedButton.Click += new System.EventHandler(this.BrowseOutputDirCombinedClicked);
            // 
            // CombinedClassifierDirTextBox
            // 
            this.CombinedClassifierDirTextBox.Location = new System.Drawing.Point(165, 177);
            this.CombinedClassifierDirTextBox.Name = "CombinedClassifierDirTextBox";
            this.CombinedClassifierDirTextBox.Size = new System.Drawing.Size(100, 20);
            this.CombinedClassifierDirTextBox.TabIndex = 26;
            // 
            // CombinedClassifierDirLabel
            // 
            this.CombinedClassifierDirLabel.AutoSize = true;
            this.CombinedClassifierDirLabel.Location = new System.Drawing.Point(6, 180);
            this.CombinedClassifierDirLabel.Name = "CombinedClassifierDirLabel";
            this.CombinedClassifierDirLabel.Size = new System.Drawing.Size(143, 13);
            this.CombinedClassifierDirLabel.TabIndex = 25;
            this.CombinedClassifierDirLabel.Text = "Combined Classifier Directory";
            // 
            // ClassifierUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 543);
            this.Controls.Add(this.ApplicationGroupBox);
            this.Controls.Add(this.TrainingGroupBox);
            this.Name = "ClassifierUI";
            this.Text = "ClassifierUI";
            this.TrainingGroupBox.ResumeLayout(false);
            this.TrainingGroupBox.PerformLayout();
            this.ApplicationGroupBox.ResumeLayout(false);
            this.ApplicationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TrainingGroupBox;
        private System.Windows.Forms.TextBox ClassifierConfigTextBox;
        private System.Windows.Forms.Label ClassifierCalibration;
        private System.Windows.Forms.Label TrainJsonFileLabel;
        private System.Windows.Forms.TextBox TrainJsonFileTextBox;
        private System.Windows.Forms.GroupBox ApplicationGroupBox;
        private System.Windows.Forms.Button BrowseClassifierConfigButton;
        private System.Windows.Forms.Button BrowseTrainJsonButton;
        private System.Windows.Forms.Button TrainClassifierButton;
        private System.Windows.Forms.Label SplitClassLabelLv3;
        private System.Windows.Forms.TextBox SplitClassLv3TextBox;
        private System.Windows.Forms.Label SplitClassLabelLv2;
        private System.Windows.Forms.TextBox SplitClassLv2TextBox;
        private System.Windows.Forms.Label SplitClassLabelLv1;
        private System.Windows.Forms.TextBox SplitClassLv1TextBox;
        private System.Windows.Forms.Button BrowseModelLv3Button;
        private System.Windows.Forms.Label Level3TrainedModelLabel;
        private System.Windows.Forms.TextBox TrainedLv3TextBox;
        private System.Windows.Forms.Button BrowseModelLv2Button;
        private System.Windows.Forms.Label Level2TrainedModelLabel;
        private System.Windows.Forms.TextBox TrainedLv2TextBox;
        private System.Windows.Forms.Button BrowseModelLv1Button;
        private System.Windows.Forms.Label Level1TrainedModelLabel;
        private System.Windows.Forms.TextBox TrainedLv1TextBox;
        private System.Windows.Forms.Button BrowseRootModelButton;
        private System.Windows.Forms.Label RootTrainedLabel;
        private System.Windows.Forms.TextBox RootTrainedModelTextBox;
        private System.Windows.Forms.Button MergeClassifiersButton;
        private System.Windows.Forms.Label MaxRepetitionCountLabel;
        private System.Windows.Forms.TextBox MaxRepetitionTextBox;
        private System.Windows.Forms.Label MinAccuracyLabel;
        private System.Windows.Forms.TextBox MinAccuracyTextBox;
        private System.Windows.Forms.TextBox MergedClassifierNameTextBox;
        private System.Windows.Forms.Label MergedClassifierLabel;
        private System.Windows.Forms.Label SeedLabel;
        private System.Windows.Forms.TextBox SeedTextBox;
        private System.Windows.Forms.Label TrainedModelLabel;
        private System.Windows.Forms.Button BrowseTrainedModelButton;
        private System.Windows.Forms.TextBox TrainedModelTextBox;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.TextBox OutpuFileNameTextBox;
        private System.Windows.Forms.Label OutputFileNameLabel;
        private System.Windows.Forms.Button BrowseOutputDirButton;
        private System.Windows.Forms.TextBox OutputDirTextBox;
        private System.Windows.Forms.Label OutputDirLabel;
        private System.Windows.Forms.Button BrowseDirCombinedButton;
        private System.Windows.Forms.TextBox CombinedClassifierDirTextBox;
        private System.Windows.Forms.Label CombinedClassifierDirLabel;
    }
}

