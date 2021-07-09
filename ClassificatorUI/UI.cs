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
using System.IO;
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
        /// <summary>
        /// handles browsing the files with .csf suffix
        /// </summary>
        public void BrowseClassifiersClicked(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Trained simple classifiers(*.csf)|*.csf";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {

                if (pressedButton == BrowseRootModelButton)
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
                else if (pressedButton == BrowseTrainedModelButton)
                {
                    TrainedModelTextBox.Text = fileDialog.FileName;
                }
            }
        }
        /// <summary>
        /// handles browsing the files with .json suffix
        /// </summary>
        public void BrowseJsonFieldsClicked(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "json file (*.json)|*.json";
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
                else if (pressedButton == BrowseTrainedModelButton)
                {
                    TrainedModelTextBox.Text = fileDialog.FileName;
                }
            }
        }
        /// <summary>
        /// Handles training of the new classifier
        /// </summary>
        public void TrainSimpleClassifierClicked(object sender, EventArgs e)
        {
            ITrainableClassifier classifier;
            if (TrainedModelTextBox.Text == "")
                classifier = TrainableClassifierFactory.CreateNew("defaultMLP");
            else
            {
                classifier = new NNClassifier();
                try
                {
                    classifier.LoadFromFile(TrainedModelTextBox.Text);
                }
                catch
                {
                    Console.WriteLine("Error - selected trained model is not in correct format");
                    return;
                }
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
            //staring the training process in a new thread
            Thread thread = new Thread(() => 
            {
                while (iterationCount < maxRepetCount && accuracy <= minAccuracy)
                {
                    try
                    {
                        accuracy = classifier.Train(ClassifierConfigTextBox.Text, TrainJsonFileTextBox.Text, ref TrainingStopped, minAccuracy, seed);
                        iterationCount++;
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error - Training or config data do not exist or are inaccessible");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
                MessageBox.Show("The training process has successfully ended");
                TrainingStopped = false;
            });
            thread.Start();
        }
        /// <summary>
        /// Handles Merging multiple classifiers
        /// </summary>
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
                try
                {
                    simpleClassifier = new NNClassifier();
                    simpleClassifier.LoadFromFile(classifierPaths[index]);
                    classifiers.Add(simpleClassifier);
                    if (index > 0)
                        splitClasses.Add(splitClassesInBox[index - 1]);
                    index++;
                }               
                catch 
                {
                    Console.WriteLine($"Error - classifier {index} could not be loaded");
                    return;
                }
            }       

            MultiLayeredClassifier multiClassifier = new MultiLayeredClassifier();
            try
            {
                multiClassifier.FromLinearTrees(classifiers, splitClasses);
            }
            catch (ArgumentException)
            {
                MessageBox.Show($"Error - Classifiers cannot be combined because the specified split classes do not match the classes of the actual classifiers");
                return;
            }
            try
            {
                multiClassifier.StoreToFile(MergedClassifierNameTextBox.Text + ".csf");
            }
            catch 
            {
                MessageBox.Show($"Error - classifier cannot be stored into a selected file");
                return;
            }
            MessageBox.Show("Classifiers successfully combined");

        }
        public void StopButtonClicked(object sender, EventArgs e)
        {
            TrainingStopped = true;
        }
    }
}
