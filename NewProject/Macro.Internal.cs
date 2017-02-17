using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corel.Interop.VGCore;

namespace NewProject1
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CgsAddInModule : System.Attribute { };

    [System.AttributeUsage(System.AttributeTargets.Constructor)]
    public class CgsAddInConstructor : System.Attribute { };

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CgsAddInMacro : System.Attribute { };

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CgsAddInTool : System.Attribute
    {
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string guid;
        private string name;
    };

    [CgsAddInModule]
    public partial class Main
    {
        private Corel.Interop.VGCore.Application app;

        [CgsAddInConstructor]
        public Main(object _app)
        {
            app = _app as Corel.Interop.VGCore.Application;
            Startup();
            tool1 tl = new tool1();
        }
    }

    [CgsAddInTool(Guid = "asdfsdfasdf", Name = "asfasdf")]
    public class tool1
    {

    }
}
