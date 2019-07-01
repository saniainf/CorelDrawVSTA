using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class ConvertColorSpaceCMYKPantone : IConvertColor
	{
		public corel.Color Convert(corel.Color color)
		{
			if (color.IsCMYK || color.IsSpot)
				return color;

			corel.Color c = new corel.Color();
			c.CopyAssign(color);
			c.ConvertToCMYK();
			return c;
		}
	}
}
