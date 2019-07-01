using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckUserColor : ICheckColor
	{
		corel.Color sampleColor;
		bool useTint;

		public CheckUserColor(corel.Color sampleColor, bool usePantoneTint)
		{
			this.sampleColor = sampleColor;
			useTint = usePantoneTint;
		}

		public bool Check(corel.Color color)
		{
			if (!useTint && color.IsSpot)
				return CheckPantoneKeepTint(color);

			return (color.IsSame(sampleColor));
		}

		private bool CheckPantoneKeepTint(corel.Color color)
		{
			return (color.SpotColorName == sampleColor.SpotColorName);
		}
	}
}
