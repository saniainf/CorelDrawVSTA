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

		public ConvertUserColor(corel.Color castColor)
		{
			this.castColor = castColor;
		}

		public corel.Color Convert(corel.Color color)
		{
			return castColor;
		}
	}
}
