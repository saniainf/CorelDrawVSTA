using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfTrimMarks
{
    struct DoMarksProperties
    {
        public readonly bool CanDecrease;
        public readonly bool OneCutting;
        public readonly double Offset;
        public readonly double MarkHeight;
        public readonly bool WhiteSubMark;
        public readonly bool ExcludeLeftEdge;
        public readonly bool ExcludeRightEdge;
        public readonly bool ExcludeTopEdge;
        public readonly bool ExcludeBottomEdge;

        public DoMarksProperties(double offset, double height, bool oneCut, bool canDecrease, bool subMark, bool leftEdge, bool rightEdge, bool topEdge, bool bottomEdge)
        {
            Offset = offset;
            MarkHeight = height;
            OneCutting = oneCut;
            CanDecrease = canDecrease;
            WhiteSubMark = subMark;
            ExcludeLeftEdge = leftEdge;
            ExcludeRightEdge = rightEdge;
            ExcludeTopEdge = topEdge;
            ExcludeBottomEdge = bottomEdge;
        }
    }
}
