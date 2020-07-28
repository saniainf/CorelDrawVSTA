using System.Windows;
using System.Windows.Controls;
using corel = Corel.Interop.VGCore;
using Corel.Interop.VGCore;

namespace InfTrimMarks
{

    public partial class DockerUI : UserControl
    {
        private corel.Application corelApp;

        public DockerUI(corel.Application app)
        {
            this.corelApp = app;
            InitializeComponent();
            resetToDefault();
        }

        private void doSmartTrimMark(object sender, RoutedEventArgs e)
        {
            if (corelApp.Documents.Count == 0)
                return;

            corelApp.ActiveDocument.Unit = cdrUnit.cdrMillimeter;
            ShapeRange sr = new ShapeRange();
            sr = corelApp.ActiveSelectionRange;

            if (sr.Count == 0)
                return;

            if ((chxLeft.IsChecked ?? false) &&
                (chxRight.IsChecked ?? false) &&
                (chxTop.IsChecked ?? false) &&
                (chxBottom.IsChecked ?? false))
                return;

            SmartTrimMark smtm = new SmartTrimMark(corelApp);
            corelApp.Optimization = true;

            var props = new DoMarksProperties(
                tbOffset.Value,
                tbMarkHeight.Value,
                chxOneShootCut.IsChecked ?? false,
                chxCanDecrease.IsChecked ?? false,
                chxWhiteSubMark.IsChecked ?? false,
                chxLeft.IsChecked ?? false,
                chxRight.IsChecked ?? false,
                chxTop.IsChecked ?? false,
                chxBottom.IsChecked ?? false);

            smtm.DoSmartTrimMarks(props, sr);

            corelApp.ActiveDocument.ClearSelection();
            corelApp.Optimization = false;
            corelApp.ActiveWindow.Refresh();
            corelApp.Refresh();
        }

        private void resetToDefaultClick(object sender, RoutedEventArgs e)
        {
            resetToDefault();
        }

        private void resetToDefault()
        {
            chxOneShootCut.IsChecked = false;
            chxWhiteSubMark.IsChecked = true;
            chxCanDecrease.IsChecked = true;
            chxTop.IsChecked = true;
            chxBottom.IsChecked = false;
            chxLeft.IsChecked = false;
            chxRight.IsChecked = false;
            tbOffset.Value = 1f;
            tbMarkHeight.Value = 4f;
        }
    }
}
