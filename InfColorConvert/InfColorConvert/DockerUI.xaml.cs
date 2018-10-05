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

		private corel.Color colorRemapUserColor = new corel.Color();
		private corel.Color colorToUserColor = new corel.Color();

		private int[] rangeRemapColorRangeCMYK = new int[8] { 50, 50, 50, 50, 50, 50, 50, 50 };
		private int[] rangeRemapColorRangeRGB = new int[6] { 50, 50, 50, 50, 50, 50 };
		private int[] rangeRemapColorRangeGray = new int[2] { 50, 50 };

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

		private void cbRemapColorSpaceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

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

		private void cbToColorSpaceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

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


		private void btnPickColorRemapColorRangeCMYK_Click(object sender, RoutedEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				corel.Color c = new corel.Color();
				if (!c.UserAssignEx())
					return;

				if (c.Type != cdrColorType.cdrColorCMYK)
					c.ConvertToCMYK();

				tbRemapColorRangeCyanMin.Value = Math.Min(c.CMYKCyan, tbRemapColorRangeCyanMin.Value);
				tbRemapColorRangeMagentaMin.Value = Math.Min(c.CMYKMagenta, tbRemapColorRangeMagentaMin.Value);
				tbRemapColorRangeYellowMin.Value = Math.Min(c.CMYKYellow, tbRemapColorRangeYellowMin.Value);
				tbRemapColorRangeBlackMin.Value = Math.Min(c.CMYKBlack, tbRemapColorRangeBlackMin.Value);

				tbRemapColorRangeCyanMax.Value = Math.Max(c.CMYKCyan, tbRemapColorRangeCyanMax.Value);
				tbRemapColorRangeMagentaMax.Value = Math.Max(c.CMYKMagenta, tbRemapColorRangeMagentaMax.Value);
				tbRemapColorRangeYellowMax.Value = Math.Max(c.CMYKYellow, tbRemapColorRangeYellowMax.Value);
				tbRemapColorRangeBlackMax.Value = Math.Max(c.CMYKBlack, tbRemapColorRangeBlackMax.Value);
			}
		}

		private void chbApplyTo_Checked(object sender, RoutedEventArgs e)
		{

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
					name = string.Format("L:{0} a:{1} b:{2}", c.LabLuminance, c.LabComponentA, c.LabComponentB);
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
	}
}
