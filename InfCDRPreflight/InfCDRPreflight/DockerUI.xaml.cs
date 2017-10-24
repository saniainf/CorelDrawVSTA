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
            corelApp.Optimization = true;
            if (corelApp.Documents.Count == 0)
                return;
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

        //private void beginAction(actionMethod method)
        //{
        //    if (corelApp.Documents.Count == 0)
        //        return;
        //    corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;

        //    if (chxOnAllPage.IsChecked ?? false)
        //        foreach (corel.Page page in corelApp.ActiveDocument.Pages)
        //            dfsTree(method, page.TreeNode);
        //    else
        //        dfsTree(method, corelApp.ActivePage.TreeNode);
        //}

        //private void dfsTree(actionMethod method, corel.TreeNode tn)
        //{

        //}

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
                //if (s.Type == corel.cdrShapeType.cdrSymbolShape)
                //{
                //    System.Windows.MessageBox.Show("Find symbol shape.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    break;
                //}
                method(s);
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

        // convert methods

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
            //beginAction(testMethod);

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
    }
}
