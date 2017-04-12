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
        private delegate void delAction(corel.Shape s);
        static private Random rnd = new Random();

        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
        }

        private void startAction(delAction action)
        {
            if (corelApp.Documents.Count == 0)
                return;
            corelApp.ActiveDocument.Unit = corel.cdrUnit.cdrMillimeter;

            if (chxOnAllPage.IsChecked ?? false)
                foreach (corel.Page page in corelApp.ActiveDocument.Pages)
                    forEachShapeOnShapeRange(action, page.Shapes.All());
            else
                forEachShapeOnShapeRange(action, corelApp.ActivePage.Shapes.All());
        }

        private void forEachShapeOnShapeRange(delAction action, corel.ShapeRange sr)
        {
            corel.Shape s;
            while (sr.Count > 0)
            {
                s = sr[1];
                sr.Remove(1);
                if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrGroupShape)
                    forEachShapeOnShapeRange(action, s.Shapes.All());
                if (s.PowerClip != null)
                    forEachShapeOnShapeRange(action, s.PowerClip.Shapes.All());
                action(s);
            }
        }

        // convert methods

        private void textToCurves(corel.Shape s)
        {
            if (s.Type == Corel.Interop.VGCore.cdrShapeType.cdrTextShape)
                s.ConvertToCurves();
        }

        private void OLEshapesToCurves(corel.Shape s)
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

        // events

        private void btnTextToCurves_Click(object sender, RoutedEventArgs e)
        {
            delAction action = textToCurves;
            startAction(action);
        }

        private void btnOLEtoCurves_Click(object sender, RoutedEventArgs e)
        {
            delAction action = OLEshapesToCurves;
            startAction(action);
        }
    }
}
