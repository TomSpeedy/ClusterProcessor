using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using ClusterCalculator;
using System.Threading;
using System.Globalization;

namespace ClusterDescriptionGen
{
    public partial class DescrUI : Form
    {
        long ProcessedCount  = 0;
        bool Stopped = false;
        bool Done = false;
        System.Windows.Forms.Timer CheckProgressTimer = new System.Windows.Forms.Timer();
        /// <summary>
        /// event to update processed clusters count
        /// </summary>
        public void TimerTicked(object sender, EventArgs e)
        {
            ProcessedCountLabel.Text = "Clusters processed: " + ProcessedCount.ToString();
            if (Done)
            {
                EnableButtonsAfterProcessing();
                CheckProgressTimer.Stop();
                Done = false;
            }

        }
        private IDescriptionWriter ClDescriptionWriter { get; set; }

        public DescrUI()
        {
            InitializeComponent();
            SelectedInputListView.View = View.Details;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            
        }       
        public void BrowseProcessButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Ini files (*.ini)|*.ini";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
                const string noClass = "<none>";
                foreach (var fileName in fileDialog.FileNames)
                {
                    var selectedFileClassPair = new ListViewItem(noClass);
                    selectedFileClassPair.SubItems.Add(fileName);
                    SelectedInputListView.Items.Add(selectedFileClassPair);
                }
                    
            }
        }
        public void BrowseConfigButtonClicked(object sender, EventArgs e)
         {
             using (var dialog = new FolderBrowserDialog())
             {
                 if (dialog.ShowDialog() == DialogResult.OK)
                 {
                    for(int i = 0; i < SelectedInputListView.Items.Count; ++i)
                    {
                        if(SelectedInputListView.Items[i].SubItems.Count < 3)
                        {
                            //TODO check if folder contains a.txt ...
                            SelectedInputListView.Items[i].SubItems.Add(dialog.SelectedPath + '\\');
                            break;
                        }

                    }
                }
             }
         }   
        
        public void RemoveSelectedButtonClicked(object sender, EventArgs e)
        {

                for (int i = SelectedInputListView.SelectedItems.Count - 1; i >= 0; i--)
                    SelectedInputListView.Items.Remove(SelectedInputListView.SelectedItems[i]);
        }
        public bool CheckInput()
        {
            if (SelectedInputListView.Items.Count == 0)
            {
                MessageBox.Show("Nothing to process - no files have been selected");
                return false;
            }
            if (AttributeCheckedList.CheckedItems.Count == 0)
            {
                MessageBox.Show("Nothing to process - no attributes have been ticked");
                return false;
            }
            if (OutputTextbox.Text == "")
            {
                MessageBox.Show("Cannot start processing - please name the output file");
                return false;
            }
            return true;
        }
        public void ProcessButtonClicked(object sender, EventArgs e)
        {
            //parse the data from the form
            ProcessedCount = 0;
            if (!CheckInput())
                return;
            string[] iniFiles = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[1].Text).ToArray();
            string[] classes = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => '"' + item.SubItems[0].Text + '"').ToArray();
            bool classesProportional = UnevenDistrRadioButton.Checked;
            var endCond = GetEndCondition();
            bool parallelProcessing = ParallelClasPartProcessRadioButton.Checked;
            string allignBy = AllignClassTextBox.Text == "" ? null : '"' + AllignClassTextBox.Text + '"';
            IList<ClusterAttribute> attributes = new List<ClusterAttribute>();
            foreach (var checkedAttribute in AttributeCheckedList.CheckedItems)
            {
               attributes.Add(((string)checkedAttribute).ToAttribute());
            }
            double maxPartitionRead = 1;
            if(double.TryParse(UsedPartitionDataRatioTextBox.Text, out double maxPartReadParsed))
            {
                if(Math.Abs(maxPartReadParsed) <= 1)
                    maxPartitionRead = maxPartReadParsed;
            }
            Stopped = false;
            DisableButtonsForProcessing();
            //process the clusters in a different thread to stay responsive
            Thread processingThread = new Thread(() =>
                GenerateDescription(iniFiles, classes, classesProportional, endCond, allignBy,
                                    parallelProcessing, attributes, maxPartitionRead)) ;
            CheckProgressTimer.Interval = 1000;
            CheckProgressTimer.Tick += TimerTicked;
            CheckProgressTimer.Start();
            processingThread.Start();
            
            
        }
        private void DisableButtonsForProcessing()
        {
            foreach (Control control in Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.Enabled = false;
                }
                GroupBox groupBox = control as GroupBox;
                if (groupBox != null)
                    foreach (Control controlInGroupBox in groupBox.Controls)
                    {
                        Button buttonInGroupBox = controlInGroupBox as Button;
                        if (buttonInGroupBox != null)
                        {
                            buttonInGroupBox.Enabled = false;
                        }
                    }
            }
            StopProcessingButton.Enabled = true;
        }
        private void EnableButtonsAfterProcessing()
        {
            foreach (Control control in Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.Enabled = true;
                }
                GroupBox groupBox = control as GroupBox;
                if (groupBox != null)
                    foreach (Control controlInGroupBox in groupBox.Controls)
                    {
                        Button buttonInGroupBox = controlInGroupBox as Button;
                        if (buttonInGroupBox != null)
                        {
                            buttonInGroupBox.Enabled = true;
                        }
                    }
            }
        }
            public void CheckAllBoxesClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < AttributeCheckedList.Items.Count; i++)
            {
                AttributeCheckedList.SetItemChecked(i, true);
            }
        }
        public void StopProcessingClicked(object sender, EventArgs e)
        {
            Stopped = true;
        }
        private void GenerateDescription(string[] iniFiles, string[] classes, bool classesProportional,
            EndCondition endCond, string allignBy, bool parallelProcessing, IList<ClusterAttribute> attributes, double maxRead)
        {
            Dictionary<ClusterClassPartition, int> writtenCount = new Dictionary<ClusterClassPartition, int>();

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text + ".json"));
            IClusterReader clusterReader = new MMClusterReader();

            List<ClusterClassCollection> clusterEnumCollections = new List<ClusterClassCollection>();
            
            NeighbourCountFilter neighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);

            for (int i = 0; i < iniFiles.Length; i++)
            {
                clusterReader.GetTextFileNames(new StreamReader(iniFiles[i]), iniFiles[i], out string pxFile, out string clFile);
                var clCollection = new ClusterInfoCollection(new StreamReader(clFile), new StreamReader(pxFile));
                var existingClEnumCollections = clusterEnumCollections.FindAll(clusterEnColl => (clusterEnColl.Class == classes[i]));
                var newPartition = new ClusterClassPartition(clCollection, null);
                newPartition.MaxRead = maxRead;
                writtenCount.Add(newPartition, 0);
                if (existingClEnumCollections.Count == 0)
                    clusterEnumCollections.Add(new ClusterClassCollection(newPartition, classes[i]));
                else
                {
                    existingClEnumCollections[0].Partitions.Add(newPartition);
                }
            }
            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var attribute in attributes)
            {
                attributePairs.Add(attribute, null);
                attributesToGet.Add(attribute);
            }
            int clustersProcessedCount = 0; 
            int maxClusterCount = 10000000;
            Random random = new Random();
            IAttributeCalculator attrCalc = new DefaultAttributeCalculator();

            var probabilities = new double[clusterEnumCollections.Count];
            long totalLength = 0;
            
            for (int i = 0; i < clusterEnumCollections.Count; i++)
            {
                totalLength += clusterEnumCollections[i].CalcLength();
            }
            for (int i = 0; i < clusterEnumCollections.Count; i++)
            {
                if (i > 0)
                    probabilities[i] = probabilities[i - 1] + clusterEnumCollections[i].CalcLength() / (double)totalLength;
                else
                    probabilities[i] = clusterEnumCollections[i].CalcLength() / (double)totalLength;
            }
            while (clustersProcessedCount < maxClusterCount && !Stopped)
            {
                ProcessedCount++;
                var currentProb = random.NextDouble();
                var currentIndex = 0;
                if (classesProportional)
                    for (int i = 0; i < probabilities.Length; i++)
                    {
                        if (probabilities[i] >= currentProb)
                        {
                            currentIndex = i;
                            break;
                        }
                    }
                else
                    currentIndex = random.Next(0, clusterEnumCollections.Count);
                var clusterEnumCollection = clusterEnumCollections[currentIndex];                
                if (parallelProcessing)
                {
                    if (clusterEnumCollections.SelectNextEtorParallel(ref clusterEnumCollection, ref currentIndex, random, allignBy, endCond))
                        break;
                }
                else if (clusterEnumCollections.SelectNextEtorSequential(ref clusterEnumCollection, 
                    ref currentIndex, random, allignBy, endCond))
                    break;
                var currentClFile = clusterEnumCollection.Partitions[clusterEnumCollection.PartitionIndex].Collection.ClFile;
                attrCalc.Calculate(clusterEnumCollection, attributesToGet, ref clusterReader, ref attributePairs);
                writtenCount[clusterEnumCollection.Partitions[clusterEnumCollection.PartitionIndex]]++;
                clustersProcessedCount++;
                ClDescriptionWriter.WriteDescription(attributePairs);
            }
            ClDescriptionWriter.Close();
            Stopped = false;
            Done = true;
            MessageBox.Show("Processing successfully completed");
        }
        private EndCondition GetEndCondition()
        {
            var endCond = EndCondition.LastClass;
            if (FirstClassEndsRadioButton.Checked)
                endCond = EndCondition.FirstClass;
            else if (FirstPartitionEndsRadioButton.Checked)
                endCond = EndCondition.FirstPartition;
            return endCond;
        }


    }
    //extension class for iteration methods over the collection of partitions
    static class ClusterClassCollectionExtensions
    {
        public static bool SelectNextEtorParallel(this List<ClusterClassCollection> clEnumCollections, ref ClusterClassCollection currentClEnumCollection, ref int currentIndex, Random random, string allignBy, EndCondition endCond)
        {
            bool done = false;
            currentClEnumCollection.SetNewCurrentEnumerator(chooseRandomly: true);
            clEnumCollections[currentIndex] = currentClEnumCollection;
            
            while (!currentClEnumCollection.CurrentEnumerator.MoveNext() 
                || !currentClEnumCollection.Partitions[currentClEnumCollection.PartitionIndex].CheckPosition())
            {
                if (endCond == EndCondition.FirstPartition)
                    done = true;
                if (allignBy == null || allignBy == currentClEnumCollection.Class)
                {
                    currentClEnumCollection.CurrentEnumerator.Dispose();
                    
                    if (currentClEnumCollection.RemovePartition())
                    {
                        clEnumCollections[currentIndex] = currentClEnumCollection;
                        continue;
                    }

                    clEnumCollections.Remove(currentClEnumCollection);
                    if (endCond == EndCondition.FirstClass)
                        done = true;
                    if (clEnumCollections.Count == 0)
                    {
                        done = true;
                        break;
                    }
                    currentIndex = random.Next(0, clEnumCollections.Count);
                    currentClEnumCollection = clEnumCollections[currentIndex];
                }
                else
                {
                    currentClEnumCollection.Partitions[currentClEnumCollection.PartitionIndex].ResetEtor();
                    currentClEnumCollection.SetNewCurrentEnumerator(chooseRandomly: false);
                    clEnumCollections[currentIndex] = currentClEnumCollection;
                }
            }             
            return done;
        }

        public static bool SelectNextEtorSequential(this List<ClusterClassCollection> clEnumCollections, ref ClusterClassCollection currentClEnumCollection, ref int currentIndex, Random random, string allignBy, EndCondition endCond)
        {
            bool done = false;
            while (!currentClEnumCollection.CurrentEnumerator.MoveNext() || !currentClEnumCollection.Partitions[currentClEnumCollection.PartitionIndex].CheckPosition())
            {
                if (endCond == EndCondition.FirstPartition)
                    done = true;
                if (allignBy == null || allignBy == currentClEnumCollection.Class)
                {
                    currentClEnumCollection.CurrentEnumerator.Dispose();
                    if (currentClEnumCollection.RemovePartition(chooseRandomly: false))
                    {
                        clEnumCollections[currentIndex] = currentClEnumCollection;
                        continue;
                    }
                    
                    clEnumCollections.Remove(currentClEnumCollection);
                    if (endCond == EndCondition.FirstClass)
                        done = true;
                    if (clEnumCollections.Count == 0)
                    {
                        done = true;
                        break;
                    }
                    currentIndex = random.Next(0, clEnumCollections.Count);
                    currentClEnumCollection = clEnumCollections[currentIndex];
                }
                else
                {
                    currentClEnumCollection.Partitions[currentClEnumCollection.PartitionIndex].ResetEtor();
                    currentClEnumCollection.SetNewCurrentEnumerator(chooseRandomly: false);
                    clEnumCollections[currentIndex] = currentClEnumCollection;
                }
            }

            return done;
        }
    }
    /// <summary>
    /// condition where the computation should be finished
    /// </summary>
    public enum EndCondition
    {
        FirstClass, FirstPartition, LastClass
    }

}



