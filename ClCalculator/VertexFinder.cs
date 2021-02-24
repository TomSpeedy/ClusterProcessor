using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    public class VertexFinder
    {
        private ISkeletonizer Skeletonizer { get; }
        const int VertexNeighbourCount = 1;
        private NeighbourCountFilter NeighbourFilter { get; }
        public VertexFinder(Calibration calib)
        {
            Skeletonizer = new ThinSkeletonizer(new EnergyCalculator(calib));
            NeighbourFilter = new NeighbourCountFilter(neighbourCount => neighbourCount == VertexNeighbourCount, NeighbourCountOption.WithAllDiagonalNeighbours); //we also count diagonal neighbours
        }

        public List<PixelPoint> FindVertices(IList<PixelPoint> pixelPoints)
        {
            pixelPoints = Skeletonizer.SkeletonizePoints(pixelPoints);
            List<PixelPoint> result = NeighbourFilter.Process(pixelPoints);
            return result;
        }
    }
}
