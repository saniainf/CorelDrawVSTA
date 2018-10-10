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

		private void Start()
		{
			stopwatch.Reset();
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

			ICheckColor check = (ICheckColor)new CheckNoneColor();
			IConvertColor convert = (IConvertColor)new ConvertKeepColor();

			switch (cbRemap.SelectedIndex)
			{
				case 0:
					switch (cbRemapColorType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = "remap color";
							check = (ICheckColor)new CheckUserColor(colorRemapUserColor);
							break;
						case 1:
							tbHelpTips.Text = "remap impure black";
							check = (ICheckColor)new CheckImpureBlack(tbRemapImpureBlackColorLimit.Value);
							break;
						case 2:
							tbHelpTips.Text = "remap impure gray";
							check = (ICheckColor)new CheckImpureGray(tbRemapImpureGrayColorLimit.Value);
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
							check = (ICheckColor)new CheckColorSpaceCMYK();
							break;
						case 1:
							tbHelpTips.Text = "remap color space rgb";
							check = (ICheckColor)new CheckColorSpaceRGB();
							break;
						case 2:
							tbHelpTips.Text = "remap color space gray";
							check = (ICheckColor)new CheckColorSpaceGray();
							break;
						case 3:
							tbHelpTips.Text = "remap color space pantone";
							check = (ICheckColor)new CheckColorSpacePantone();
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
							check = (ICheckColor)new CheckColorRangeCMYK(tbRemapColorRangeCyanMin.Value,
																		tbRemapColorRangeCyanMax.Value,
																		tbRemapColorRangeMagentaMin.Value,
																		tbRemapColorRangeMagentaMax.Value,
																		tbRemapColorRangeYellowMin.Value,
																		tbRemapColorRangeYellowMax.Value,
																		tbRemapColorRangeBlackMin.Value,
																		tbRemapColorRangeBlackMax.Value);
							break;
						case 1:
							tbHelpTips.Text = "remap rgb range";
							check = (ICheckColor)new CheckColorRangeRGB(tbRemapColorRangeRedMin.Value,
																		tbRemapColorRangeRedMax.Value,
																		tbRemapColorRangeGreenMin.Value,
																		tbRemapColorRangeGreenMax.Value,
																		tbRemapColorRangeBlueMin.Value,
																		tbRemapColorRangeBlueMax.Value);
							break;
						case 2:
							tbHelpTips.Text = "remap gray range";
							check = (ICheckColor)new CheckColorRangeGray(tbRemapColorRangeGrayMin.Value, tbRemapColorRangeGrayMax.Value);
							break;
						default:
							break;
					}
					break;
				case 3:
					tbHelpTips.Text = "remap any color";
					check = (ICheckColor)new CheckAnyColor();
					break;
				default:
					break;
			}

			switch (cbTo.SelectedIndex)
			{
				case 0:
					tbHelpTips.Text = tbHelpTips.Text + " to color";
					convert = (IConvertColor)new ConvertUserColor(colorToUserColor);
					break;
				case 1:
					switch (cbToColorSpaceType.SelectedIndex)
					{
						case 0:
							tbHelpTips.Text = tbHelpTips.Text + " to color space cmyk";
							convert = (IConvertColor)new ConvertColorSpaceCMYK();
							break;
						case 1:
							tbHelpTips.Text = tbHelpTips.Text + " to color space cmyk + pantone";
							convert = (IConvertColor)new ConvertColorSpaceCMYKPantone();
							break;
						case 2:
							tbHelpTips.Text = tbHelpTips.Text + " to color space rgb";
							convert = (IConvertColor)new ConvertColorSpaceRGB();
							break;
						case 3:
							tbHelpTips.Text = tbHelpTips.Text + " to color space gray";
							convert = (IConvertColor)new ConvertColorSpaceGray();
							break;
						case 4:
							tbHelpTips.Text = tbHelpTips.Text + " to color space pantone";

							break;
						default:
							break;
					}
					break;
				case 2:
					tbHelpTips.Text = tbHelpTips.Text + " to color tint";
					convert = (IConvertColor)new ConvertColorTint(fountainColorTint);
					break;
				default:
					break;
			}

			RemapShapeRange remapShapeRange = new RemapShapeRange(check, convert, corelApp.ActivePage.Shapes.All());
			remapShapeRange.Start();

			corelApp.ActiveDocument.ClearSelection();
			corelApp.Optimization = false;
			corelApp.ActiveWindow.Refresh();
			corelApp.Application.Refresh();

			stopwatch.Stop();
			//MessageBox.Show(stopwatch.ElapsedMilliseconds);
		}
	}
}
