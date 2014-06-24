using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SchemaLogic.SchemaTypes
{
    public abstract class XmlSchemaWrapper
    {
        public string Name { get; private set; }

        public NodeType NodeType { get; protected set; }

        public ObservableCollection<XmlSchemaWrapper> Children { get; protected set; }

        public XmlSchemaWrapper Parent { get; set; }

        public abstract bool IsDrillable { get; }

        public XmlSchemaWrapper(string name, NodeType nodeType, XmlSchemaWrapper parent)
        {
            this.Name = name;
            this.Parent = parent;
            Children = new ObservableCollection<XmlSchemaWrapper>();
        }

        public override string ToString()
        {
            return Name;
        }

        public abstract void DrillOnce();
    }
}
