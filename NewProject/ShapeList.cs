using Corel.Interop.VGCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewProject1
{
    class ShapeList
    {
        public ItemType ShapeType;
        public string Name; 
        public string DisplayName { get { return Name + ": " + Shapes.Count; } }
        public List<ShapeEntity> Shapes;

        public ShapeList(ItemType type, string name)
        {
            this.ShapeType = type;
            this.Name = name;
            Shapes = new List<ShapeEntity>();
        }
    }
}
