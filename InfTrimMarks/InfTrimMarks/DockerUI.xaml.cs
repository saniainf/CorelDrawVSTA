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

namespace InfTrimMarks
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        string prevtbText;

        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
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

        private void doSmartTrimMark(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;

            corel.Rect rect = new corel.Rect();
            ShapeRange sr = new ShapeRange();
            sr = corelApp.ActiveSelectionRange;

            if (sr.Count == 0)
                tbOffset.Text = "44";
            int offset = Convert.ToInt32(tbOffset.Text);
            int markHeight = Convert.ToInt32(tbMarkHeight.Text);

            foreach (corel.Shape s in sr)
            {
                rect = s.BoundingBox;
                drawLine(sr, rect.Left + offset, rect.Bottom, rect.Left + offset, rect.Bottom - 4);
                drawLine(sr, rect.Right - offset, rect.Bottom, rect.Right - offset, rect.Bottom - 4);
                drawLine(sr, rect.Left, rect.Bottom + offset, rect.Left - 4, rect.Bottom + offset);
                drawLine(sr, rect.Left, rect.Top - offset, rect.Left - 4, rect.Top - offset);
                drawLine(sr, rect.Left + offset, rect.Top, rect.Left + offset, rect.Top + 4);
                drawLine(sr, rect.Right - offset, rect.Top, rect.Right - offset, rect.Top + 4);
                drawLine(sr, rect.Right, rect.Top - offset, rect.Right + 4, rect.Top - offset);
                drawLine(sr, rect.Right, rect.Bottom + offset, rect.Right + 4, rect.Bottom + offset);
            }
        }

        private void drawLine(ShapeRange sr, double startX, double startY, double endX, double endY)
        {
            corel.Shape line;
            corel.Color color = corelApp.CreateRegistrationColor();
            OutlineStyle oStyle = corelApp.OutlineStyles[0];
            double width = 0.0762;

            if (checkPoint(sr, endX, endY))
            {
                line = corelApp.ActiveLayer.CreateLineSegment(startX, startY, endX, endY);
                line.Outline.SetProperties(width, oStyle, color);
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

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text != "")
                prevtbText = ((TextBox)sender).Text;
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text == "")
                ((TextBox)sender).Text = prevtbText;
        }
    }
}
