using System;
using Corel.Interop.VGCore;

namespace InfTrimMarks
{
    struct FigureBBox : IEquatable<FigureBBox>
    {
        /// <summary>
        /// Left edge
        /// </summary>
        public readonly double X;
        /// <summary>
        /// Bottom edge
        /// </summary>
        public readonly double Y;

        public readonly double Width;
        public readonly double Height;

        public double Top { get => Y + Height; }
        public double Bottom { get => Y; }
        public double Left { get => X; }
        public double Right { get => X + Width; }
        public double CenterX { get => X + Width / 2; }
        public double CenterY { get => Y + Height / 2; }

        public FigureBBox(Rect rect)
        {
            X = Math.Round(rect.x, 2);
            Y = Math.Round(rect.y, 2);
            Width = Math.Round(rect.Width, 2);
            Height = Math.Round(rect.Height, 2);
        }

        public FigureBBox(double x, double y, double width, double height)
        {
            X = Math.Round(x, 2);
            Y = Math.Round(y, 2);
            Width = Math.Round(width, 2);
            Height = Math.Round(height, 2);
        }

        public bool IsPointInside(double x, double y) => (x > X && x < Right) && (y > Y && y < Top);

        public bool IsPointInside(Point p) => (p.X > X && p.X < Right) && (p.Y > Y && p.Y < Top);

        public bool Intersect(Rect rect)
        {
            if (Math.Abs(CenterX - rect.CenterX) > (Width / 2 + rect.Width / 2))
                return false;
            if (Math.Abs(CenterY - rect.CenterY) > (Height / 2 + rect.Height / 2))
                return false;
            return true;
        }

        public FigureBBox Union(FigureBBox bBox)
        {
            var x = X < bBox.X ? X : bBox.X;
            var y = Y < bBox.Y ? Y : bBox.Y;
            var width = (Right > bBox.Right ? Right : bBox.Right) - x;
            var height = (Top > bBox.Top ? Top : bBox.Top) - y;
            return new FigureBBox(x, y, width, height);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FigureBBox))
                return false;

            var value = (FigureBBox)obj;

            return X == value.X && Y == value.Y && Width == value.Width && Height == value.Height;
        }

        public bool Equals(FigureBBox other) =>
            X == other.X &&
            Y == other.Y &&
            Width == other.Width &&
            Height == other.Height;

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + X.GetHashCode();
                result = result * 23 + Y.GetHashCode();
                result = result * 23 + Width.GetHashCode();
                result = result * 23 + Height.GetHashCode();
                return result;
            }
        }
    }
}
