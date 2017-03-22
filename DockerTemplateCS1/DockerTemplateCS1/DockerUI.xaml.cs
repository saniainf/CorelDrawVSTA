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
using System.Text.RegularExpressions;

namespace DockerTemplateCS1
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
        }

        private void doSmartTrimMark(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;
            
            corel.Rect rect = new corel.Rect();
            ShapeRange sr = new ShapeRange();
            sr = corelApp.ActiveSelectionRange;
            corel.Shape line;
            OutlineStyle oStyle = corelApp.OutlineStyles[0];

            foreach (corel.Shape s in sr)
            {
                rect = s.BoundingBox;
                if (checkPoint(sr, rect.Left + 1, rect.Bottom - 4))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Left + 1, rect.Bottom, rect.Left + 1, rect.Bottom - 4);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Right - 1, rect.Bottom - 4))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Right - 1, rect.Bottom, rect.Right - 1, rect.Bottom - 4);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Left - 4, rect.Bottom + 1))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Left, rect.Bottom + 1, rect.Left - 4, rect.Bottom + 1);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Left - 4, rect.Top - 1))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Left, rect.Top - 1, rect.Left - 4, rect.Top - 1);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Left + 1, rect.Top + 4))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Left + 1, rect.Top, rect.Left + 1, rect.Top + 4);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Right - 1, rect.Top + 4))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Right - 1, rect.Top, rect.Right - 1, rect.Top + 4);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Right + 4, rect.Top - 1))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Right, rect.Top - 1, rect.Right + 4, rect.Top - 1);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
                if (checkPoint(sr, rect.Right + 4, rect.Bottom + 1))
                {
                    line = corelApp.ActiveLayer.CreateLineSegment(rect.Right, rect.Bottom + 1, rect.Right + 4, rect.Bottom + 1);
                    line.Outline.SetProperties(0.0762, oStyle, corelApp.CreateRegistrationColor());
                }
            }
        }

        bool checkPoint(ShapeRange sr, double x, double y)
        {
            foreach (corel.Shape s in sr)
            {
                corel.Rect r = s.BoundingBox;
                if (r.IsPointInside(x, y))
                    return false;
            }
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(lbPresetsList.SelectedItem.ToString());
        }
    }
}
