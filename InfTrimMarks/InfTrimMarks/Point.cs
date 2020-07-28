
using System;

namespace InfTrimMarks
{
    struct Point : IEquatable<Point>
    {
        public readonly double X;
        public readonly double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point)) return false;

            var value = (Point)obj;

            return X == value.X && Y == value.Y;
        }

        public bool Equals(Point other) => X == other.X && Y == other.Y;

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + X.GetHashCode();
                result = result * 23 + Y.GetHashCode();
                return result;
            }
        }
    }
}
