using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class ConvertColorTint : IConvertColor
	{
		private corel.Color[] fountainColors;

		public ConvertColorTint(corel.Color[] fountainColors)
		{
			this.fountainColors = fountainColors;
		}

		public corel.Color Convert(corel.Color color)
		{
			corel.Color c = new corel.Color();
			c.CopyAssign(color);
			c.ConvertToGray();
			int i = (int)Math.Floor(c.Gray / 2.55f);
			return fountainColors[i];
		}
	}
}
