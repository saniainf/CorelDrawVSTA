﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
    class ConvertUserColor : IConvertColor
    {
        corel.Color castColor;
        bool useTint;

        public ConvertUserColor(corel.Color castColor, bool usePantoneTint)
        {
            useTint = usePantoneTint;
            this.castColor = castColor;
        }

        public corel.Color Convert(corel.Color color)
        {
            if (!useTint && color.Type == cdrColorType.cdrColorSpot && castColor.IsSpot)
                return ConvertPantoneKeepTint(color);
            if (!useTint && color.Type != cdrColorType.cdrColorSpot && castColor.IsSpot)
            {
                corel.Color c = new corel.Color();
                c.CopyAssign(color);
                c.ConvertToGray();
                // 255 = 0, 0 = 100
                int i = c.Gray;
                i = 100 - (int)Math.Round((i / 255f) * 100);
                c.CopyAssign(castColor);
                c.Tint = i;
                return c;
            }
            return castColor;
        }

        private corel.Color ConvertPantoneKeepTint(corel.Color color)
        {
            corel.Color c = new corel.Color();
            c.CopyAssign(castColor);
            c.Tint = color.Tint;
            return c;
        }
    }
}
