﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;

namespace InfColorConvert
{
	class CheckColorSpaceCMYK : ICheckColor
	{
		public bool Check(corel.Color color)
		{
			return (color.Type == cdrColorType.cdrColorCMYK);
		}
	}
}
