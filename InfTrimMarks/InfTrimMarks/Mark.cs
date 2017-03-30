using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfTrimMarks
{
    class Mark
    {
        public double StartX { get { return startX; } }
        public double StartY { get { return startY; } }
        public double EndX { get { return (startX + height * direction.X); } }
        public double EndY { get { return (startY + height * direction.Y); } }
        public double Height { get { return height; } set { height = value; } }

        private double startX;
        private double startY;
        private double height;
        private System.Windows.Vector direction;

        public Mark(double startX, double startY, System.Windows.Vector direction, double height)
        {
            this.startX = startX;
            this.startY = startY;
            this.height = height;
            this.direction = direction;
        }
    }
}
