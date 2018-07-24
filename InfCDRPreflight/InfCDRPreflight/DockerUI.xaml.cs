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

namespace InfCDRPreflight
{

	public partial class DockerUI : UserControl
	{
		private corel.Application corelApp;
		private delegate void actionMethod(corel.Shape s);
		static private Random rnd = new Random();

		public DockerUI(corel.Application app)
		{
			this.corelApp = app;
			InitializeComponent();

			replaceColor.CMYKAssign(0, 0, 0, 0);
			applyColor.CMYKAssign(0, 0, 0, 100);
			updateColorSampleBar();
		}

		private void beginAction(actionMethod method)
		{
			if (corelApp.Documents.Count == 0)
				return;

			corelApp.Optimization = true;

			corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;

			if (chxOnAllPage.IsChecked ?? false)
				foreach (corel.Page page in corelApp.ActiveDocument.Pages)
					forEachShapeOnShapeRange(method, page.Shapes.All());
			else
				forEachShapeOnShapeRange(method, corelApp.ActivePage.Shapes.All());

			corelApp.ActiveDocument.ClearSelection();
			corelApp.Optimization = false;
			corelApp.ActiveWindow.Refresh();
			corelApp.Application.Refresh();
		}

		private void forEachShapeOnShapeRange(actionMethod method, corel.ShapeRange sr)
		{
			corel.Shape s;
			while (sr.Count > 0)
			{
				s = sr[1];
				sr.Remove(1);

				// работа в группе
				if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrGroupShape)
					groupShape(method, s);

				// работа в PowerClip
				if (s.PowerClip != null)
					powerClipShape(method, s);

				// текст обрабатывается своими методами
				if (s.Type == corel.cdrShapeType.cdrTextShape)
					textShape(method, s);

				else method(s);
			}
		}

		private void groupShape(actionMethod method, corel.Shape s)
		{
			forEachShapeOnShapeRange(method, s.Shapes.All());
		}

		private void powerClipShape(actionMethod method, corel.Shape s)
		{
			corel.ShapeRange sr = s.PowerClip.Shapes.All();
			s.PowerClip.EnterEditMode();
			forEachShapeOnShapeRange(method, sr);
			s.PowerClip.LeaveEditMode();
		}

		private void textShape(actionMethod method, corel.Shape s)
		{
			if (method == textToCurves)
				method(s);
			if (method == uniformFillToCMYK)
				textUniformFillToCMYK(s.Text.Story.Characters);
			if (method == outlineFillToCMYK)
				textOutlineFillToCMYK(s.Text.Story.Characters);
			else method(s);
		}

		private SolidColorBrush convertToSolidColorBrush(corel.Color c)
		{
			return
				(SolidColorBrush)(new BrushConverter().ConvertFrom(c.HexValue));
		}

		private void testMethod(corel.Shape s)
		{
			//s.Fill.UniformColor.CMYKAssign(0, 100, 50, 0);

			if (s.Type == corel.cdrShapeType.cdrGuidelineShape)
				s.Delete();

			//string str = "";
			//corel.SeparationPlates plates;
			//corel.SeparationPlate plate;
			//plates = corelApp.ActiveDocument.PrintSettings.Separations.Plates;

			//for (int j = 1; j <= plates.Count; j++)
			//{
			//	plate = plates[j];
			//	if (plate.Enabled)
			//		str = str + plate.Color.ToString() + "\n";
			//}
			//MessageBox.Show(str);
		}

		#region convert methods

		private void textUniformFillToCMYK(corel.TextCharacters t)
		{
			foreach (corel.TextRange tr in t)
			{
				if (tr.Characters.All.Fill.UniformColor.Type != corel.cdrColorType.cdrColorCMYK)
					tr.Characters.All.Fill.UniformColor.ConvertToCMYK();
			}
		}

