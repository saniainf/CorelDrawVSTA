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
		//Stopwatch stopwatch = new Stopwatch();

		private void Start()
		{
			if (corelApp.Documents.Count == 0)
				return;

			//stopwatch.Reset();
			//stopwatch.Start();

			ICheckColor check = (ICheckColor)new CheckNoneColor();
			IConvertColor convert = (IConvertColor)new ConvertKeepColor();

			switch (cbRemap.SelectedIndex)
			{
				case 0:
					switch (cbRemapColorType.SelectedIndex)
					{
						case 0:
							//remap color
							check = (ICheckColor)new CheckUserColor(colorRemapUserColor, chbRemapUserColorTint.IsChecked ?? false); // использование тинта пантона
							break;
						case 1:
							//remap impure black
							check = (ICheckColor)new CheckImpureBlack(tbRemapImpureBlackColorLimit.Value);
							break;
						case 2:
							//remap impure gray
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
							//remap color space cmyk
							check = (ICheckColor)new CheckColorSpaceCMYK();
							break;
						case 1:
							//remap color space rgb
							check = (ICheckColor)new CheckColorSpaceRGB();
							break;
						case 2:
							//remap color space gray
							check = (ICheckColor)new CheckColorSpaceGray();
							break;
						case 3:
							//remap color space pantone
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
							//remap cmyk range
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
							//remap rgb range
							check = (ICheckColor)new CheckColorRangeRGB(tbRemapColorRangeRedMin.Value,
																		tbRemapColorRangeRedMax.Value,
																		tbRemapColorRangeGreenMin.Value,
																		tbRemapColorRangeGreenMax.Value,
																		tbRemapColorRangeBlueMin.Value,
																		tbRemapColorRangeBlueMax.Value);
							break;
						case 2:
							//remap gray range
							check = (ICheckColor)new CheckColorRangeGray(tbRemapColorRangeGrayMin.Value, tbRemapColorRangeGrayMax.Value);
							break;
						default:
							break;
					}
					break;
				case 3:
					//remap any color
					check = (ICheckColor)new CheckAnyColor();
					break;
				default:
					break;
			}

			switch (cbTo.SelectedIndex)
			{
				case 0:
					//to color
					convert = (IConvertColor)new ConvertUserColor(colorToUserColor, chbToUserColorTint.IsChecked ?? false); // использование тинта пантона
					break;
				case 1:
					switch (cbToColorSpaceType.SelectedIndex)
					{
						case 0:
							//to color space cmyk
							convert = (IConvertColor)new ConvertColorSpaceCMYK();
							break;
						case 1:
							//to color space cmyk + pantone
							convert = (IConvertColor)new ConvertColorSpaceCMYKPantone();
							break;
						case 2:
							//to color space rgb
							convert = (IConvertColor)new ConvertColorSpaceRGB();
							break;
						case 3:
							//to color space gray
							convert = (IConvertColor)new ConvertColorSpaceGray();
							break;
						case 4:
							//to color space pantone
							convert = (IConvertColor)new ConvertColorSpacePantone(corelApp);
							break;
						default:
							break;
					}
					break;
				case 2:
					//to color tint
					convert = (IConvertColor)new ConvertColorTint(fountainColorTint, (rbToColorTint.IsChecked ?? false));
					break;
				default:
					break;
			}

			switch (cbApplyRange.SelectedIndex)
			{
				case 0:
					//select
					boost.BoostStart();
					Apply(corelApp.ActiveSelectionRange, check, convert);
					boost.BoostFinish();
					break;
				case 1:
					//layer
					boost.BoostStart();
					Apply(corelApp.ActiveLayer.Shapes.All(), check, convert);
					boost.BoostFinish();
					break;
				case 2:
					//page
					boost.BoostStart();
					Apply(corelApp.ActivePage.SelectableShapes.All(), check, convert);
					boost.BoostFinish();
					break;
				case 3:
					//document
					boost.BoostStart();
					foreach (corel.Page p in corelApp.ActiveDocument.Pages)
					{
						p.Activate();
						Apply(p.SelectableShapes.All(), check, convert);
					}
					boost.BoostFinish();
					break;
				case 4:
					//all
					foreach (corel.Document doc in corelApp.Documents)
					{
						doc.Activate();
						boost.BoostStart();
						foreach (corel.Page p in doc.Pages)
						{
							p.Activate();
							Apply(p.SelectableShapes.All(), check, convert);
						}
						boost.BoostFinish();
					}
					break;
				default:
					break;
			}

			//stopwatch.Stop();
			//MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
		}

		private void Apply(corel.ShapeRange sr, ICheckColor check, IConvertColor convert)
		{
			bool fill = chbApplyFill.IsChecked ?? false;
			bool outline = chbApplyOutline.IsChecked ?? false;
			bool textAs = rbTextAsStory.IsChecked ?? false;

			if (sr != null)
				if (sr.Count > 0)
				{
					RemapShapeRange remapShapeRange = new RemapShapeRange(check, convert, sr, fill, outline, textAs);
					remapShapeRange.Start();
				}
		}
	}
}
