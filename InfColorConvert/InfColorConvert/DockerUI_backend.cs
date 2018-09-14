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

namespace InfColorConvert
{
	public partial class DockerUI : UserControl
	{
		private delegate bool findDelegate(corel.Shape s);
		private delegate void convertDelegate(corel.Shape s);

		private corel.Color findColor = new corel.Color();

		private void Start()
		{
			findDelegate fd = RemapUserColor;
			convertDelegate cd = ToUserColor;

			RemapShape(fd,cd,corelApp.ActivePage.Shapes.All());
		}

		private void RemapShape(findDelegate find, convertDelegate convert, corel.ShapeRange sr)
		{
			for (int i = 0; i < sr.Count; i++)
				if (find(sr[i]))
					convert(sr[i]);
		}

		#region find methods

		private bool RemapUserColor(corel.Shape s)
		{
			return false;
		}

		#endregion

		#region convert methods

		private void ToUserColor(corel.Shape s)
		{

		}

		#endregion
	}
}
