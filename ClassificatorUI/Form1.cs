using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassifierForClusters;
using System.Globalization;
using System.Threading;
using ClusterCalculator;

namespace ClassificatorUI
{
    public partial class ClassifierUI : Form
    {
        bool TrainingStopped = false;
        public ClassifierUI()
        {
            InitializeComponent();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        public void BrowseButtonClicked(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if (pressedButton == BrowseClassifierConfigButton)
                {
                    ClassifierConfigTextBox.Text = fileDialog.FileName;
                }
                else if (pressedButton == BrowseTrainJsonButton)
                {
                    TrainJsonFileTextBox.Text = fileDialog.FileName;
                }
                else if (pressedButton == BrowseRootModelButton)
                {
                    RootTrainedModelTextBox.Text = fileDialog.FileName;
                }
                else if (pressedButton == BrowseModelLv1Button)
                {
                    TrainedLv1TextBox.Text = fileDialog.FileName;
                }
                else if (pressedButton == BrowseModelLv2Button)
                {
                    TrainedLv2TextBox.Text = fileDialog.FileName;
                }
                else if (pressedButton == BrowseModelLv3Button)
                {
                    TrainedLv3TextBox.Text = fileDialog.FileName;
                }
            }

        }

        public void TrainSimpleClassifierClicked(object sender, EventArgs e)
        {
            ITrainableClassifier classifier;
            if (TrainedModelTextBox.Text == "")
                classifier = TrainableClassifierFactory.CreateNew("defaultMLP");
            else
            {
                classifier = new NNClassifier();
                classifier.LoadFromFile(TrainedModelTextBox.Text);
            }
            int maxRepetCount = 1;
            if (uint.TryParse(MaxRepetitionTextBox.Text, out uint chosenRepetCount))
            {
                maxRepetCount = (int)chosenRepetCount;
            }
            double minAccuracy = 0;
            if (double.TryParse(MinAccuracyTextBox.Text, out double chosenMinAccuracy))
            {
                minAccuracy = chosenMinAccuracy;
            }
            int seed = 0;
            if (int.TryParse(SeedTextBox.Text, out int parsedSeed))
            {
                seed = parsedSeed;
            }
            int iterationCount = 0;
            double accuracy = 0;

            while (iterationCount < maxRepetCount && accuracy <= minAccuracy)
            {              
               Thread thread = new Thread(() => { accuracy = classifier.Train(ClassifierConfigTextBox.Text, TrainJsonFileTextBox.Text, ref TrainingStopped, minAccuracy, seed); });
                thread.Start();
               thread.Join();
               iterationCount++;
            }
            MessageBox.Show("The training process has successfully ended");
        }

        public void MergeClassifiersClicked(object sender, EventArgs e)
        {
            
            List<IClassifier> classifiers = new List<IClassifier>();
            List<string> splitClasses = new List<string>();
            IClassifier simpleClassifier = null;
            const int textBoxCount = 4;
            List<string> classifierPaths = new List<string>(new string[textBoxCount]
                {
                    RootTrainedModelTextBox.Text,
                    TrainedLv1TextBox.Text,
                    TrainedLv2TextBox.Text,
                    TrainedLv3TextBox.Text
                });
            List<string> splitClassesInBox = new List<string>(new string[textBoxCount - 1]
                {
                    SplitClassLv1TextBox.Text,
                    SplitClassLv2TextBox.Text,
                    SplitClassLv3TextBox.Text
                });
            int index = 0;
            while ( index < textBoxCount && classifierPaths[index] != "")
            {
                simpleClassifier = new NNClassifier();
                simpleClassifier.LoadFromFile(classifierPaths[index]);
                classifiers.Add(simpleClassifier);
                if (index > 0)
                    splitClasses.Add(splitClassesInBox[index - 1]);
                index++;
            }       




            MultiLayeredClassifier multiClassifier = new MultiLayeredClassifier();
            multiClassifier.FromLinearTrees(classifiers, splitClasses);
            multiClassifier.StoreToFile(MergedClassifierNameTextBox.Text);
            MultiLayeredClassifier classi = new MultiLayeredClassifier();
            classi.LoadFromFile(MergedClassifierNameTextBox.Text);
            MessageBox.Show(classi.TestModel("../../../ClusterDescriptionGen/bin/Debug/testCollection.json").ToString());
        }

    }
}
