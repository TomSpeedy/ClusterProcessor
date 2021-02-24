using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClusterCalculator
{
    //TODO: Clean Redraw here and also in UI
    public interface IZCalculator
    {
        PointD3[] TransformPoints(Cluster cluster);
        double CalculateZ(PixelPoint point);
    }
    public class ZCalculator : IZCalculator
    {
        //Set default: 500 um, 110 V depl, bias: 230 V, mob: 45 V s / um2
        private Configuration Configuration { get; }
        private Cluster Cluster { get; set; }
        public ZCalculator() 
        {
            Configuration = new Configuration(thickness: 500d, mobility: 45d, ud: 110d, ub: 230d);
        }
        public PointD3[] TransformPoints(Cluster cluster)
        {
            Cluster = cluster;
            var points3D = new PointD3[cluster.Points.Length];
            for (int i = 0; i < points3D.Length; i++)
            {
                var z = CalculateZ(cluster.Points[i]);
                points3D[i] = new PointD3(cluster.Points[i].xCoord, cluster.Points[i].yCoord, (float)z);
            }
            return points3D;
        }
        public double CalculateZ(PixelPoint point)
        {
            var relativeToA = point.ToA - Cluster.FirstToA;
            return (Configuration.Thick / (2 * Configuration.Ud)) * (Configuration.Ud + Configuration.Ub) *
                (1 - Math.Exp((-2) * Configuration.Ud * Configuration.Mob * relativeToA / Math.Pow(Configuration.Thick, 2)));
        }
    }
    public struct PointD3
    {
        public float X;
        public float Y;
        public float Z;
        public PointD3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
    

}
