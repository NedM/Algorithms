using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public struct Point : IComparable, IEquatable<Point>
    {
        private double x;
        private double y;

        public static double CalculateEuclidianDistance(Point p1, Point p2)
        {
            //SQRT((x1 - x2)^2 + (y1 - y2)^2)
            return Math.Sqrt(Math.Pow((p1.X - p2.X), 2.0) + Math.Pow((p1.Y - p2.Y), 2.0));
        }

        public static Point Origin { get { return new Point(0, 0); } }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X { get { return this.x; } set { this.x = value; } }
        public double Y { get { return this.y; } set { this.y = value; } }

        public override string ToString()
        {
            return string.Format("({0}, {1})", this.X, this.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return this.Equals((Point)obj);
            }
            else
            {
                return false;
            }
        }

        public double CalculateEuclidianDistance(Point other)
        {
            return Point.CalculateEuclidianDistance(this, other);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() + this.Y.GetHashCode();
        }

        /// <summary>
        /// Define comparison on the basis of which point is closer to the origin
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if(!(obj is Point))
            {
                throw new ArgumentException(string.Format("Comparison is not defined for Point and {0}", obj.GetType().ToString()));
            }
            return this.CompareTo((Point)obj);
        }

        /// <summary>
        /// Define comparison on the basis of which point is closer to the origin
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Point other)
        {
            double distFromOriginForThis = Point.CalculateEuclidianDistance(this, Point.Origin);
            double distFromOriginForOther = Point.CalculateEuclidianDistance(other, Point.Origin);
            return distFromOriginForThis.CompareTo(distFromOriginForOther);
        }

        public bool Equals(Point other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }
    }
}
