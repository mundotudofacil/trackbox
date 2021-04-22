using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO.Ports;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;


namespace LuvaIMU
{
    class KalmanFilter2D
    {
        public Matrix<double> state;


        Emgu.CV.KalmanFilter kal;
        public KalmanFilter2D()
        {

            kal = new Emgu.CV.KalmanFilter(4, 2, 0, DepthType.Cv64F);
            state = new Matrix<double>(new double[]
            {
                    0.0d, 0.0d
            });
            kal.Correct(state.Mat);

            //Debug.WriteLine(processNoise[0, 0]);

            kal.TransitionMatrix.SetTo<double>(new double[] {
                1, 0, .5, 0,
                0, 1, 0, .5,
                0, 0, 1, 0,
                0, 0, 0, 1
            });
            kal.MeasurementNoiseCov.SetTo<double>(new double[] {
                0.1, 0,
                0, 0.1
            });
            kal.ProcessNoiseCov.SetTo(new double[] {
                0.0001, 0     , 0     , 0     ,
                0     , 0.0001, 0     , 0     ,
                0     , 0     , 0.0001, 0     ,
                0     , 0     , 0     , 0.0001
            });
            
            kal.ErrorCovPost.SetTo(new double[] {
                .1, 0, 0, 0,
                0, .1, 0, 0,
                0, 0, .1, 0,
                0, 0, 0, .1
            });
            kal.MeasurementMatrix.SetTo(new double[] {
                1, 0, 0, 0,
                0, 1, 0, 0
            });

        }


        public Point filterPoints(Point pt)
        {
            state = new Matrix<double>(new double[]{0.0, 0.0});
            state[0, 0] = pt.X;
            state[1, 0] = pt.Y;
            kal.Correct(state.Mat);
            Matrix<double> prediction = Mat_to_Matrix(kal.Predict());
            Point predictPoint = new Point(prediction[0, 0], prediction[1, 0]);
            //Point measurePoint = new Point(0, 0);
            //syntheticData.GoToNextState();
            // PointF[ results = new PointF[2];
            //results[0] = predictPoint;
            //results[1] = estimatedPoint;
            //px = predictPoint.X;
            //py = predictPoint.Y;
            //cx = estimatedPoint.X;
            //cy = estimatedPoint.Y;
            return predictPoint;
        }

        Matrix<double> Mat_to_Matrix(Mat mat)
        {
            Matrix<double> matrix = new Matrix<double>(mat.Rows, mat.Cols, mat.NumberOfChannels);
            mat.CopyTo(matrix);
            return matrix;
        }

        Mat Matrix_to_Mat(Matrix<double> matrix)
        {
            Mat mat = new Mat(4, 1, DepthType.Cv32F, matrix.Mat.NumberOfChannels);
            matrix.Mat.ConvertTo(mat, DepthType.Cv32F);
            return mat;
        }
    }
}
