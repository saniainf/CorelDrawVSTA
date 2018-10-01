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

		public CheckImpureBlack(int blackLimit)
		{
			this.blackLimit = blackLimit;
		}

		public bool Check(corel.Color color)
		{
			switch (color.Type)
			{
				case cdrColorType.cdrColorBlackAndWhite:
					break;
				case cdrColorType.cdrColorCMY:
					break;
				case cdrColorType.cdrColorCMYK:
					if ((color.CMYKCyan > 0 || color.CMYKMagenta > 0 || color.CMYKYellow > 0) && color.CMYKBlack > (100 - blackLimit))
						return true;
					break;
				case cdrColorType.cdrColorGray:
					break;
				case cdrColorType.cdrColorHLS:
					break;
				case cdrColorType.cdrColorHSB:
					break;
				case cdrColorType.cdrColorLab:
					break;
				case cdrColorType.cdrColorMixed:
					break;
				case cdrColorType.cdrColorMultiChannel:
					break;
				case cdrColorType.cdrColorPantone:
					break;
				case cdrColorType.cdrColorPantoneHex:
					break;
				case cdrColorType.cdrColorRGB:
					break;
				case cdrColorType.cdrColorRegistration:
					break;
				case cdrColorType.cdrColorSpot:
					break;
				case cdrColorType.cdrColorUserInk:
					break;
				case cdrColorType.cdrColorYIQ:
					break;
				default:
					break;
			}

			return false;
		}
	}
}
