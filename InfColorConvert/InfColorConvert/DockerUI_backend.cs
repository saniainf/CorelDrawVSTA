using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;
using System.Diagnostics;
using System.Threading;

namespace InfColorConvert
{
	public partial class DockerUI : UserControl
	{
		Stopwatch stopwatch = new Stopwatch();
		int ich;

		private corel.Color colorRemapUserColor = new corel.Color();
		private corel.Color colorToUserColor = new corel.Color();

		private void Start()
		{
			stopwatch.Reset();
			ich = 0;
			stopwatch.Start();

			corelApp.Optimization = true;

			switch (cbApplyRange.SelectedIndex)
			{
				case 0:
					break;
				case 1:
					break;
				case 2:
					break;
				case 3:
					break;
				default:
					break;
			}

			//CheckUserColor checkUserC = new CheckUserColor(colorRemapUserColor);
			CheckImpureBlack checkImpureBlack = new CheckImpureBlack(10);
			ConvertUserColor convertUserC = new ConvertUserColor(colorToUserColor);

			RemapShapeRange remapShapeRange = new RemapShapeRange(checkImpureBlack as ICheckColor, convertUserC as IConvertColor, corelApp.ActivePage.Shapes.All());

			corelApp.ActiveDocument.ClearSelection();
			corelApp.Optimization = false;
			corelApp.ActiveWindow.Refresh();
			corelApp.Application.Refresh();

			stopwatch.Stop();
			MessageBox.Show("char count " + ich + " time " + stopwatch.ElapsedMilliseconds);
		}
	}
}
