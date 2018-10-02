using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckColorSpacePantone : ICheckColor
	{
		public bool Check(corel.Color color)
		{
			return (color.Type == cdrColorType.cdrColorSpot || color.Type == cdrColorType.cdrColorPantone);
		}
	}
}
