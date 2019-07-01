using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckImpureGray : ICheckColor
	{
		int colorLimit;

		public CheckImpureGray(int colorLimit)
		{
			this.colorLimit = colorLimit;
		}

		// TODO переделать эту хрень!
		public bool Check(Color color)
		{
			if (color.IsCMYK)
			{
				int c = color.CMYKCyan;
				int m = color.CMYKMagenta;
				int y = color.CMYKYellow;

				if (c < 10 || m < 10 || y < 10)
					return false;
				float average = (c + m + y) / 3f;
				float min = average - colorLimit;
				float max = average + colorLimit;

				if (c > min && c < max)
					if (m > min && m < max)
						if (y > min && y < max)
							return true;
			}
			return false;
		}
	}
}
