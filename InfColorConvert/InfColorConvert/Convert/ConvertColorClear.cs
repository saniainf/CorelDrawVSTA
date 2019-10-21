using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
    class ConvertColorClear : IConvertColor
    {
        public corel.Color Convert(corel.Color color)
        {
            return color;
        }
    }
}
