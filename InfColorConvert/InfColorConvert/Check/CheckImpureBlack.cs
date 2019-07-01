using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckImpureBlack : ICheckColor
	{
		int blackLimit;
		int sumCMY = 30;

		public CheckImpureBlack(int blackLimit)
		{
			this.blackLimit = blackLimit;
		}

		public bool Check(corel.Color color)
		{
			if (color.IsCMYK)
				if ((color.CMYKCyan + color.CMYKMagenta + color.CMYKYellow > sumCMY) && color.CMYKBlack > (100 - blackLimit))
					return true;

			return false;
		}
	}
}
