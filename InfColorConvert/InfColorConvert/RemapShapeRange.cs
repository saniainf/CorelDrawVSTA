using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;
using System.Diagnostics;
using System.Windows;

namespace InfColorConvert
{
	class RemapShapeRange
	{
		ICheckColor checkColor;
		IConvertColor convertColor;
		corel.ShapeRange shapeRange;
		bool applyFill = true;
		bool applyOutline = true;
		bool textAsStory = true;

		/// <summary>
		/// перекрасить массив шейпов
		/// </summary>
		/// <param name="checkColor">метод проверки</param>
		/// <param name="convertColor">метод конвертации</param>
		/// <param name="shapeRange">массив шейпов</param>
		/// <param name="fill">применить к заливке</param>
		/// <param name="outline">применить к обводке</param>
		/// <param name="textAsStory">текст посимвольно</param>
		public RemapShapeRange(ICheckColor checkColor, IConvertColor convertColor, corel.ShapeRange shapeRange, bool fill, bool outline, bool textAsStory)
		{
			this.checkColor = checkColor;
			this.convertColor = convertColor;
			this.shapeRange = shapeRange;
			this.applyFill = fill;
			this.applyOutline = outline;
			this.textAsStory = textAsStory;
		}

		public void Start()
		{
			RemapInShapeRange(shapeRange);
		}

		private void RemapInShapeRange(corel.ShapeRange sr)
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
			RemapInShapeRange(s.Shapes.All());
		}

		private void RemapInPowerClipShape(corel.Shape s)
		{
			corel.ShapeRange sr = s.PowerClip.Shapes.All();
			s.PowerClip.EnterEditMode();
			RemapInShapeRange(sr);
			s.PowerClip.LeaveEditMode();
		}

		// convert

		private void RemapCdrSimpleShape(corel.Shape s)
		{
			if (applyFill && s.CanHaveFill)
			{
				switch (s.Fill.Type)
				{
					case cdrFillType.cdrFountainFill:
						RemapFountainFill(s.Fill.Fountain);
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
						RemapUniformFill(s.Fill);
						break;
					default:
						break;
				}
			}

			if (applyOutline && s.CanHaveOutline)
			{
				switch (s.Outline.Type)
				{
					case cdrOutlineType.cdrEnhancedOutline:
						RamapOutline(s.Outline);
						break;
					case cdrOutlineType.cdrNoOutline:
						break;
					case cdrOutlineType.cdrOutline:
						RamapOutline(s.Outline);
						break;
					default:
						break;
				}
			}
		}

		private void RemapCdrTextShape(corel.Shape s)
		{
			if (textAsStory)
			{
				RemapCdrSimpleShape(s);
			}

			else
			{
				foreach (corel.TextRange tr in s.Text.Story.Characters)
				{
					if (applyFill)
						switch (tr.Fill.Type)
						{
							case cdrFillType.cdrFountainFill:
								RemapFountainFill(tr.Fill.Fountain);
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
								RemapUniformFill(tr.Fill);
								break;
							default:
								break;
						}

					if (applyOutline)
						switch (tr.Outline.Type)
						{
							case cdrOutlineType.cdrEnhancedOutline:
								RamapOutline(tr.Outline);
								break;
							case cdrOutlineType.cdrNoOutline:
								break;
							case cdrOutlineType.cdrOutline:
								RamapOutline(tr.Outline);
								break;
							default:
								break;
						}
				}
			}
		}

		private void RemapUniformFill(corel.Fill fill)
		{
			if (checkColor.Check(fill.UniformColor))
				fill.UniformColor = convertColor.Convert(fill.UniformColor);
		}

		private void RemapFountainFill(corel.FountainFill fill)
		{
			for (int i = 0; i < fill.Colors.Count; i++)
			{
				if (checkColor.Check(fill.Colors[i].Color))
					fill.Colors[i].Color = convertColor.Convert(fill.Colors[i].Color);
			}
		}

		private void RamapOutline(corel.Outline outline)
		{
			if (checkColor.Check(outline.Color))
				outline.Color = convertColor.Convert(outline.Color);
		}



	}
}
