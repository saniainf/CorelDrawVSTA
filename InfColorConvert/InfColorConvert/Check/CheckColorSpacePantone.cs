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
		private bool paletteIsFixed;

		public CheckColorSpacePantone(bool paletteIsFixed)
		{
			this.paletteIsFixed = paletteIsFixed;
		}

		public bool Check(corel.Color color)
		{
			if (color.IsSpot)
			{
				if (paletteIsFixed && color.Palette.Locked)
					return true;
				if (!paletteIsFixed && !color.Palette.Locked)
					return true;
			}
			return false;
		}
	}
}
