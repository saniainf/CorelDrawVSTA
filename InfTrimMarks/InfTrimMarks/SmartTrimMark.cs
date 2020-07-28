using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;
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

        public void DoSmartTrimMarks(DoMarksProperties props, ShapeRange sr)
        {
            HashSet<FigureBBox> figureBBoxes = GetFigureBBoxes(sr, props);
            FigureBBox overBBox = GetOverlapBBox(figureBBoxes);
            HashSet<Mark> marks = LayoutMarks(figureBBoxes, overBBox, props);
            DrawMarks(marks, props);
        }

        private HashSet<FigureBBox> GetFigureBBoxes(ShapeRange sr, DoMarksProperties props)
        {
            HashSet<FigureBBox> result = new HashSet<FigureBBox>();

            foreach (corel.Shape item in sr)
            {
                if (!props.OneCutting)
                    result.Add(new FigureBBox(item.BoundingBox));
                else
                {
                    var bbox = item.BoundingBox;
                    var x = bbox.x - props.Offset;
                    var y = bbox.y - props.Offset;
                    var width = props.Offset * 2 + bbox.Width;
                    var height = props.Offset * 2 + bbox.Height;
                    result.Add(new FigureBBox(x, y, width, height));
                }
            }

            return result;
        }

        private FigureBBox GetOverlapBBox(HashSet<FigureBBox> boxes)
        {
            double x = double.MaxValue;
            double y = double.MaxValue;
            double right = double.MinValue;
            double top = double.MinValue;

            foreach (var item in boxes)
            {
                x = x < item.X ? x : item.X;
                y = y < item.Y ? y : item.Y;
                right = right > item.Right ? right : item.Right;
                top = top > item.Top ? top : item.Top;
            }

            return new FigureBBox(x, y, right - x, top - y);
        }

        private HashSet<Mark> LayoutMarks(HashSet<FigureBBox> bBoxes, FigureBBox overBBox, DoMarksProperties props)
        {
            var marks = new HashSet<Mark>();
            foreach (var bbox in bBoxes)
            {

                NewMark(marks, bBoxes, overBBox, props, bbox.X, bbox.Top - props.Offset, -1, 0);
                NewMark(marks, bBoxes, overBBox, props, bbox.X, bbox.Y + props.Offset, -1, 0);
                NewMark(marks, bBoxes, overBBox, props, bbox.X + props.Offset, bbox.Y, 0, -1);
                NewMark(marks, bBoxes, overBBox, props, bbox.Right - props.Offset, bbox.Y, 0, -1);
                NewMark(marks, bBoxes, overBBox, props, bbox.Right, bbox.Y + props.Offset, 1, 0);
                NewMark(marks, bBoxes, overBBox, props, bbox.Right, bbox.Top - props.Offset, 1, 0);
                NewMark(marks, bBoxes, overBBox, props, bbox.Right - props.Offset, bbox.Top, 0, 1);
                NewMark(marks, bBoxes, overBBox, props, bbox.X + props.Offset, bbox.Top, 0, 1);
            }
            return marks;
        }

        private void NewMark(HashSet<Mark> marks, HashSet<FigureBBox> bBoxes, FigureBBox overBBox, DoMarksProperties props, double sx, double sy, int directionX, int directionY)
        {
            if (PointOutside(overBBox, sx, sy, props))
                return;

            var height = props.MarkHeight;
            do
            {
                var ex = directionX * height + sx;
                var ey = directionY * height + sy;
                if (PointInside(bBoxes, ex, ey))
                    if (props.CanDecrease)
                        height -= decrement;
                    else break;
                else
                {
                    marks.Add(new Mark(new Point(sx, sy), new Point(ex, ey)));
                    break;
                }
            } while (height > 0);
        }

        private bool PointInside(HashSet<FigureBBox> bBoxes, double x, double y)
        {
            foreach (var item in bBoxes)
                if (item.IsPointInside(x, y))
                    return true;

            return false;
        }

        private bool PointOutside(FigureBBox box, double x, double y, DoMarksProperties props)
        {
            if (props.ExcludeLeftEdge && x < box.X + safeZone)
                return true;
            if (props.ExcludeRightEdge && x > box.Right - safeZone)
                return true;
            if (props.ExcludeTopEdge && y > box.Top - safeZone)
                return true;
            if (props.ExcludeBottomEdge && y < box.Y + safeZone)
                return true;
            return false;
        }

        private void DrawMarks(HashSet<Mark> marks, DoMarksProperties props)
        {
            ShapeRange sr = new ShapeRange();

            foreach (var item in marks)
                sr.Add(DrawLine(item));

            Shape groupMarks = sr.Group();

            if (props.WhiteSubMark)
            {
                Shape wsm = groupMarks.Duplicate();
                wsm.Outline.SetProperties(1, corelApp.OutlineStyles[0], corelApp.CreateCMYKColor(0, 0, 0, 0));
                wsm.OrderBackOne();
                ShapeRange tempSr = new ShapeRange();
                tempSr.Add(wsm);
                tempSr.Add(groupMarks);
                tempSr.Group();
            }
        }

        private Shape DrawLine(Mark mark)
        {
            Shape line;
            corel.Color color = corelApp.CreateRegistrationColor();
            OutlineStyle oStyle = corelApp.OutlineStyles[0];
            double width = outlineWidth;
            line = corelApp.ActiveLayer.CreateLineSegment(mark.StartPoint.X, mark.StartPoint.Y, mark.EndPoint.X, mark.EndPoint.Y);
            line.Outline.SetProperties(width, oStyle, color);
            line.Fill.ApplyNoFill();
            return line;
        }
    }
}
