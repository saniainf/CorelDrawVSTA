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
        }

        private void beginAction(actionMethod method)
        {
            if (corelApp.Documents.Count == 0)
                return;
            corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;

            if (chxOnAllPage.IsChecked ?? false)
                foreach (corel.Page page in corelApp.ActiveDocument.Pages)
                    forEachShapeOnShapeRange(method, page.Shapes.All());
            else
                forEachShapeOnShapeRange(method, corelApp.ActivePage.Shapes.All());
        }

        private void forEachShapeOnShapeRange(actionMethod method, corel.ShapeRange sr)
        {
            corel.Shape s;
            while (sr.Count > 0)
            {
                s = sr[1];
                sr.Remove(1);
                if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrGroupShape)
                    groupShape(method, s);
                if (s.PowerClip != null)
                    powerClipShape(method, s);
                method(s);
            }
        }

        private void groupShape(actionMethod method, corel.Shape s)
        {
            forEachShapeOnShapeRange(method, s.Shapes.All());
        }

        private void powerClipShape(actionMethod method, corel.Shape s)
        {
            //if (s.Fill.Type != Corel.Interop.VGCore.cdrFillType.cdrNoFill)
            //{
            //    //corel.Shape copyShape = s.Duplicate().;

            //    //s.PowerClip.EnterEditMode();
            //}
            forEachShapeOnShapeRange(method, s.PowerClip.Shapes.All());
        }

        // convert methods

        private void textToCurves(corel.Shape s)
        {
            if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrTextShape)
                s.ConvertToCurves();
        }

        private void oleShapesToCurves(corel.Shape s)
        {
            corel.Rect rect;
            if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrOLEObjectShape)
            {
                rect = s.BoundingBox;
                s.Cut();
                corelApp.ActiveLayer.PasteSpecial("Metafile");
                s = corelApp.ActiveSelection;
                s.SetPosition(rect.Left, rect.Top);
                s.SetSize(rect.Width, rect.Height);
                forEachShapeOnShapeRange(textToCurves, s.Shapes.All());
            }
        }

        private void uniformFillToCMYK(corel.Shape s)
        {
            if (s.Fill.Type == corel.cdrFillType.cdrUniformFill)
                if (s.Fill.UniformColor.Type != corel.cdrColorType.cdrColorCMYK)
                    s.Fill.UniformColor.ConvertToCMYK();
        }

        private void outlineFillToCMYK(corel.Shape s)
        {
            if (s.Outline.Type == corel.cdrOutlineType.cdrOutline)
                if (s.Outline.Color.Type != corel.cdrColorType.cdrColorCMYK)
                    s.Outline.Color.ConvertToCMYK();
        }

        private void fountainFillToCMYK(corel.Shape s)
        {
            if (s.Fill.Type == corel.cdrFillType.cdrFountainFill)
            {
                for (int i = 0; i < s.Fill.Fountain.Colors.Count; i++)
                {
                    if (s.Fill.Fountain.Colors[i].Color.Type != corel.cdrColorType.cdrColorCMYK)
                        s.Fill.Fountain.Colors[i].Color.ConvertToCMYK();
                }
            }
        }

        private void bitmapToCMYK(corel.Shape s)
        {
            if (s.Type == corel.cdrShapeType.cdrBitmapShape)
                if (s.Bitmap.Mode != corel.cdrImageType.cdrCMYKColorImage)
                    s.Bitmap.ConvertTo(corel.cdrImageType.cdrCMYKColorImage);
        }

        private void testMethod(corel.Shape s)
        {
            s.Fill.UniformColor.CMYKAssign(0, 100, 50, 0);
        }

        // events

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
    }
}
