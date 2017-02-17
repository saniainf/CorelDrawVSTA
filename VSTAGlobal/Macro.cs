using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.Xaml;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corel.Interop.VGCore;
using System.Windows.Interop;


namespace VSTAGlobal
{
    public partial class Main
    {
        private void Startup()
        {

        }

        [CgsAddInMacro]
        public void Macro1()
        {
            app.ActiveDocument.Unit = cdrUnit.cdrMillimeter;

            IntPtr ip = (IntPtr)app.AppWindow.Handle;
            UserControl1 uc1 = new UserControl1();
            System.Windows.Window w = new System.Windows.Window();
            WindowInteropHelper wih = new WindowInteropHelper(w);
            wih.Owner = ip;
            w.Content = uc1;
            w.Show();
        }
    }
}
