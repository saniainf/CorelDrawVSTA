using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckColorRangeCMYK : ICheckColor
	{
		private int cyanMin, cyanMax, magentaMin, magentaMax, yellowMin, yellowMax, blackMin, blackMax;


		public CheckColorRangeCMYK(int cMin, int cMax, int mMin, int mMax, int yMin, int yMax, int bMin, int bMax)
		{
			this.cyanMin = cMin;
			this.cyanMax = cMax;
			this.magentaMin = mMin;
			this.magentaMax = mMax;
			this.yellowMin = yMin;
			this.yellowMax = yMax;
			this.blackMin = bMin;
			this.blackMax = bMax;
		}

		public bool Check(corel.Color color)
		{
			if (color.Type != cdrColorType.cdrColorCMYK)
				return false;

			if ((color.CMYKCyan >= cyanMin && color.CMYKCyan <= cyanMax) &&
				(color.CMYKMagenta >= magentaMin && color.CMYKMagenta <= magentaMax) &&
				(color.CMYKYellow >= yellowMin && color.CMYKYellow <= yellowMax) &&
				(color.CMYKBlack >= blackMin && color.CMYKBlack <= blackMax))
				return true;

			return false;
		}
	}
}
