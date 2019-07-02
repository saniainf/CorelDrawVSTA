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

		private Boost boost;

		private corel.Color colorRemapUserColor = new corel.Color();
		private corel.Color colorToUserColor = new corel.Color();
		private corel.Color[] fountainColorTint = new corel.Color[101];

		public DockerUI(corel.Application app)
		{
			this.corelApp = app;
			InitializeComponent();

			boost = new Boost(app);

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

			chbApplyFill.IsChecked = true;
			chbApplyOutline.IsChecked = true;

			for (int i = 0; i < fountainColorTint.Count(); i++)
			{
				fountainColorTint[i] = corelApp.CreateCMYKColor(0, 0, 0, i);
			}
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
			ComboBox comboBox = (ComboBox)sender;

			spRemapColorSpacePantone.Visibility = System.Windows.Visibility.Collapsed;

			switch (comboBox.SelectedIndex)
			{
				case 0:
					break;
				case 1:
					break;
				case 2:
					break;
				case 3:
					spRemapColorSpacePantone.Visibility = System.Windows.Visibility.Visible;
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

				tbRemapColorRangeCyanMin.Value = c.CMYKCyan;
				tbRemapColorRangeCyanMax.Value = c.CMYKCyan;
				tbRemapColorRangeMagentaMin.Value = c.CMYKMagenta;
				tbRemapColorRangeMagentaMax.Value = c.CMYKMagenta;
				tbRemapColorRangeYellowMin.Value = c.CMYKYellow;
				tbRemapColorRangeYellowMax.Value = c.CMYKYellow;
				tbRemapColorRangeBlackMin.Value = c.CMYKBlack;
				tbRemapColorRangeBlackMax.Value = c.CMYKBlack;
			}

			else
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

		private void btnPickColorRemapColorRangeRGB_Click(object sender, RoutedEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				corel.Color c = new corel.Color();
				if (!c.UserAssignEx())
					return;

				if (c.Type != cdrColorType.cdrColorRGB)
					c.ConvertToRGB();

				tbRemapColorRangeRedMin.Value = c.RGBRed;
				tbRemapColorRangeRedMax.Value = c.RGBRed;
				tbRemapColorRangeGreenMin.Value = c.RGBGreen;
				tbRemapColorRangeGreenMax.Value = c.RGBGreen;
				tbRemapColorRangeBlueMin.Value = c.RGBBlue;
				tbRemapColorRangeBlueMax.Value = c.RGBBlue;
			}

			else
			{
				corel.Color c = new corel.Color();
				if (!c.UserAssignEx())
					return;

				if (c.Type != cdrColorType.cdrColorRGB)
					c.ConvertToRGB();

				tbRemapColorRangeRedMin.Value = Math.Min(c.RGBRed, tbRemapColorRangeRedMin.Value);
				tbRemapColorRangeGreenMin.Value = Math.Min(c.RGBGreen, tbRemapColorRangeGreenMin.Value);
				tbRemapColorRangeBlueMin.Value = Math.Min(c.RGBBlue, tbRemapColorRangeBlueMin.Value);

				tbRemapColorRangeRedMax.Value = Math.Max(c.RGBRed, tbRemapColorRangeRedMax.Value);
				tbRemapColorRangeGreenMax.Value = Math.Max(c.RGBGreen, tbRemapColorRangeGreenMax.Value);
				tbRemapColorRangeBlueMax.Value = Math.Max(c.RGBBlue, tbRemapColorRangeBlueMax.Value);
			}
		}

		private void btnPickColorRemapColorRangeGray_Click(object sender, RoutedEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				corel.Color c = new corel.Color();
				if (!c.UserAssignEx())
					return;

				if (c.Type != cdrColorType.cdrColorGray)
					c.ConvertToGray();

				tbRemapColorRangeGrayMin.Value = c.Gray;
				tbRemapColorRangeGrayMax.Value = c.Gray;
			}

			else
			{
				corel.Color c = new corel.Color();
				if (!c.UserAssignEx())
					return;

				if (c.Type != cdrColorType.cdrColorGray)
					c.ConvertToGray();

				tbRemapColorRangeGrayMin.Value = Math.Min(c.Gray, tbRemapColorRangeGrayMin.Value);

				tbRemapColorRangeGrayMax.Value = Math.Max(c.Gray, tbRemapColorRangeGrayMax.Value);
			}
		}

		private void btnGrabColorToColorTint_Click(object sender, RoutedEventArgs e)
		{
			corel.Shape s = corelApp.ActiveSelection;
			if (s == null || s.Shapes.Count > 1 || s.Fill.Type != cdrFillType.cdrFountainFill)
				return;

			corel.FountainFill ff = s.Fill.Fountain;

			for (int i = 1; i < ff.Colors.Count; i++)
			{
				corel.FountainColor startFC = ff.Colors[i - 1];
				corel.FountainColor endFC = ff.Colors[i];
				int posStart = startFC.Position;
				int posEnd = endFC.Position;
				corel.Color startC = startFC.Color;
				corel.Color endC = endFC.Color;

				if (startC.IsSpot)
				{
					corel.Color c = new corel.Color();
					c.CopyAssign(startC);
					c.ConvertToCMYK();
					startC = c;
				}

				if (endC.IsSpot)
				{
					corel.Color c = new corel.Color();
					c.CopyAssign(endC);
					c.ConvertToCMYK();
					endC = c;
				}

				for (int j = posStart; j <= posEnd; j++)
				{
					int cyan = (int)Math.Floor((double)(startC.CMYKCyan + (endC.CMYKCyan - startC.CMYKCyan) * (j - posStart) / (posEnd - posStart)));
					int magenta = (int)Math.Floor((double)(startC.CMYKMagenta + (endC.CMYKMagenta - startC.CMYKMagenta) * (j - posStart) / (posEnd - posStart)));
					int yellow = (int)Math.Floor((double)(startC.CMYKYellow + (endC.CMYKYellow - startC.CMYKYellow) * (j - posStart) / (posEnd - posStart)));
					int black = (int)Math.Floor((double)(startC.CMYKBlack + (endC.CMYKBlack - startC.CMYKBlack) * (j - posStart) / (posEnd - posStart)));

					fountainColorTint[j].CMYKAssign(cyan, magenta, yellow, black);
				}
			}

			cnvToColorSpaceColorBar.Background = ConvertFromFountain(ff);
		}

		private LinearGradientBrush ConvertFromFountain(corel.FountainFill ff)
		{
			LinearGradientBrush lgBrush = new LinearGradientBrush();
			lgBrush.StartPoint = new System.Windows.Point(0, 0.5);
			lgBrush.EndPoint = new System.Windows.Point(1, 0.5);

			foreach (corel.FountainColor fc in ff.Colors)
			{
				corel.Color c = new corel.Color();
				c.CopyAssign(fc.Color);
				c.ConvertToRGB();
				float pos = fc.Position;

				System.Windows.Media.Color bc = System.Windows.Media.Color.FromRgb((byte)c.RGBRed, (byte)c.RGBGreen, (byte)c.RGBBlue);

				GradientStop gs = new GradientStop(bc, pos / 100f);

				lgBrush.GradientStops.Add(gs);
			}

			return lgBrush;
		}

		private void chbApplyFill_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!(chbApplyOutline.IsChecked ?? false))
				chbApplyOutline.IsChecked = true;
		}

		private void chbApplyOutline_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!(chbApplyFill.IsChecked ?? false))
				chbApplyFill.IsChecked = true;
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

		private void btnTest_Click(object sender, RoutedEventArgs e)
		{
			Random rnd = new Random();
			corel.ShapeRange sr = corelApp.ActivePage.Shapes.All();
			int i = 100;

			foreach (corel.Shape s in sr)
			{
				//s.Fill.UniformColor.CMYKAssign(rnd.Next(100), rnd.Next(100), rnd.Next(100), rnd.Next(100));
				//s.Outline.Color.CMYKAssign(rnd.Next(100), rnd.Next(100), rnd.Next(100), rnd.Next(100));
				//s.Fill.UniformColor.RGBAssign(rnd.Next(100), rnd.Next(100), rnd.Next(100));
				//s.Outline.Color.RGBAssign(rnd.Next(100), rnd.Next(100), rnd.Next(100));
				s.Fill.UniformColor = corelApp.CreateCMYKColor(Math.Max(i, 0), Math.Max(i, 0), Math.Max(i, 0), Math.Max(i, 0));
				//s.Outline.Color = corelApp.CreateCMYKColor(0, 0, 0, (rnd.Next(100)));
				i--;
			}
		}
	}
}
