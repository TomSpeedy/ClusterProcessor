### **Cluster Processor**

**Description**

Cluster Processor is an application for processing data from TimePix3 detector. The application consists of two main parts - viewer and filters. 

//TODO

**Using**

The application has its GUI - graphical user interface which provides a simple way to use the app.

//TODO

**Work on the project**

*Start of the project*

As I consider my primary programming language to be C#, I decided to use windows forms application would be ideal for the purpose of creating a simple GUI over a code in C#. Firstly, when I chose this topic I was excited but also a little bit worried. I hadn't heard about TimePix detectors before but Lukas Meduna and Frantisek Mraz gave me really good introduction to the principles of timepix detectors and their work. I decided to use Lukas' application for timepix data clustering and I implemented a simple cluster viewer which loaded given file in xxx format and displayed a first cluster image. Image represented the visualization of Time over Threshold property for each pixel and it was possible to navigate through collection of these cluster by using buttons Next and Previous.

*Cluster image colour*

The little problem I encountered here was deciding the approperiate mapping of ToT to colour of each pixel. At first I chose a linear scale mapping to spectrum from yellow to red and as I compared the images with logarithmic mapping I found out that logarithmic mapping displayed a better distinction between possible 'real' trajectory of particle and 'noise'. 

*Cluster loading*

I also decided It is a good idea not to load all clusters into memory as the size of the data might be really huge and could potentially cause 'swapping'. That would cause significant performance issues which means that when user clicks the 'next' button program actually traverses the file to find and display current cluster which saves memory and time even despite potentially linear search time for the cluster in .cl file (creates new Cluster object). Searching in .px file is implementing by .Seek() method which is sufficiently fast for this purpose.

*Filters - abstract class vs interview*

For the filters I chose a different approach. Because of the fact that creating the Cluster object is 'time and space expensive' if done on huge collection of Clusters, I chose to sequentially read data for each cluster in .cl file (that is done in the method .Process()) and for each line in .cl file a method .MatchesFilter() is called (implemented differently for each specific filter). This way each filter is provided by reasonably fast iterating algorithm and only implement its own condition of accepting the cluster. (the reason why I preferred an abstract class over an interface)

*Multifilter*

Then I found out that using more filters would cause more iterations over the collection of clusters which is unnecessary. So I created a special type of filter which gets all filters we want to apply in constructor and then for each .cl line it applies all of its filters .MatchesFilter() methods with lazy evaluation. Lazy evaluation means that as soon as one of the filters returns false from .MatchesFilter() method, the program directly moves to the next line without evaluation of the remaining filters.

*Collection Histogram*

To get a better depiction of the collection of clusters which are loaded, I chose to make Histogram displaying the value of property of the clusters. X axis is scaled relatively to the data (requires one iteration over the collection) and is divided into constant number of intervals (= 100). I decided that the calculation of the histogram will be done right when they are loaded as this provides some more time for calculation on background until user click to show histogram. This way I could find out the distribution of the given attribute in the collection of clusters. Setting the attribute to pixelCount I found out that most of the clusters are really small <10 pixels and bigger clusters are really rare. 

 //TODO Implementation with partial and full Cluster.LoadCluster() method

*Pixel Histogram*

Pixel histogram is a histogram of a pixels in a specific cluster (the one user is currently seeing). The calculation is done each time the Next or Previous buttons are clicked, to be ready when user decides to show this histogram. Histogram could be used as a property of a cluster that could be utilized during classification.

*3D Visualization*

//TODO