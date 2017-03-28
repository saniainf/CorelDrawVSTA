using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
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
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;
using System.Text.RegularExpressions;

namespace InfTrimMarks
{
    class SmartTrimMark
    {
        private corel.Application corelApp;

        public SmartTrimMark(corel.Application app)
        {
            this.corelApp = app;
        }

        public void doSmartTrimMark(double offset, double markHeight, ShapeRange sr)
        {
            corel.Rect rect = new corel.Rect();

            foreach (corel.Shape s in sr)
            {
                rect = s.BoundingBox;
                drawLine(sr, rect.Left + offset, rect.Bottom, rect.Left + offset, rect.Bottom - markHeight);
                drawLine(sr, rect.Right - offset, rect.Bottom, rect.Right - offset, rect.Bottom - markHeight);
                drawLine(sr, rect.Left, rect.Bottom + offset, rect.Left - markHeight, rect.Bottom + offset);
                drawLine(sr, rect.Left, rect.Top - offset, rect.Left - markHeight, rect.Top - offset);
                drawLine(sr, rect.Left + offset, rect.Top, rect.Left + offset, rect.Top + markHeight);
                drawLine(sr, rect.Right - offset, rect.Top, rect.Right - offset, rect.Top + markHeight);
                drawLine(sr, rect.Right, rect.Top - offset, rect.Right + markHeight, rect.Top - offset);
                drawLine(sr, rect.Right, rect.Bottom + offset, rect.Right + markHeight, rect.Bottom + offset);
            }
        }

        private void drawLine(ShapeRange sr, double startX, double startY, double endX, double endY)
        {
            corel.Shape line;
            corel.Color color = corelApp.CreateRegistrationColor();
            OutlineStyle oStyle = corelApp.OutlineStyles[0];
            double width = 0.0762;

            if (checkPoint(sr, endX, endY))
            {
                line = corelApp.ActiveLayer.CreateLineSegment(startX, startY, endX, endY);
                line.Outline.SetProperties(width, oStyle, color);
            }
        }

        bool checkPoint(ShapeRange sr, double x, double y)
        {
            foreach (corel.Shape s in sr)
            {
                corel.Rect r = s.BoundingBox;
                if (r.IsPointInside(x, y))
                    return false;
            }
            return true;
        }
    }
}
