using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corel.Interop.VGCore;
using corel = Corel.Interop.VGCore;
using System.IO;
using System.Xml;

namespace InfColorConvert
{
	class XmlPaletteService
	{
		private corel.Application corelApp;

		private string palettePantonePlusPath;
		private string palettePreviousPath;

		private List<PaletteListItem> palettes = new List<PaletteListItem>();

		public XmlPaletteService(corel.Application corelApp)
		{
			this.corelApp = corelApp;

			palettePantonePlusPath = corelApp.SetupPath + @"Color\Palettes\Spot\PANTONE\PANTONE+\";
			palettePreviousPath = corelApp.SetupPath + @"Color\Palettes\Spot\PANTONE\Previous Version\";
		}

		public List<PaletteListItem> LoadPalettes()
		{
			pantonePlusLoad();
			pantonePreviousLoad();

			return palettes;
		}

		private void pantonePlusLoad()
		{
			DirectoryInfo dir = new DirectoryInfo(palettePantonePlusPath);

			foreach (FileInfo file in dir.GetFiles("*.xml"))
			{
				XmlDocument xmlDoc = new XmlDocument();

				xmlDoc.Load(file.FullName);
				XmlElement xRoot = xmlDoc.DocumentElement;
				string name = xRoot.GetAttribute("name");
				if (name.Contains("Coated"))
					palettes.Add(new PaletteListItem { Title = name, Id = xRoot.GetAttribute("guid") });
			}
		}

		private void pantonePreviousLoad()
		{
			XmlDocument xmlDoc = new XmlDocument();

			xmlDoc.Load(palettePreviousPath + "panmetlc.xml");
			XmlElement xRoot = xmlDoc.DocumentElement;
			palettes.Add(new PaletteListItem { Title = xRoot.GetAttribute("name"), Id = xRoot.GetAttribute("guid") });
		}
	}
}
