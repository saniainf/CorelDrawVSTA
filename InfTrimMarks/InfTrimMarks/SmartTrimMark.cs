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
        private double safeZone = 0.5d;
        double decrement = 0.5d;

        public SmartTrimMark(corel.Application app)
        {
            this.corelApp = app;
        }

        public void DoSmartTrimMark(double offset, double markHeight, ShapeRange sr)
        {
            corel.Rect rect = new corel.Rect();

            foreach (corel.Shape s in sr)
            {
                rect = s.BoundingBox;
                //drawLine(rect.Left + offset, rect.Bottom, rect.Left + offset, rect.Bottom - markHeight);
                //drawLine(rect.Right - offset, rect.Bottom, rect.Right - offset, rect.Bottom - markHeight);
                //drawLine(rect.Left, rect.Bottom + offset, rect.Left - markHeight, rect.Bottom + offset);
                //drawLine(rect.Left, rect.Top - offset, rect.Left - markHeight, rect.Top - offset);
                //drawLine(rect.Left + offset, rect.Top, rect.Left + offset, rect.Top + markHeight);
                //drawLine(rect.Right - offset, rect.Top, rect.Right - offset, rect.Top + markHeight);
                //drawLine(rect.Right, rect.Top - offset, rect.Right + markHeight, rect.Top - offset);
                //drawLine(rect.Right, rect.Bottom + offset, rect.Right + markHeight, rect.Bottom + offset);
            }
        }

        public void DoSmartTrimMarksOneShoot(double offset, double markHeight, ShapeRange sr, bool? whiteSubMark, bool? leftEdge, bool? rightEdge, bool? topEdge, bool? bottomEdge)
        {
            corel.Rect rect = new corel.Rect();
            ShapeRange marks = new ShapeRange();

            foreach (corel.Shape s in sr)
            {
                rect = s.BoundingBox;

                Mark lb = new Mark(rect.Left - offset, rect.Bottom, -1, 0, markHeight);
                Mark lt = new Mark(rect.Left - offset, rect.Top, -1, 0, markHeight);
                Mark tl = new Mark(rect.Left, rect.Top + offset, 0, 1, markHeight);
                Mark tr = new Mark(rect.Right, rect.Top + offset, 0, 1, markHeight);
                Mark rb = new Mark(rect.Right + offset, rect.Bottom, 1, 0, markHeight);
                Mark rt = new Mark(rect.Right + offset, rect.Top, 1, 0, markHeight);
                Mark bl = new Mark(rect.Left, rect.Bottom - offset, 0, -1, markHeight);
                Mark br = new Mark(rect.Right, rect.Bottom - offset, 0, -1, markHeight);

                drawMark(marks, sr, lb, true);
                drawMark(marks, sr, lt, true);
                drawMark(marks, sr, tl, true);
                drawMark(marks, sr, tr, true);
                drawMark(marks, sr, rb, true);
                drawMark(marks, sr, rt, true);
                drawMark(marks, sr, bl, true);
                drawMark(marks, sr, br, true);
            }
            removeDoubleLine(marks);
            removeOutsideLine(marks, sr.BoundingBox, leftEdge ?? false, rightEdge ?? false, topEdge ?? false, bottomEdge ?? false);
            corel.Shape groupMarks = marks.Group();
            if (whiteSubMark ?? false)
            {
                corel.Shape wsm = groupMarks.Duplicate();
                wsm.Outline.SetProperties(1, corelApp.OutlineStyles[0], corelApp.CreateCMYKColor(0, 0, 0, 0));
                wsm.OrderBackOne();
                ShapeRange tempSr = new ShapeRange();
                tempSr.Add(wsm);
                tempSr.Add(groupMarks);
                groupMarks = tempSr.Group();
            }
        }

        private void removeOutsideLine(ShapeRange marks, corel.Rect rect, bool leftEdge, bool rightEdge, bool topEdge, bool bottomEdge)
        {
            ShapeRange toRemove = new ShapeRange();
            foreach (corel.Shape s in marks)
            {
                if (leftEdge && s.RightX < rect.Left)
                    toRemove.Add(s);
                if (rightEdge && s.LeftX > rect.Right)
                    toRemove.Add(s);
                if (topEdge && s.BottomY > rect.Top)
                    toRemove.Add(s);
                if (bottomEdge && s.TopY < rect.Bottom)
                    toRemove.Add(s);
            }
            marks.RemoveRange(toRemove);
            toRemove.Delete();
        }

        private corel.Shape drawLine(Mark mark)
        {
            corel.Shape line;
            corel.Color color = corelApp.CreateRegistrationColor();
            OutlineStyle oStyle = corelApp.OutlineStyles[0];
            double width = 0.0762;
            line = corelApp.ActiveLayer.CreateLineSegment(mark.StartX, mark.StartY, mark.EndX, mark.EndY);
            line.Outline.SetProperties(width, oStyle, color);
            return line;
        }

        private void drawMark(ShapeRange marks, ShapeRange sr, Mark mark, bool canDecrease)
        {

            if (canDecrease)
            {
                do
                {
                    if (!endPointInside(sr, mark))
                    {
                        marks.Add(drawLine(mark));
                        return;
                    }
                    else
                        mark.Height = mark.Height - decrement;

                } while (mark.Height > 0);
            }
            else
            {
                if (!endPointInside(sr, mark))
                    marks.Add(drawLine(mark));
            }
        }

        private bool endPointInside(ShapeRange sr, Mark mark)
        {
            foreach (corel.Shape s in sr)
            {
                corel.Rect r = s.BoundingBox;
                if (mark.EndX >= (r.Left - safeZone) && mark.EndX <= (r.Right + safeZone))
                    if (mark.EndY <= (r.Top + safeZone) && mark.EndY >= (r.Bottom - safeZone))
                        return true;
            }
            return false;
        }

        private void removeDoubleLine(ShapeRange marks)
        {
            ShapeRange toRemove = new ShapeRange();
            for (int i = 1; i <= marks.Count; i++)
            {
                for (int j = i + 1; j <= marks.Count; j++)
                {
                    corel.Rect sr = marks.Shapes[i].BoundingBox;
                    corel.Rect r = marks.Shapes[j].BoundingBox;
                    if (sr.Left == r.Left && sr.Right == r.Right && sr.Top == r.Top && sr.Bottom == r.Bottom)
                        toRemove.Add(marks.Shapes[i]);
                }
            }
            marks.RemoveRange(toRemove);
            toRemove.Delete();
        }
    }
}
