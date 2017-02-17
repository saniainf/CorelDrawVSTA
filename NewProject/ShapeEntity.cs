using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject1
{
    class ShapeEntity
    {
        public Shape Shape { get; set; }
        public int Page { get; set; }
        public string DisplayName { get { return Shape.Type.ToString() + " on Page: " + Page; } }
    }
}
