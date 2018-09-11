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
		private corel.Application corelApp;
		public DockerUI(corel.Application app)
		{
			this.corelApp = app;
			InitializeComponent();

			//default
			cbRemapFrom.SelectedIndex = 0;
			cbRemapColorType.SelectedIndex = 0;
			cbRemapColorSpaceType.SelectedIndex = 0;
			cbRemapColorRangeType.SelectedIndex = 0;

			cbRemapTo.SelectedIndex = 0;
			cbToColorType.SelectedIndex = 0;
			cbToColorSpaceType.SelectedIndex = 0;
		}

		private void cbRemapFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spRemapColor.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorSpace.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorRange.Visibility = System.Windows.Visibility.Collapsed;
			spRemapAnyColor.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spRemapColor.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spRemapColorSpace.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spRemapColorRange.Visibility = System.Windows.Visibility.Visible;
					break;
				case 3:
					spRemapAnyColor.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}

		private void cbRemapColorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spRemapColorCMYK.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorRGB.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorGray.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorSpot.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spRemapColorCMYK.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spRemapColorRGB.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spRemapColorGray.Visibility = System.Windows.Visibility.Visible;
					break;
				case 3:
					spRemapColorSpot.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}

		private void cbRemapColorRangeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spRemapColorRangeCMYK.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorRangeRGB.Visibility = System.Windows.Visibility.Collapsed;
			spRemapColorRangeGray.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spRemapColorRangeCMYK.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spRemapColorRangeRGB.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spRemapColorRangeGray.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}

		private void cbRemapTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spToColor.Visibility = System.Windows.Visibility.Collapsed;
			spToColorSpace.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spToColor.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spToColorSpace.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}

		private void cbToColorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spToColorCMYK.Visibility = System.Windows.Visibility.Collapsed;
			spToColorRGB.Visibility = System.Windows.Visibility.Collapsed;
			spToColorGray.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spToColorCMYK.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spToColorRGB.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spToColorGray.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}
	}
}
