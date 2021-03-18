using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;
using System.Windows;

namespace InfColorConvert
{
	class ConvertColorSpacePantone : IConvertColor
	{
		private corel.Application corelApp;
		private List<string> palettesID;
		private Dictionary<string, corel.Color> foundColors;

		public ConvertColorSpacePantone(corel.Application app, List<string> palettesID)
		{
			corelApp = app;
			//palettesID = new string[] {
			// "6e75244b-d853-4d2e-bea2-a5da3f195d08",	//PANTONE+ Solid Coated-V2
			// "978463a2-0b90-4f87-8d5b-7220fbd06768",	//PANTONE+ Pastels &amp; Neons Coated
			// "66570bd8-9070-44a3-91cd-a695987fc88b",	//PANTONE+ Premium Metallics Coated
			// "3ab7ce0c-8952-4838-8ebb-8fdfcf3d2c2a" };	//PANTONE® metallic coated
			this.palettesID = palettesID;
			foundColors = new Dictionary<string, Color>();
		}

		public corel.Color Convert(corel.Color color)
		{
			string colorName;

			if (color.Type == cdrColorType.cdrColorSpot)
			{
				colorName = color.Name;

				// если спот из таблиц пантонов
				if (colorName.Contains("PANTONE"))
				{
					colorName = colorName.Replace(" 2X", "");
					colorName = colorName.Substring(0, colorName.LastIndexOf(' '));

					//разные названия TransWhite
					if (!colorName.Contains("Trans. White"))
						colorName = colorName + " C";

					// исправление неправильного названия в PANTONE MATCHING SYSTEM Coated - Corel 10
					if (colorName == "PANTONE Relfex Blue C")
						colorName = "PANTONE Reflex Blue C";
				}
			}

			else
			{
				colorName = color.HexValue.ToString();
			}

			// поиск в найденных цветах
			if (foundColors.ContainsKey(colorName))
			{
				corel.Color c = foundColors[colorName];
				// если спот сохранить тинт
				if (color.Type == cdrColorType.cdrColorSpot && color.IsTintable && c.IsSpot && c.IsTintable)
				{
					c.Tint = color.Tint;
					return c;
				}
				else
					return c;
			}

			// поиск в палитрах корела если палитра Locked
			if (color.Type == cdrColorType.cdrColorSpot)
			{
				foreach (string id in palettesID)
				{
					if (color.PaletteIdentifier != id)
					{
						corel.Palette castPalette = corelApp.PaletteManager.GetPalette(id);
						int colorID = castPalette.FindColor(colorName);
						if (colorID != 0)
						{
							corel.Color c = new Color();
							c.PaletteAssign(castPalette.Identifier, colorID);
							foundColors.Add(c.Name.ToString(), c);
							c.Tint = color.Tint;
							return c;
						}
					}
					else
					{
						return color;
					}
				}

				// если нигде нет
				if (MessageBox.Show("В палитрах ненайден цвет:\n" + color.Name + "\n" + "Заменить вручную?", "Ненайден цвет", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
				{
					corel.Color c = new corel.Color();
					if (c.UserAssignEx())
					{
						// если спот учитывать тинт 
						foundColors.Add(colorName, c);
						if (c.IsSpot && c.IsTintable)
							c.Tint = color.Tint;
						return c;
					}
				}
				else
				{
					foundColors.Add(colorName, color);
					return color;
				}
			}

			else
			{
				corel.Color c = new corel.Color();
				c.CopyAssign(color);
				c.ConvertToPalette("6e75244b-d853-4d2e-bea2-a5da3f195d08");
				foundColors.Add(colorName, c);
				return c;
			}

			return color;
		}
	}
}
