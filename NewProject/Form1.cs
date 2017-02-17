using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corel.Interop.VGCore;

namespace NewProject1
{
    enum ItemType { Curves, OLEObj, BitMap };

    public partial class Form1 : Form
    {
        private Corel.Interop.VGCore.Application app;
        List<ShapeList> dataFindItem;

        public Form1(Corel.Interop.VGCore.Application app)
        {
            this.app = app;
            InitializeComponent();
            dataFindItem = new List<ShapeList>(Enum.GetNames(typeof(ItemType)).Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearDataFindItem();
            findShapes();
        }

        private void clearDataFindItem()
        {
            //ShapeList curves = new ShapeList();
        }

        private void findShapes()
        {
            foreach (Shape s in app.ActiveDocument.ActivePage.Shapes)
            {
                if (s.Type == cdrShapeType.cdrBitmapShape)
                {
                    //dataFindItem[(int)ItemType.bitmap].Shapes.Add(new ShapeEntity { Shape = s, Page = 1 });
                }
                if (s.Type == cdrShapeType.cdrOLEObjectShape)
                {
                    //dataFindItem[1].Shapes.Add(new ShapeEntity { Shape = s, Page = 1 });
                }
            }
            listFindItem.DataSource = dataFindItem;
            listFindItem.DisplayMember = "DisplayName";
        }

        private void listFindItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShapeList sl = (ShapeList)listFindItem.SelectedItem;
            listSelectItem.DataSource = sl.Shapes;
            listSelectItem.DisplayMember = "DisplayName";
        }

        private void listSelectItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShapeEntity se = (ShapeEntity)listSelectItem.SelectedItem;
            Shape s = se.Shape;
            app.ActiveDocument.ClearSelection();
            s.Selected = true;
        }
    }
}
