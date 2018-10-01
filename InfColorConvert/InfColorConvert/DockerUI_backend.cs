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

		private delegate bool FindDelegate(corel.Color c);
		private delegate corel.Color ConvertDelegate(corel.Color c);

		private FindDelegate CheckColor;
		private ConvertDelegate ConvertColor;

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

			RemapShapeInShapeRange(corelApp.ActivePage.Shapes.All());

			corelApp.ActiveDocument.ClearSelection();
			corelApp.Optimization = false;
			corelApp.ActiveWindow.Refresh();
			corelApp.Application.Refresh();

			stopwatch.Stop();
			MessageBox.Show("char count " + ich + " time " + stopwatch.ElapsedMilliseconds);
		}

		private void RemapShapeInShapeRange(corel.ShapeRange sr)
		{
			foreach (corel.Shape s in sr)
			{
				if (s.Type == cdrShapeType.cdrGroupShape)
					RemapInGroupShape(s);

				if (s.PowerClip != null)
					RemapInPowerClipShape(s);

				switch (s.Type)
				{
					case cdrShapeType.cdr3DObjectShape:
						break;
					case cdrShapeType.cdrArtisticMediaGroupShape:
						break;
					case cdrShapeType.cdrBevelGroupShape:
						break;
					case cdrShapeType.cdrBitmapShape:
						break;
					case cdrShapeType.cdrBlendGroupShape:
						break;
					case cdrShapeType.cdrConnectorShape:
						break;
					case cdrShapeType.cdrContourGroupShape:
						break;
					case cdrShapeType.cdrCurveShape:
						RemapCdrSimpleShape(s);
						break;
					case cdrShapeType.cdrCustomEffectGroupShape:
						break;
					case cdrShapeType.cdrCustomShape:
						//table
						break;
					case cdrShapeType.cdrDropShadowGroupShape:
						break;
					case cdrShapeType.cdrEPSShape:
						break;
					case cdrShapeType.cdrEllipseShape:
						RemapCdrSimpleShape(s);
						break;
					case cdrShapeType.cdrExtrudeGroupShape:
						break;
					//case cdrShapeType.cdrGroupShape:
					//	break;
					case cdrShapeType.cdrGuidelineShape:
						break;
					case cdrShapeType.cdrHTMLActiveObjectShape:
						break;
					case cdrShapeType.cdrHTMLFormObjectShape:
						break;
					case cdrShapeType.cdrLinearDimensionShape:
						break;
					case cdrShapeType.cdrMeshFillShape:
						break;
					case cdrShapeType.cdrNoShape:
						break;
					case cdrShapeType.cdrOLEObjectShape:
						break;
					case cdrShapeType.cdrPerfectShape:
						RemapCdrSimpleShape(s);
						break;
					case cdrShapeType.cdrPolygonShape:
						RemapCdrSimpleShape(s);
						break;
					case cdrShapeType.cdrRectangleShape:
						RemapCdrSimpleShape(s);
						break;
					case cdrShapeType.cdrSelectionShape:
						break;
					case cdrShapeType.cdrSymbolShape:
						break;
					case cdrShapeType.cdrTextShape:
						RemapCdrTextShape(s);
						break;
					default:
						break;
				}
			}
		}

		private void RemapInGroupShape(corel.Shape s)
		{
			RemapShapeInShapeRange(s.Shapes.All());
		}

		private void RemapInPowerClipShape(corel.Shape s)
		{
			corel.ShapeRange sr = s.PowerClip.Shapes.All();
			s.PowerClip.EnterEditMode();
			RemapShapeInShapeRange(sr);
			s.PowerClip.LeaveEditMode();
		}

		private void RemapCdrTextShape(corel.Shape s)
		{
			foreach (corel.TextRange tr in s.Text.Story.Characters)
			{
				ich++;

				if (tr.Fill.Type == cdrFillType.cdrUniformFill)
				{
					if (CheckColor(tr.Fill.UniformColor))
						tr.Fill.UniformColor = ConvertColor(tr.Fill.UniformColor);
				}

				if (tr.Fill.Type == cdrFillType.cdrFountainFill)
				{
					for (int i = 0; i < tr.Fill.Fountain.Colors.Count; i++)
					{
						if (CheckColor(tr.Fill.Fountain.Colors[i].Color))
							tr.Fill.Fountain.Colors[i].Color = ConvertColor(tr.Fill.Fountain.Colors[i].Color);
					}
				}

				if (tr.Outline.Type == cdrOutlineType.cdrOutline || tr.Outline.Type == cdrOutlineType.cdrEnhancedOutline)
				{
					if (CheckColor(tr.Outline.Color))
						tr.Outline.Color = ConvertColor(tr.Outline.Color);
				}
			}
		}

		private void RemapCdrSimpleShape(corel.Shape s)
		{
			if (s.CanHaveFill)
			{
				switch (s.Fill.Type)
				{
					case cdrFillType.cdrFountainFill:
						RemapFountainFill(s);
						break;
					case cdrFillType.cdrHatchFill:
						break;
					case cdrFillType.cdrNoFill:
						break;
					case cdrFillType.cdrPatternFill:
						break;
					case cdrFillType.cdrPostscriptFill:
						break;
					case cdrFillType.cdrTextureFill:
						break;
					case cdrFillType.cdrUniformFill:
						RemapUniformFill(s);
						break;
					default:
						break;
				}
			}

			if (s.CanHaveOutline)
			{
				switch (s.Outline.Type)
				{
					case cdrOutlineType.cdrEnhancedOutline:
						RamapOutline(s);
						break;
					case cdrOutlineType.cdrNoOutline:
						break;
					case cdrOutlineType.cdrOutline:
						RamapOutline(s);
						break;
					default:
						break;
				}
			}
		}

		private void RemapUniformFill(corel.Shape s)
		{
			if (CheckColor(s.Fill.UniformColor))
				s.Fill.UniformColor = ConvertColor(s.Fill.UniformColor);
		}

		private void RemapFountainFill(corel.Shape s)
		{
			for (int i = 0; i < s.Fill.Fountain.Colors.Count; i++)
			{
				if (CheckColor(s.Fill.Fountain.Colors[i].Color))
					s.Fill.Fountain.Colors[i].Color = ConvertColor(s.Fill.Fountain.Colors[i].Color);
			}
		}

		private void RamapOutline(corel.Shape s)
		{
			if (CheckColor(s.Outline.Color))
				s.Outline.Color = ConvertColor(s.Outline.Color);
		}

		#region check shape methods

		private bool RemapUserColor(corel.Color c)
		{
			return c.IsSame(colorRemapUserColor);
		}

		private bool RemapImpureBlack(corel.Color c)
		{
			return false;
		}

		private bool RemapImpureGray(corel.Color c)
		{
			return false;
		}

		private bool RemapColorSpaceCMYK(corel.Color c)
		{
			return false;
		}

		private bool RemapColorSpaceRGB(corel.Color c)
		{
			return false;
		}

		private bool RemapColorSpaceGray(corel.Color c)
		{
			return false;
		}

		private bool RemapColorRangeCMYK(corel.Color c)
		{
			return false;
		}

		private bool RemapColorRangeRGB(corel.Color c)
		{
			return false;
		}

		private bool RemapColorRangeGray(corel.Color c)
		{
			return false;
		}

		private bool RemapAnyColor(corel.Color c)
		{
			return false;
		}

		#endregion

		#region convert shape methods

		private corel.Color ToUserColor(corel.Color c)
		{
			return colorToUserColor;
		}

		#endregion
	}
}
