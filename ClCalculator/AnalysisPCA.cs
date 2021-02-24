using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Accord.Statistics.Analysis;
using Accord.Math.Comparers;
using Accord.Statistics;
using Accord.Math;
using Accord.Math.Decompositions;
//using System.Windows.Forms.DataVisualization.Charting;
using Accord.Controls;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ClustersProcessor")]


namespace ClusterCalculator
{
    public class AnalysisPCA
    {
        const int inputDimension = 2;
        private double[,] ToDoublePoints(IList<PixelPoint> points)
        {
            double[,] doublePoints = new double[points.Count, inputDimension];
            for (int i = 0; i < points.Count; i++)
            {
                doublePoints[i, 0] = points[i].xCoord;
                doublePoints[i, 1] = points[i].yCoord;
            }
            return doublePoints;
        }
        public double[,] Transform(IList<PixelPoint> points)
        {
            double[,] data = ToDoublePoints(points);

            // Step 2. Subtract the mean
            double[] mean = data.Mean(0);
            double[,] dataAdjust = data.Subtract(mean,VectorType.RowVector);
            // Step 3. Calculate the covariance matrix
            double[,] cov = dataAdjust.Covariance();
            // Step 4. Calculate the eigenvectors and
            // eigenvalues of the covariance matrix
            var evd = new EigenvalueDecomposition(cov);
            double[] eigenvalues = evd.RealEigenvalues;
            double[,] eigenvectors = evd.Eigenvectors;

            // Sort eigenvalues and vectors in descending order
            eigenvectors = Matrix.Sort(eigenvalues, eigenvectors,
            new GeneralComparer(ComparerDirection.Descending, true));
            // Select all eigenvectors
            double[,] featureVector = eigenvectors;
            // Step 6. Deriving the new data set
            double[,] finalData =  Matrix.Dot(dataAdjust, featureVector);
            return finalData;
            
        }
        public PointD3[] To3DPoints(double[,] finalData)
        {
            PointD3[] array3D = new PointD3[finalData.Rows()];
            for (int i = 0; i < finalData.Rows(); i++)
            {
                array3D[i] = new PointD3((float)finalData[i, 0], (float)finalData[i, 1], 0); 
            }
            return array3D;
        }
    }
}
