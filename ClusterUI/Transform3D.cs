using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using ChartDirector;

namespace ClusterUI
{
    //TODO: Clean Redraw here and also in UI
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
        private readonly ThreeDScatterChart chart;
        private readonly double[] xData;
        private readonly double[] zData;
        private readonly double[] yData;
        public int angleVert { get; set; }
        public int angleHoriz { get; set; }
        const string Title = "3D Trajectory";
        //Name of demo module
        public string getName() { return Title; }

        //Number of charts produced in this demo module
        public int getNoOfCharts() { return 1; }

        //Main code for creating chart.
        //Note: the argument chartIndex is unused because this demo only has 1 chart.
        public ScatterChart(WinChartViewer viewer, Point3D[] points3D)
        {
            xData = points3D.Select(point => (double)point.X / 10D).ToArray(); 
            yData = points3D.Select(point => (double)point.Y / 10D).ToArray(); 
            zData = points3D.Select(point => (double)point.Z / 10D).ToArray(); 

            // Create a ThreeDScatterChart object of size 720 x 600 pixels
            var chart = new ThreeDScatterChart(viewer.Width, viewer.Height);
            chart.setPlotRegion((viewer.Width / 2), (viewer.Height / 2), 200, 200, 200);

            Initialize(chart);
            // Output the chart
            viewer.Chart = chart;
            //Save chart
            this.chart = chart;
        }
        public ThreeDScatterChart RotateChart(int angleVert, int angleHoriz)
        {
            var chart = new ThreeDScatterChart(this.chart.getWidth(), this.chart.getHeight());
            chart.setPlotRegion((this.chart.getWidth() / 2), (this.chart.getHeight() / 2), 200, 200, 200);
            //chart.setColorAxis(645, 270, Chart.Left, 200, Chart.Right);

            Initialize(chart);
            chart.setViewAngle(angleVert, angleHoriz);
            //viewer.ImageMap = chart.getHTMLImageMap("clickable", "",
            // "title='(x={x|p}, y={y|p}, z={z|p}'");
            return chart;
        }
        private void Initialize(ThreeDScatterChart chart)
        {
            const string TitleFont = "Times New Roman Italic";
            const string AxisLabelFont = "Arial Bold";
            const int AxisFontSize = 10;
            chart.addTitle(Title, TitleFont, 18);
            // Add a scatter group to the chart using 11 pixels glass sphere symbols, in which the
            // color depends on the z value of the symbol
            chart.addScatterGroup(this.xData, this.yData, this.zData, "Trajectory", ChartDirector.Chart.GlassSphere2Shape, 11,
                ChartDirector.Chart.SameAsMainColor);
            // Set the x, y and z axis titles using 10 points Arial Bold font
            chart.xAxis().setTitle("X-Axis", AxisLabelFont, AxisFontSize);
            chart.yAxis().setTitle("Y-Axis", AxisLabelFont, AxisFontSize);
            chart.zAxis().setTitle("Z-Axis", AxisLabelFont, AxisFontSize);
        }
    }

}
