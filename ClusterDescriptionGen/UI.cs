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
    public partial class UI : Form
    {
        long ProcessedCount = 0;
        System.Windows.Forms.Timer CheckProgressTimer = new System.Windows.Forms.Timer();
        public void TimerTicked(object sender, EventArgs e)
        {
            ProcessedCountLabel.Text = "Clusters processed: " + ProcessedCount.ToString();
        }
        private IDescriptionWriter ClDescriptionWriter { get; set; }

        public UI()
        {
            InitializeComponent();
            SelectedInputListView.View = View.Details;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            
        }       
        public void BrowseProcessButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
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
        public void ProcessButtonClicked(object sender, EventArgs e)
        {

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
            Thread processingThread = new Thread(() =>  GenerateDescription(ref ProcessedCount, iniFiles, classes, classesProportional, endCond, allignBy, parallelProcessing, attributes, maxPartitionRead) );
            CheckProgressTimer.Interval = 1000;
            CheckProgressTimer.Tick += TimerTicked;
            CheckProgressTimer.Start();
            processingThread.Start();
            
            
        }
        public void CheckAllBoxesClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < AttributeCheckedList.Items.Count; i++)
            {
                AttributeCheckedList.SetItemChecked(i, true);
            }
        }
        private void GenerateDescription(ref long processedCount, string[] iniFiles, string[] classes, bool classesProportional,
            EndCondition endCond, string allignBy, bool parallelProcessing, IList<ClusterAttribute> attributes, double maxRead)
        {
            Dictionary<ClusterClassPartition, int> writtenCount = new Dictionary<ClusterClassPartition, int>();

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text));
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
            int clustersProcessedCount = 0; //remove
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
            while (clustersProcessedCount < maxClusterCount)
            {
                processedCount++;
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
                //var progress = clusterEnumCollection.Partitions.Select(partition => partition.Collection.ClFile.BaseStream.Position).Sum() / (double)clusterEnumCollection.CalcLength();
                
                if (parallelProcessing)
                {
                    if (clusterEnumCollections.SelectNextEtorParallel(ref clusterEnumCollection, ref currentIndex, random, allignBy, endCond))
                        break;
                }
                else if (clusterEnumCollections.SelectNextEtorSequential(ref clusterEnumCollection, ref currentIndex, random, allignBy, endCond))
                    break;
                var currentClFile = clusterEnumCollection.Partitions[clusterEnumCollection.PartitionIndex].Collection.ClFile;
                if (/*currentClFile.BaseStream.Position > currentClFile.BaseStream.Length * 0.9 && writtenCount[clusterEnumCollection.Partitions[clusterEnumCollection.PartitionIndex]] < 3000*/true)
                {
                    attrCalc.Calculate(clusterEnumCollection, attributesToGet, ref clusterReader, ref attributePairs);
                    writtenCount[clusterEnumCollection.Partitions[clusterEnumCollection.PartitionIndex]]++;
                    clustersProcessedCount++;
                    ClDescriptionWriter.WriteDescription(attributePairs);
                }
            }
            ClDescriptionWriter.Close();
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
                    //currentClEnumCollection = clEnumCollections[currentIndex];
                }
                //after class removal, chooose the nex class randomly

            }

            return done;
        }
    }
    public enum EndCondition
    {
        FirstClass, FirstPartition, LastClass
    }

}



