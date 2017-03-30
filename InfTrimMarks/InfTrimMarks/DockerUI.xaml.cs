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
using System.Globalization;

namespace InfTrimMarks
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;
        private const string unitsStr = " mm";

        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
            tbOffset.Text = "1" + unitsStr;
            tbMarkHeight.Text = "4" + unitsStr;
        }

        private void doSmartTrimMark(object sender, RoutedEventArgs e)
        {
            corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;

            ShapeRange sr = new ShapeRange();
            sr = corelApp.ActiveSelectionRange;
            double offset;
            double markHeight;

            if (sr.Count == 0)
                return;
            double.TryParse(tbOffset.Text.Replace(unitsStr, ""), out offset);
            double.TryParse(tbMarkHeight.Text.Replace(unitsStr, ""), out markHeight);
            SmartTrimMark smtm = new SmartTrimMark(corelApp);
            smtm.DoSmartTrimMarksOneShoot(offset, markHeight, sr);
        }
    }
}
