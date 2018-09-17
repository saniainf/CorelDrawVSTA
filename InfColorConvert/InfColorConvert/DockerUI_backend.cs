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
		private delegate bool findDelegate(corel.Color c);
		private delegate corel.Color convertDelegate(corel.Color c);

		private findDelegate checkColor;
		private convertDelegate convertColor;

		private corel.Color colorRemapUserColor = new corel.Color();
		private corel.Color colorToUserColor = new corel.Color();

		private void Start()
		{
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
						RemapCdrCurveShape(s);
						break;
					case cdrShapeType.cdrCustomEffectGroupShape:
						break;
					case cdrShapeType.cdrCustomShape:
						break;
					case cdrShapeType.cdrDropShadowGroupShape:
						break;
					case cdrShapeType.cdrEPSShape:
						break;
					case cdrShapeType.cdrEllipseShape:
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
						break;
					case cdrShapeType.cdrPolygonShape:
						break;
					case cdrShapeType.cdrRectangleShape:
						break;
					case cdrShapeType.cdrSelectionShape:
						break;
					case cdrShapeType.cdrSymbolShape:
						break;
					case cdrShapeType.cdrTextShape:
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

		private void RemapCdrCurveShape(corel.Shape s)
		{
			if (s.CanHaveFill)
			{
				if (s.Fill.Type == corel.cdrFillType.cdrUniformFill)
				{
					if (checkColor(s.Fill.UniformColor))
					{
						s.Fill.UniformColor = convertColor(s.Fill.UniformColor);
					}
				}
			}
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