		private void textOutlineFillToCMYK(corel.TextCharacters t)
		{
			foreach (corel.TextRange tr in t)
			{
				if (tr.Characters.All.Outline.Color.Type != corel.cdrColorType.cdrColorCMYK)
					tr.Characters.All.Outline.Color.ConvertToCMYK();
			}
		}

		private void textToCurves(corel.Shape s)
		{
			if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrTextShape)
				s.ConvertToCurves();
		}

		private void oleShapesToCurves(corel.Shape s)
		{
			if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrOLEObjectShape)
			{
				corel.Rect oleRect;
				corel.Shape vectShape;

				corel.Shape oleShape = s;
				oleRect = oleShape.BoundingBox;

				oleShape.Copy();
				corelApp.ActiveLayer.PasteSpecial("Metafile");
				vectShape = corelApp.ActiveSelection.Shapes.First;
				//vectShape.ConvertToCurves();
				vectShape.RotationAngle = oleShape.RotationAngle;
				vectShape.SetPosition(oleRect.Left, oleRect.Top);
				vectShape.SetSize(oleRect.Width, oleRect.Height);

				vectShape.TreeNode.LinkAfter(oleShape.TreeNode);
				oleShape.Delete();
			}
		}

		private void uniformFillToCMYK(corel.Shape s)
		{
			if (s.CanHaveFill)
				if (s.Fill.Type == corel.cdrFillType.cdrUniformFill)
					if (s.Fill.UniformColor.Type != corel.cdrColorType.cdrColorCMYK)
						s.Fill.UniformColor.ConvertToCMYK();
		}

		private void outlineFillToCMYK(corel.Shape s)
		{
			if (s.CanHaveOutline)
				if (s.Outline.Type == corel.cdrOutlineType.cdrOutline)
					if (s.Outline.Color.Type != corel.cdrColorType.cdrColorCMYK)
						s.Outline.Color.ConvertToCMYK();
		}

		private void fountainFillToCMYK(corel.Shape s)
		{
			if (s.CanHaveFill)
				if (s.Fill.Type == corel.cdrFillType.cdrFountainFill)
				{
					foreach (corel.FountainColor c in s.Fill.Fountain.Colors)
					{
						if (c.Color.Type != corel.cdrColorType.cdrColorCMYK)
							c.Color.ConvertToCMYK();
					}
				}
		}

		private void bitmapToCMYK(corel.Shape s)
		{
			if (s.Type == corel.cdrShapeType.cdrBitmapShape)
				if (s.Bitmap.Mode != corel.cdrImageType.cdrCMYKColorImage)
					s.Bitmap.ConvertTo(corel.cdrImageType.cdrCMYKColorImage);
		}

		private void resampleBitmap(corel.Shape s)
		{
			int resolution = tbBitmapDpi.Value;
			if (s.Type == corel.cdrShapeType.cdrBitmapShape)
				if (s.Bitmap.ResolutionX != resolution || s.Bitmap.ResolutionY != resolution)
					s.Bitmap.Resample(0, 0, true, resolution, resolution);
		}

		private void dropShadowBreakApart(corel.Shape s)
		{
			if (s.Type == corel.cdrShapeType.cdrDropShadowGroupShape)
			{
				s.Effect.DropShadow.ShadowGroup.Separate();
			}
		}

		private void lensEffectToBitmap(corel.Shape s)
		{
			if (s.Effects.LensEffect != null)
			{
				s.ConvertToBitmapEx(corel.cdrImageType.cdrCMYKColorImage, false, true, 300, corel.cdrAntiAliasingType.cdrNormalAntiAliasing, true, true, 95);
			}
		}

		private void symbolToShape(corel.Shape s)
		{
			if (s.Type == corel.cdrShapeType.cdrSymbolShape)
			{
				s.Symbol.RevertToShapes();
			}
		}

		private void contourGroupBreakApart(corel.Shape s)
		{
			if (s.Type == corel.cdrShapeType.cdrContourGroupShape)
			{
				s.Effect.Contour.ContourGroup.Separate();
			}
		}

		#endregion

		#region buttons events

		private void btnTextToCurves_Click(object sender, RoutedEventArgs e)
		{
			beginAction(textToCurves);
		}

		private void btnOLEtoCurves_Click(object sender, RoutedEventArgs e)
		{
			beginAction(oleShapesToCurves);
		}

		private void btnTest_Click(object sender, RoutedEventArgs e)
		{
			beginAction(testMethod);

		}

		private void btnUniformFillToCMYK_Click(object sender, RoutedEventArgs e)
		{
			beginAction(uniformFillToCMYK);
		}

		private void btnFountainFillToCMYK_Click(object sender, RoutedEventArgs e)
		{
			beginAction(fountainFillToCMYK);
		}

		private void btnOutlineFillToCMYK_Click(object sender, RoutedEventArgs e)
		{
			beginAction(outlineFillToCMYK);
		}

		private void btnBitmapToCMYK_Click(object sender, RoutedEventArgs e)
		{
			beginAction(bitmapToCMYK);
		}

		private void btnDropShadowBreakApart_Click(object sender, RoutedEventArgs e)
		{
			beginAction(dropShadowBreakApart);
		}

		private void btnLensEffectToBitmap_Click(object sender, RoutedEventArgs e)
		{
			beginAction(lensEffectToBitmap);
		}

		private void btnSymbolToShape_Click(object sender, RoutedEventArgs e)
		{
			beginAction(symbolToShape);
		}

		private void btnContourGroupBreakApart_Click(object sender, RoutedEventArgs e)
		{
			beginAction(contourGroupBreakApart);
		}

		private void btnResampleBitmap_Click(object sender, RoutedEventArgs e)
		{
			beginAction(resampleBitmap);
		}

		#endregion

		#region replace color

		private corel.Color replaceColor = new corel.Color();
		private corel.Color applyColor = new corel.Color();

		private void btnPickReplaceColor_Click(object sender, RoutedEventArgs e)
		{
			replaceColor.UserAssignEx();
			updateColorSampleBar();
		}

		private void btnPickApplyColor_Click(object sender, RoutedEventArgs e)
		{
			applyColor.UserAssignEx();
			updateColorSampleBar();
		}

		private void btnReplaceColor_Click(object sender, RoutedEventArgs e)
		{
			if (rbReplaceFill.IsChecked ?? false)
				beginAction(replaceFillColor);
			if (rbReplaceOutline.IsChecked ?? false)
				beginAction(replaceOutlineColor);
		}

		private void btnSwapColor_Click(object sender, RoutedEventArgs e)
		{
			corel.Color c = replaceColor;
			replaceColor = applyColor;
			applyColor = c;
			updateColorSampleBar();
		}

		private void updateColorSampleBar()
		{
			replaceColorBar.Background = convertToSolidColorBrush(replaceColor);
			replaceColorBar.ToolTip = replaceColor.Type.ToString();
			applyColorBar.Background = convertToSolidColorBrush(applyColor);
			applyColorBar.ToolTip = applyColor.Name;
		}

		private void replaceFillColor(corel.Shape s)
		{
			if (s.CanHaveFill)
			{
				if (s.Fill.Type == corel.cdrFillType.cdrUniformFill)
					if (s.Fill.UniformColor.IsSame(replaceColor))
						s.Fill.UniformColor = applyColor;

				if (s.Fill.Type == corel.cdrFillType.cdrFountainFill)
				{
					for (int i = 0; i < s.Fill.Fountain.Colors.Count; i++)
					{
						if (s.Fill.Fountain.Colors[i].Color.IsSame(replaceColor))
							s.Fill.Fountain.Colors[i].Color = applyColor;
					}
				}
			}
		}

		private void replaceOutlineColor(corel.Shape s)
		{
			if (s.CanHaveOutline)
				if (s.Outline.Type == corel.cdrOutlineType.cdrOutline)
					if (s.Outline.Color.IsSame(replaceColor))
						s.Outline.Color = applyColor;
		}

		#endregion
	}
}
