using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corel.Interop.VGCore;

namespace NewProject1
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

            Form1 frm = new Form1(app);
            System.Windows.Forms.IWin32Window iw;
            IntPtr ip = (IntPtr)app.AppWindow.Handle;
            iw = System.Windows.Forms.Control.FromHandle(ip);
            frm.Show(iw);
        }
    }
}
