using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckColorRangeRGB : ICheckColor
	{
		private int redMin, redMax, greenMin, greenMax, blueMin, blueMax;

		public CheckColorRangeRGB(int rMin, int rMax, int gMin, int gMax, int bMin, int bMax)
		{
			this.redMin = rMin;
			this.redMax = rMax;
			this.greenMin = gMin;
			this.greenMax = gMax;
			this.blueMin = bMin;
			this.blueMax = bMax;
		}

		public bool Check(corel.Color color)
		{
			if (color.Type != cdrColorType.cdrColorRGB)
				return false;

			if ((color.RGBRed >= redMin && color.RGBRed <= redMax) &&
				(color.RGBGreen >= greenMin && color.RGBGreen <= greenMax) &&
				(color.RGBBlue >= blueMin && color.RGBBlue <= blueMax))
				return true;

			return false;
		}
	}
}
