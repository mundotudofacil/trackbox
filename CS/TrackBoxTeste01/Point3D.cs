using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackBoxTeste01
{
    class Point3D
    {
        protected double m_x;
        protected double m_y;
        protected double m_z;

        public Point3D(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        public double Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }

        public double Z
        {
            get
            {
                return m_z;
            }
            set
            {
                m_z = value;
            }
        }

        public Point3D RotateX(int angle)
        {
            double rad;
            double cosa;
            double sina;
            double yn;
            double zn;

            rad = angle * Math.PI / 180;
            cosa = Math.Cos(rad);
            sina = Math.Sin(rad);
            yn = this.Y * cosa - this.Z * sina;
            zn = this.Y * sina + this.Z * cosa;
            return new Point3D(this.X, yn, zn);
        }

        public Point3D RotateY(int angle)
        {
            double rad;
            double cosa;
            double sina;
            double Xn;
            double Zn;

            rad = angle * Math.PI / 180;
            cosa = Math.Cos(rad);
            sina = Math.Sin(rad);
            Zn = this.Z * cosa - this.X * sina;
            Xn = this.Z * sina + this.X * cosa;

            return new Point3D(Xn, this.Y, Zn);
        }

        public Point3D RotateZ(int angle)
        {
            double rad;
            double cosa;
            double sina;
            double Xn;
            double Yn;

            rad = angle * Math.PI / 180;
            cosa = Math.Cos(rad);
            sina = Math.Sin(rad);
            Xn = this.X * cosa - this.Y * sina;
            Yn = this.X * sina + this.Y * cosa;
            return new Point3D(Xn, Yn, this.Z);
        }

        public Point3D Project(double viewWidth, double viewHeight, double fov, double viewDistance) 
        {
            double factor;
            double Xn;
            double Yn;
            factor = fov / (double)(viewDistance + this.Z);
            Xn = this.X * factor + viewWidth / (double)2;
            Yn = this.Y * factor + viewHeight / (double)2;
            return new Point3D(Xn, Yn, this.Z);
        }
    }
}
