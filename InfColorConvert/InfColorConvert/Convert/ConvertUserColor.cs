using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class ConvertUserColor : IConvertColor
	{
		corel.Color castColor;
		bool useTint;

		public ConvertUserColor(corel.Color castColor, bool usePantoneTint)
		{
			useTint = usePantoneTint;
			this.castColor = castColor;
		}

		public corel.Color Convert(corel.Color color)
		{
			if (!useTint && color.IsSpot && castColor.IsSpot)
				return ConvertPantoneKeepTint(color);
			return castColor;
		}

		private corel.Color ConvertPantoneKeepTint(corel.Color color)
		{
			corel.Color c = new corel.Color();
			c.CopyAssign(castColor);
			c.Tint = color.Tint;
			return c;
		}
	}
}
