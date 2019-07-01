using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckColorRangeGray : ICheckColor
	{
		private int grayMin, grayMax;

		public CheckColorRangeGray(int gMin, int gMax)
		{
			this.grayMin = gMin;
			this.grayMax = gMax;
		}

		public bool Check(corel.Color color)
		{
			if (!color.IsGray)
				return false;

			if (color.Gray >= grayMin && color.Gray <= grayMax)
				return true;

			return false;
		}
	}
}
