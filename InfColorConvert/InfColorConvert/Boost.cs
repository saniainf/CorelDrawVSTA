using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;

namespace InfColorConvert
{
	class Boost
	{
		private corel.Application corelApp;

		public Boost(corel.Application app)
		{
			corelApp = app;
		}

		public void BoostStart()
		{
			corelApp.Optimization = true;
			corelApp.EventsEnabled = false;
			corelApp.ActiveDocument.SaveSettings();
			corelApp.ActiveDocument.PreserveSelection = false;
		}

		public void BoostFinish()
		{
			corelApp.ActiveDocument.PreserveSelection = true;
			corelApp.ActiveDocument.RestoreSettings();
			corelApp.EventsEnabled = true;
			corelApp.Optimization = false;
			corelApp.ActiveWindow.Refresh();
			corelApp.Application.Refresh();
		}
	}
}
