using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;
using System;
using System.Windows;
using System.Collections.Generic;

namespace InfTrimMarks
{
    class SmartTrimMark
    {
        private corel.Application corelApp;
        private double safeZone = 0.2d;
        double decrement = 0.5d;
        double outlineWidth = 0.0762d;

        public SmartTrimMark(corel.Application app)
        {
            this.corelApp = app;
        }

        public void DoSmartTrimMarks(bool canDecrease, bool oneShoot, double offset, double markHeight, ShapeRange sr, bool whiteSubMark, bool leftEdge, bool rightEdge, bool topEdge, bool bottomEdge)
        {
            corel.Rect rect = new corel.Rect();
            ShapeRange marks = new ShapeRange();
            List<corel.Rect> crossShape;
            Mark leftBot, leftTop, topLeft, topRight, rightBot, rightTop, botLeft, botRight;
            foreach (corel.Shape currentShape in sr)
            {
                rect = currentShape.BoundingBox;
                if (oneShoot)
                {
                    leftBot = new Mark(rect.Left - offset, rect.Bottom, -1, 0, markHeight);
                    leftTop = new Mark(rect.Left - offset, rect.Top, -1, 0, markHeight);
                    topLeft = new Mark(rect.Left, rect.Top + offset, 0, 1, markHeight);
                    topRight = new Mark(rect.Right, rect.Top + offset, 0, 1, markHeight);
                    rightBot = new Mark(rect.Right + offset, rect.Bottom, 1, 0, markHeight);
                    rightTop = new Mark(rect.Right + offset, rect.Top, 1, 0, markHeight);
                    botLeft = new Mark(rect.Left, rect.Bottom - offset, 0, -1, markHeight);
                    botRight = new Mark(rect.Right, rect.Bottom - offset, 0, -1, markHeight);
                }
                else
                {
                    leftBot = new Mark(rect.Left, rect.Bottom + offset, -1, 0, markHeight);
                    leftTop = new Mark(rect.Left, rect.Top - offset, -1, 0, markHeight);
                    topLeft = new Mark(rect.Left + offset, rect.Top, 0, 1, markHeight);
                    topRight = new Mark(rect.Right - offset, rect.Top, 0, 1, markHeight);
                    rightBot = new Mark(rect.Right, rect.Bottom + offset, 1, 0, markHeight);
                    rightTop = new Mark(rect.Right, rect.Top - offset, 1, 0, markHeight);
                    botLeft = new Mark(rect.Left + offset, rect.Bottom, 0, -1, markHeight);
                    botRight = new Mark(rect.Right - offset, rect.Bottom, 0, -1, markHeight);
                }

                rect.Offset(-(markHeight * 2), -(markHeight * 2));
                rect.Width = rect.Width + (markHeight * 4);
                rect.Height = rect.Height + (markHeight * 4);
                crossShape = new List<corel.Rect>();
                foreach (Shape s in sr)
                    if (!matchBB(s, currentShape))
                        if (crossAABBAABB(s.BoundingBox, rect))
                            crossShape.Add(s.BoundingBox);

                drawMark(leftBot, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(leftTop, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(topLeft, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(topRight, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(rightBot, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(rightTop, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(botLeft, marks, crossShape, currentShape.BoundingBox, canDecrease);
                drawMark(botRight, marks, crossShape, currentShape.BoundingBox, canDecrease);
            }

            removeDoubleLine(marks);
            removeOutsideLine(marks, sr.BoundingBox, leftEdge, rightEdge, topEdge, bottomEdge);
            corel.Shape groupMarks = marks.Group();

            if (whiteSubMark)
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

        private void drawMark(Mark mark, ShapeRange marks, List<corel.Rect> crossShape, corel.Rect currentShape, bool canDecrease)
        {
            if (canDecrease)
            {
                do
                {
                    if (!crossLineAABB(crossShape, mark))
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
                if (!crossLineAABB(crossShape, mark))
                    marks.Add(drawLine(mark));
            }
        }

        private void removeOutsideLine(ShapeRange marks, corel.Rect rect, bool leftEdge, bool rightEdge, bool topEdge, bool bottomEdge)
        {
            ShapeRange toRemove = new ShapeRange();
            foreach (corel.Shape s in marks)
            {
                if (leftEdge && s.RightX < rect.Left + safeZone)
                    toRemove.Add(s);
                if (rightEdge && s.LeftX > rect.Right - safeZone)
                    toRemove.Add(s);
                if (topEdge && s.BottomY > rect.Top - safeZone)
                    toRemove.Add(s);
                if (bottomEdge && s.TopY < rect.Bottom + safeZone)
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
            double width = outlineWidth;
            line = corelApp.ActiveLayer.CreateLineSegment(mark.StartX, mark.StartY, mark.EndX, mark.EndY);
            line.Outline.SetProperties(width, oStyle, color);
            return line;
        }

        private bool crossAABBAABB(corel.Rect r1, corel.Rect r2)
        {
            if (Math.Abs(r1.CenterX - r2.CenterX) > (r1.Width / 2 + r2.Width / 2))
                return false;
            if (Math.Abs(r1.CenterY - r2.CenterY) > (r1.Height / 2 + r2.Height / 2))
                return false;
            return true;
        }

        private bool matchBB(corel.Shape s1, corel.Shape s2)
        {
            if (s1.CenterX == s2.CenterX && s1.CenterY == s2.CenterY && s1.SizeWidth == s2.SizeWidth && s1.SizeHeight == s2.SizeHeight)
                return true;
            return false;
        }

        private bool crossLineAABB(List<corel.Rect> rects, Mark mark)
        {
            double markHalfSize;
            double rectHalfSize;
            double tSize;

            if (mark.DirectionX == 0)
            {
                //vert
                markHalfSize = mark.Height / 2;
                foreach (corel.Rect r in rects)
                {
                    rectHalfSize = r.Height / 2;
                    tSize = Math.Abs(r.CenterY - mark.CenterY) - safeZone;
                    if (mark.CenterX > r.Left - safeZone && mark.CenterX < r.Right + safeZone)
                        if (tSize < markHalfSize + rectHalfSize)
                            return true;
                }
            }
            else
            {
                //horiz
                markHalfSize = mark.Height / 2;
                foreach (corel.Rect r in rects)
                {
                    rectHalfSize = r.Width / 2;
                    tSize = Math.Abs(r.CenterX - mark.CenterX) - safeZone;
                    if (mark.CenterY > r.Bottom - safeZone && mark.CenterY < r.Top + safeZone)
                        if (tSize < markHalfSize + rectHalfSize)
                            return true;
                }
            }
            return false;
        }

        private bool endPointInside(ShapeRange sr, Mark mark)
        {
            corel.Rect r;
            foreach (corel.Shape s in sr)
            {
                r = s.BoundingBox;
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
                    if (matchBB(marks.Shapes[i], marks.Shapes[j]))
                        toRemove.Add(marks.Shapes[i]);
                }
            }
            marks.RemoveRange(toRemove);
            toRemove.Delete();
        }
    }
}
