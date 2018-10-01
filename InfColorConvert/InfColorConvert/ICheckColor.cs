using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;

namespace InfColorConvert
{
	interface ICheckColor
	{
		bool Check(corel.Color color);
	}
}
