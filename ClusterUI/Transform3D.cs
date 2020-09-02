using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using ChartDirector;

namespace ClusterUI
{
    interface IZCalculator
    {
        Point3D[] TransformPoints(Cluster cluster);
        double CalculateZ(PixelPoint point);
    }
    class ZCalculator : IZCalculator
    {
        //Set default: 500 um, 110 V depl, bias: 230 V, mob: 45 V s / um2
        private Configuration Configuration { get; }
        private Cluster Cluster { get; set; }
        public ZCalculator() 
        {
            Configuration = new Configuration(thickness: 500d, mobility: 45d, ud: 110d, ub: 230d);
        }
        public Point3D[] TransformPoints(Cluster cluster)
        {
            Cluster = cluster;
            var points3D = new Point3D[cluster.Points.Length];
            for (int i = 0; i < points3D.Length; i++)
            {
                var z = CalculateZ(cluster.Points[i]);
                points3D[i] = new Point3D(cluster.Points[i].xCoord, cluster.Points[i].yCoord, (float)z);
            }
            return points3D;
        }
        public double CalculateZ(PixelPoint point)
        {
            var relativeToA = point.ToA - Cluster.FirstToA;
            return (Configuration.Thick / Configuration.Ud) * (Configuration.Ud + Configuration.Ub) *
                (1 - Math.Exp(2 * Configuration.Ud * Configuration.Mob * relativeToA / Math.Pow(Configuration.Thick, 2)));
                //Configuration.Mob * Configuration.Ub / Configuration.Thick
        }
    }
    struct Configuration
    {
        public double Thick { get; }
        public double Mob { get; }
        public double Ud { get; }
        public double Ub {get;}
        public Configuration(double thickness, double mobility, double ud, double ub)
        {
            Thick = thickness;
            Mob = mobility;
            Ud = ud;
            Ub = ub;
        }
    }
    public class ScatterChart
    {
        //Name of demo module
        public string getName() { return "3D Scatter Chart "; }

        //Number of charts produced in this demo module
        public int getNoOfCharts() { return 1; }

        //Main code for creating chart.
        //Note: the argument chartIndex is unused because this demo only has 1 chart.
        public void CreateChart(WinChartViewer viewer, Point3D[] points3D)
        {
            // The XYZ data for the 3D scatter chart as 3 random data series
            RanSeries r = new RanSeries(0);
            double[] xData = points3D.Select(point => (double)point.X/10D).ToArray(); //r.getSeries2(100, 100, -10, 10);
            double[] yData = points3D.Select(point => (double)point.Y/10D).ToArray(); //r.getSeries2(100, 0, 0, 20);
            double[] zData = points3D.Select(point => (double)point.Z/10D).ToArray(); //r.getSeries2(100, 100, -10, 10);

            // Create a ThreeDScatterChart object of size 720 x 600 pixels
            ThreeDScatterChart chart = new ThreeDScatterChart(720, 600);

            // Add a title to the chart using 20 points Times New Roman Italic font
            chart.addTitle("3D Scatter Chart", "Times New Roman Italic", 20);

            // Set the center of the plot region at (350, 280), and set width x depth x height to
            // 360 x 360 x 270 pixels
            chart.setPlotRegion(300, 200, 256, 256, 256);

            // Add a scatter group to the chart using 11 pixels glass sphere symbols, in which the
            // color depends on the z value of the symbol
            chart.addScatterGroup(xData, yData, zData, "Trajectory", ChartDirector.Chart.GlassSphere2Shape, 11,
                ChartDirector.Chart.SameAsMainColor);

            // Add a color axis (the legend) in which the left center is anchored at (645, 270). Set
            // the length to 200 pixels and the labels on the right side.
            //chart.setColorAxis(645, 270, Chart.Left, 200, Chart.Right);

            // Set the x, y and z axis titles using 10 points Arial Bold font
            chart.xAxis().setTitle("X-Axis", "Arial Bold", 10);
            chart.yAxis().setTitle("Y-Axis", "Arial Bold", 10);
            chart.zAxis().setTitle("Z-Axis", "Arial Bold", 10);

            // Output the chart
            viewer.Chart = chart;

            //include tool tip for the chart
            viewer.ImageMap = chart.getHTMLImageMap("clickable", "",
                "title='(x={x|p}, y={y|p}, z={z|p}'");
        }
    }

}
