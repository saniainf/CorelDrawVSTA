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

		public CheckUserColor(corel.Color castColor)
		{
			this.sampleColor = castColor;
		}

		public bool Check(corel.Color color)
		{
			return (color.IsSame(sampleColor));
		}
	}
}
