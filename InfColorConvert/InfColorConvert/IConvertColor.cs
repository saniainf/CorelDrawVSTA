﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;

namespace InfColorConvert
{
	interface IConvertColor
	{
		corel.Color Convert(corel.Color color);
	}
}
