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

			colorRemapUserColor.CMYKAssign(0, 0, 0, 0);
			colorToUserColor.CMYKAssign(0, 0, 0, 100);
			UpdateColorBar();

			cbRemap.SelectedIndex = 0;
			cbRemapColorType.SelectedIndex = 0;
			cbRemapColorSpaceType.SelectedIndex = 0;
			cbRemapColorRangeType.SelectedIndex = 0;

			cbTo.SelectedIndex = 0;
			cbToColorSpaceType.SelectedIndex = 0;

			cbApplyRange.SelectedIndex = 0;
		}

		#region combobox events

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

			PickConvertMode();
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

			PickConvertMode();
		}

		private void cbRemapColorSpaceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PickConvertMode();
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

			PickConvertMode();
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

			PickConvertMode();
		}

		private void cbToColorSpaceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PickConvertMode();
		}

		#endregion

		private void btnPickRemapUserColor_Click(object sender, RoutedEventArgs e)
		{
			colorRemapUserColor.UserAssign();
			UpdateColorBar();
		}

		private void btnPickToUserColor_Click(object sender, RoutedEventArgs e)
		{
			colorToUserColor.UserAssign();
			UpdateColorBar();
		}

		private void btnApply_Click(object sender, RoutedEventArgs e)
		{
			Start();
		}

		private void UpdateColorBar()
		{
			cnvRemapUserColorBar.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorRemapUserColor.HexValue));
			cnvToUserColorBar.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorToUserColor.HexValue));

			lblRemapUserColor.Content = ColorName(colorRemapUserColor);
			lblToUserColor.Content = ColorName(colorToUserColor);
		}

		private string ColorName(corel.Color c)
		{
			string name = "undefined";

			switch (c.Type)
			{
				case cdrColorType.cdrColorBlackAndWhite:
					break;
				case cdrColorType.cdrColorCMY:
					name = string.Format("C:{0} M:{1} Y:{2}", c.CMYCyan, c.CMYYellow, c.CMYMagenta);
					break;
				case cdrColorType.cdrColorCMYK:
					name = string.Format("C:{0} M:{1} Y:{2} K:{3}", c.CMYKCyan, c.CMYKYellow, c.CMYKMagenta, c.CMYKBlack);
					break;
				case cdrColorType.cdrColorGray:
					name = string.Format("Gray L: {0}", c.Gray);
					break;
				case cdrColorType.cdrColorHLS:
					name = string.Format("H:{0} L:{1} S:{2}", c.HLSHue, c.HLSLightness, c.HLSSaturation);
					break;
				case cdrColorType.cdrColorHSB:
					name = string.Format("H:{0} S:{1} B:{2}", c.HSBHue, c.HSBSaturation, c.HSBBrightness);
					break;
				case cdrColorType.cdrColorLab:
					name = string.Format("L:{0} A:{1} B:{2}", c.LabLuminance, c.LabComponentA, c.LabComponentB);
					break;
				case cdrColorType.cdrColorMixed:
					break;
				case cdrColorType.cdrColorMultiChannel:
					break;
				case cdrColorType.cdrColorPantone:
					name = c.Name;
					break;
				case cdrColorType.cdrColorPantoneHex:
					name = c.Name;
					break;
				case cdrColorType.cdrColorRGB:
					name = string.Format("R:{0} G:{1} B:{2}", c.RGBRed, c.RGBGreen, c.RGBBlue);
					break;
				case cdrColorType.cdrColorRegistration:
					name = "Registration Color";
					break;
				case cdrColorType.cdrColorSpot:
					name = c.Name;
					break;
				case cdrColorType.cdrColorUserInk:
					name = c.Name;
					break;
				case cdrColorType.cdrColorYIQ:
					name = string.Format("Y:{0} I:{1} Q:{2}", c.YIQLuminanceY, c.YIQChromaI, c.YIQChromaQ);
					break;
				default:
					break;
			}
			return name;
		}

		//адище
		private void PickConvertMode()
		{
			switch (cbRemap.SelectedIndex)
			{
				case 0:
					switch (cbRemapColorType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = "remap color";
							break;
						case 1:
							tbHelpTips.Text = "remap impure black";
							break;
						case 2:
							tbHelpTips.Text = "remap impure gray";
							break;
						default:
							break;
					}
					break;
				case 1:
					switch (cbRemapColorSpaceType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = "remap color space cmyk";
							break;
						case 1:
							tbHelpTips.Text = "remap color space rgb";
							break;
						case 2:
							tbHelpTips.Text = "remap color space gray";
							break;
						default:
							break;
					}
					break;
				case 2:
					switch (cbRemapColorRangeType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = "remap cmyk range";
							break;
						case 1:
							tbHelpTips.Text = "remap rgb range";
							break;
						case 2:
							tbHelpTips.Text = "remap gray range";
							break;
						default:
							break;
					}
					break;
				case 3:
					tbHelpTips.Text = "remap any color";
					break;
				default:
					break;
			}

			switch (cbTo.SelectedIndex)
			{
				case 0:
					tbHelpTips.Text = tbHelpTips.Text + " to color";
					break;
				case 1:
					switch (cbToColorSpaceType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = tbHelpTips.Text + " to color space cmyk";
							break;
						case 1:
							tbHelpTips.Text = tbHelpTips.Text + " to color space rgb";
							break;
						case 2:
							tbHelpTips.Text = tbHelpTips.Text + " to color space gray";
							break;
						default:
							break;
					}
					break;
				case 2:
					tbHelpTips.Text = tbHelpTips.Text + " to color tint";
					break;
				default:
					break;
			}
		}
	}
}
