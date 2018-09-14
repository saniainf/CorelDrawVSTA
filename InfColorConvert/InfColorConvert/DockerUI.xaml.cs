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
	//frontend
	public partial class DockerUI : UserControl
	{
		private corel.Application corelApp;

		public DockerUI(corel.Application app)
		{
			this.corelApp = app;
			InitializeComponent();

			//default
			tbHelpTips.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore.";

			cbRemap.SelectedIndex = 0;
			cbRemapColorType.SelectedIndex = 0;
			cbRemapColorSpaceType.SelectedIndex = 0;
			cbRemapColorRangeType.SelectedIndex = 0;

			cbTo.SelectedIndex = 0;
			cbToColorSpaceType.SelectedIndex = 0;

			cbApplyRange.SelectedIndex = 0;
		}

		private void cbRemap_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

			spRemapUserColor.Visibility = System.Windows.Visibility.Collapsed;
			spRemapImpureBlack.Visibility = System.Windows.Visibility.Collapsed;
			spRemapImpureGray.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spRemapUserColor.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spRemapImpureBlack.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spRemapImpureGray.Visibility = System.Windows.Visibility.Visible;
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

		private void cbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;

			spToUserColor.Visibility = System.Windows.Visibility.Collapsed;
			spToColorSpace.Visibility = System.Windows.Visibility.Collapsed;
			spToColorTint.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					spToUserColor.Visibility = System.Windows.Visibility.Visible;
					break;
				case 1:
					spToColorSpace.Visibility = System.Windows.Visibility.Visible;
					break;
				case 2:
					spToColorTint.Visibility = System.Windows.Visibility.Visible;
					break;
				default:
					break;
			}
		}

		private void btnApply_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("apply");
		}
	}
}
