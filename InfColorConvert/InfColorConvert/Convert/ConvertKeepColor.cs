using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class ConvertKeepColor : IConvertColor
	{
		public corel.Color Convert(corel.Color color)
		{
			return color;
		}
	}
}
