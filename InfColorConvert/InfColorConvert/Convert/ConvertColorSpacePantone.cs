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
		private string[] palettesId;
		private Dictionary<string, corel.Color> foundColors;

		public ConvertColorSpacePantone(corel.Application app)
		{
			corelApp = app;
			palettesId = new string[] {
			 "6e75244b-d853-4d2e-bea2-a5da3f195d08",	//PANTONE+ Solid Coated-V2
			 "978463a2-0b90-4f87-8d5b-7220fbd06768",	//PANTONE+ Pastels &amp; Neons Coated
			 "66570bd8-9070-44a3-91cd-a695987fc88b",	//PANTONE+ Premium Metallics Coated
			 "3ab7ce0c-8952-4838-8ebb-8fdfcf3d2c2a" };	//PANTONE® metallic coated
			foundColors = new Dictionary<string, Color>();

			corel.Color c = new Color();
		}

		public corel.Color Convert(corel.Color color)
		{
			string colorName;

			colorName = color.Name;
			colorName = colorName.Replace(" 2X", "");
			colorName = colorName.Substring(8, colorName.LastIndexOf(' ') - 8);
			colorName = "PANTONE " + colorName + " C";

			// исправление неправильного названия в PANTONE MATCHING SYSTEM Coated - Corel 10
			if (colorName == "PANTONE Relfex Blue C")
				colorName = "PANTONE Reflex Blue C";

			// поиск в найденных цветах
			if (foundColors.ContainsKey(colorName))
			{
				//System.Diagnostics.Debug.WriteLine("повтор");
				return foundColors[colorName];
			}

			// поиск в палитрах корела
			foreach (string id in palettesId)
			{
				if (color.PaletteIdentifier != id)
				{
					corel.Palette castPalette = corelApp.PaletteManager.GetPalette(id);
					int colorID = castPalette.FindColor(colorName);
					if (colorID != 0)
					{
						foundColors.Add(castPalette.get_Color(colorID).Name.ToString(), castPalette.get_Color(colorID));
						return castPalette.get_Color(colorID);
					}
				}
				else
				{
					return color;
				}
			}

			// если нигде нет
			if (MessageBox.Show("Ненайден цвет:\n" + colorName + "\n" + "Заменить вручную?", "Ненайден цвет", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
			{
				corel.Color c = new corel.Color();
				if (c.UserAssignEx())
				{
					foundColors.Add(colorName, c);
					return c;
				}
			}

			return color;
		}
	}
}
