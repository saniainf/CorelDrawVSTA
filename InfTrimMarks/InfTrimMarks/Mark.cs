
using System;

namespace InfTrimMarks
{
    struct Mark : IEquatable<Mark>
    {
        public readonly Point StartPoint;
        public readonly Point EndPoint;

        public Mark(Point start, Point end)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Mark)) return false;

            var value = (Mark)obj;

            return (StartPoint.Equals(value.StartPoint) &&
                EndPoint.Equals(value.EndPoint));
        }

        public bool Equals(Mark other) =>
            (StartPoint.Equals(other.StartPoint) &&
            EndPoint.Equals(other.EndPoint));

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + StartPoint.GetHashCode();
                result = result * 23 + EndPoint.GetHashCode();
                return result;
            }
        }
    }
}
