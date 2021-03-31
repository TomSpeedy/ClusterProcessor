using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ClusterFilter;
using ClusterCalculator;
using System.Threading;
using System.Globalization;
namespace ClusterDescriptionGen
{
    public partial class Form1 : Form
    {
        private const string configPath = "../../../config/calib_files_fe/";
        private IDescriptionWriter ClDescriptionWriter { get; set; }
        
        public Form1()
        {
            InitializeComponent();
            SelectedInputListView.View = View.Details;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }
        public void CopyLastPathButtonClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectedInputListView.Items.Count; ++i)
            {
                if (SelectedInputListView.Items[i].SubItems.Count < 3)
                {
                    //TODO check if folder contains a.txt ...
                    SelectedInputListView.Items[i].SubItems.Add(SelectedInputListView.Items[i - 1].SubItems[2]);
                    break;
                }

            }
        }
        public void BrowseProcessButtonClicked(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
                //InputTextbox.Text += string.Join(";",fileDialog.FileNames);
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
        
        
        public void ProcessButtonClicked(object sender, EventArgs e)
        {
            

            ClDescriptionWriter = new JSONDecriptionWriter(new StreamWriter(OutputTextbox.Text));
            IClusterReader clusterReader  = new MMClusterReader();
        string[] iniFiles = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[1].Text).ToArray();
            string[] classes = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[0].Text).ToArray();
            //string[] configDirs = SelectedInputListView.Items.Cast<ListViewItem>().Select(item => item.SubItems[2].Text).ToArray();
            //string[] pxFiles = new string[iniFiles.Length];
            List <ClusterClassCollection> clusterEnumCollections = new List<ClusterClassCollection>();

            NeighbourCountFilter neighbourCountFilter = new NeighbourCountFilter(nCount => nCount >= 3, NeighbourCountOption.WithYpsilonNeighbours);

            for (int i = 0; i < iniFiles.Length; i++ )
            {
                clusterReader.GetTextFileNames(new StreamReader(iniFiles[i]), iniFiles[i], out string pxFile, out string clFile);
                var clCollection = new ClusterInfoCollection(new StreamReader(clFile), new StreamReader(pxFile));

                var existingClEnumCollections = clusterEnumCollections.FindAll(clusterEnColl => (clusterEnColl.Class == classes[i]));
                var newPartition = new ClusterClassPartition(clCollection, null);
                if (existingClEnumCollections.Count == 0)
                  clusterEnumCollections.Add(new ClusterClassCollection( newPartition, classes[i]));
                else
                {
                    existingClEnumCollections[0].Partitions.Add(newPartition);
                }
            }

            var attributePairs = new Dictionary<ClusterAttribute, object>();
            IList<ClusterAttribute> attributesToGet = new List<ClusterAttribute>();
            foreach (var checkedAttribute in AttributeCheckedList.CheckedItems)
            {
                var attributeName = ((string)checkedAttribute).ToAttribute();
                attributePairs.Add(attributeName, null);
                attributesToGet.Add(attributeName);
            }
            Cluster current;
            int clustersProcessedCount = 0; //remove
            int maxClusterCount = 1000000;
            int minimalClassCount = clusterEnumCollections.Count;
            Random random = new Random();
            IAttributeCalculator attrCalc = new DefaultAttributeCalculator();
            while (clustersProcessedCount < maxClusterCount && clusterEnumCollections.Count >= minimalClassCount)
            {
                var currentIndex = random.Next(0, clusterEnumCollections.Count);
                var clusterEnumCollection = clusterEnumCollections[currentIndex];
                              
                if (clusterEnumCollections.SelectNextClusterEnum(ref clusterEnumCollection, ref currentIndex, random))
                    break;
                attrCalc.Calculate(clusterEnumCollection, attributesToGet, ref clusterReader, ref attributePairs);
                clustersProcessedCount++;
                
                    ClDescriptionWriter.WriteDescription(attributePairs);
                    //if (clustersProcessedCount > 1000) break;
                //}
            }
            ClDescriptionWriter.Close();
        }
    }
    static class ClusterClassCollectionExtensions
        {
            public static bool SelectNextClusterEnum(this List<ClusterClassCollection> clEnumCollections, ref ClusterClassCollection currentClEnumCollection, ref int currentIndex, Random random)
        {
            bool done = false;
            currentClEnumCollection.SetNewCurrentEnumerator();
            clEnumCollections[currentIndex] = currentClEnumCollection;
            while (!currentClEnumCollection.CurrentEnumerator.MoveNext())
            {
                currentClEnumCollection.CurrentEnumerator.Dispose();
                if (currentClEnumCollection.RemovePartition())
                {
                    clEnumCollections[currentIndex] = currentClEnumCollection;
                    continue;
                }
                clEnumCollections.Remove(currentClEnumCollection);
                if (clEnumCollections.Count == 0)
                {
                    done = true;
                    break;
                }
                currentIndex = random.Next(0, clEnumCollections.Count);
                currentClEnumCollection = clEnumCollections[currentIndex];
                //currentClEnumCollection.Enumerator = currentClEnumCollection.Collection.GetEnumerator();
            }
            return done;
        }
        }
    }



