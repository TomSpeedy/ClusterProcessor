using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterCalculator
{
    /// <summary>
    /// serves as a class for finding vertex of a cluster after skeletonization
    /// </summary>
    public class VertexFinder
    {
        private ISkeletonizer Skeletonizer { get; }
        const int VertexNeighbourCount = 1;
        private NeighbourCountFilter NeighbourFilter { get; }
        public VertexFinder()
        {
            Skeletonizer = new ThinSkeletonizer();
            NeighbourFilter = new NeighbourCountFilter(neighbourCount => neighbourCount <= VertexNeighbourCount, NeighbourCountOption.Base); //we also count diagonal neighbours
        }

        public List<PixelPoint> FindVertices(IList<PixelPoint> pixelPoints)
        {
            pixelPoints = Skeletonizer.SkeletonizePoints(pixelPoints);
            List<PixelPoint> result = NeighbourFilter.Process(pixelPoints);
            return result;
        }
    }
}
