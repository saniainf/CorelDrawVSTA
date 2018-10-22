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
		private bool tint = true;
		private Random rnd = new Random();

		public ConvertColorTint(corel.Color[] fountainColors, bool tint)
		{
			this.fountainColors = fountainColors;
			this.tint = tint;
		}

		public corel.Color Convert(corel.Color color)
		{
			corel.Color c = new corel.Color();
			int i = 0;
			c.CopyAssign(color);
			c.ConvertToGray();
			//float y = 0.299f * c.RGBRed + 0.587f * c.RGBGreen + 0.114f * c.RGBBlue;

			if (tint)
				i = (int)Math.Floor(c.Gray / 2.55f);
			else
				i = rnd.Next(100);

			return fountainColors[i];
		}
	}
}
