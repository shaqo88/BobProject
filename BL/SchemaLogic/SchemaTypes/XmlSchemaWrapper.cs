using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.UtilityClasses;
using System.ComponentModel;
using System.Collections.Specialized;

namespace BL.SchemaLogic.SchemaTypes
{
    public enum NodeType { Element, Choice, Sequence, SequenceItem, NULL }

    public abstract class XmlSchemaWrapper : PropertyNotifyObject, INotifyHighLevelPropertyChanged
    {
        public event PropertyChangedEventHandler HighLevelPropertyChanged;

        #region Private Members

        private string m_name;

        private bool m_hasBeenDrilled;

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

        public bool HasBeenDrilled
        {
            get { return m_hasBeenDrilled; }
            private set { SetProperty(ref m_hasBeenDrilled, value); }
        }

        public virtual bool AllChildrenDrilled
        {
            get
            {
                foreach (var child in Children)
                {
                    if (!child.AllChildrenDrilled)
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
            PropertyChanged += XmlSchemaWrapper_PropertyChanged;
            Children.CollectionChanged += Children_CollectionChanged;
        }

        void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO : need to use Reset or Move or Replace?
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        ((XmlSchemaWrapper)item).HighLevelPropertyChanged += XmlSchemaWrapperChild_HighLevelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        ((XmlSchemaWrapper)item).HighLevelPropertyChanged -= XmlSchemaWrapperChild_HighLevelPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        void XmlSchemaWrapperChild_HighLevelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllChildAttributesFilled" || e.PropertyName == "AllChildrenDrilled")
            {
                RaisePropertyChangedEvent(e.PropertyName);
            }
        }

        void XmlSchemaWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HasBeenDrilled")
                RaiseHighLevelPropertyChanged("AllChildrenDrilled");
        }

        #endregion

        #region Methods

        protected abstract void InternalDrill();

        protected void RaiseHighLevelPropertyChanged(string propertyName)
        {
            if (HighLevelPropertyChanged != null)
                HighLevelPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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
