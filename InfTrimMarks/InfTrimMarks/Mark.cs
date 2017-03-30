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
        public double EndX { get { return (startX + height * directionX); } }
        public double EndY { get { return (startY + height * directionY); } }
        public double Height { get { return height; } set { height = value; } }

        private double startX;
        private double startY;
        private double height;
        private int directionX;
        private int directionY;

        public Mark(double startX, double startY, int directionX, int directionY, double height)
        {
            this.startX = startX;
            this.startY = startY;
            this.height = height;
            this.directionX = directionX;
            this.directionY = directionY;
        }
    }
}
