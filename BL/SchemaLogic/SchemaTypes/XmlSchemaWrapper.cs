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

    public abstract class XmlSchemaWrapper : PropertyNotifyObject
    {
        #region Private Members

        private string m_name;

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return m_name;
            }
            private set
            {
                SetProperty(ref m_name, value);
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

        #endregion

        #region Constructor

        public XmlSchemaWrapper(string name, NodeType nodeType, XmlSchemaWrapper parent, bool nonDrillable = false)
        {
            this.Name = name;
            this.Parent = parent;
            Children = new ObservableCollection<XmlSchemaWrapper>();
            HasBeenDrilled = nonDrillable;
            NodeType = nodeType;
        }

        #endregion

        #region Methods

        protected abstract void InternalDrill();

        public override string ToString()
        {
            return Name;
        }


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

        #endregion
    }
}
