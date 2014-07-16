using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.UtilityClasses;

namespace BL.SchemaLogic.SchemaTypes
{
    public enum NodeType { Element, Choice, Sequence, SequenceItem, NULL }

    public abstract class XmlSchemaWrapper : ObservableObject
    {

        private string m_name;

        public string Name 
        {
            get 
            {
                return m_name; 
            }
            private set
            {
                m_name = value;
                RaisePropertyChangedEvent("Name");
            }
        }

        public NodeType NodeType { get; protected set; }

        public ObservableCollection<XmlSchemaWrapper> Children { get; protected set; }

        public XmlSchemaWrapper Parent { get; set; }

        public abstract bool IsDrillable { get; }

        public bool HasBeenDrilled { get; private set; }

        public bool AllChildrenDrilled
        {
            get
            {
                foreach (var child in Children)
                {
                    if ((child.IsDrillable && !child.HasBeenDrilled))
                        return false;
                }

                return !(IsDrillable && !HasBeenDrilled);
            }
        }

        public virtual bool AllChildAttributesFilled
        {
            get
            {
                foreach (var child in Children)
                {
                    if (!child.AllChildAttributesFilled)
                        return false;
                }

                return true;
            }
        }

        public XmlSchemaWrapper(string name, NodeType nodeType, XmlSchemaWrapper parent, bool nonDrillable = false)
        {
            this.Name = name;
            this.Parent = parent;
            Children = new ObservableCollection<XmlSchemaWrapper>();
            HasBeenDrilled = nonDrillable;
            NodeType = nodeType;
        }

        public override string ToString()
        {
            return Name;
        }

        protected abstract void InternalDrill();

        public void DrillOnce()
        {
            if (HasBeenDrilled)
                return;
            else
            {
                InternalDrill();
                HasBeenDrilled = true;
            }
        }
    }
}
