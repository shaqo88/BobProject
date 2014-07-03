using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public enum NodeType { Element, Choice, Sequence, SequenceItem, NULL }

    /// <summary>
    /// For <choice> and <sequence> tags
    /// </summary>
    public abstract class XmlSchemaGroupBaseWrapper : XmlSchemaWrapper
    {
        protected event Action OnGroupDrill;
        private XmlSchemaGroupBase Group { get; set; }
        public override bool IsDrillable { get { return true; } }

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group, NodeType nodeType, XmlSchemaWrapper parent)
            : base(nodeType.ToString(), nodeType, parent)
        {
            Group = group;
        }

        public static XmlSchemaGroupBaseWrapper SchemaGroupWrappersFactory(XmlSchemaGroupBase group, XmlSchemaWrapper parent)
        {
            if (group is XmlSchemaSequence)
                return new XmlSchemaSequenceArray(group as XmlSchemaSequence, parent);
            else if (group is XmlSchemaChoice)
                return new XmlSchemaChoiceWrapper(group as XmlSchemaChoice, parent);

            throw new Exception(string.Format("Group factory failed! Group could not be created, unknown type: {0}", group.GetType()));
        }

        protected override void InternalDrill()
        {
            foreach (var item in Group.Items)
            {
                if (item is XmlSchemaElement)
                {
                    Children.Add(new XmlSchemaElementWrapper(item as XmlSchemaElement, this));
                }
                else if (item is XmlSchemaGroupBase)
                {
                    if (item is XmlSchemaChoice)
                    {
                        Children.Add(new XmlSchemaChoiceWrapper(item as XmlSchemaChoice, this));
                    }
                    else if (item is XmlSchemaSequence)
                    {
                        Children.Add(new XmlSchemaSequenceArray(item as XmlSchemaSequence, this));
                    }
                    ((XmlSchemaGroupBaseWrapper)Children.Last()).DrillOnce();
                }
            }

            if (OnGroupDrill != null)
                OnGroupDrill();
        }
    }

    public class XmlSchemaNullChoice : XmlSchemaWrapper
    {
        public XmlSchemaNullChoice(XmlSchemaChoiceWrapper parent)
            : base("NULL", NodeType.NULL, parent, true)
        {
        }

        public override bool IsDrillable
        {
            get { return false; }
        }

        protected override void InternalDrill()
        {
        }
    }

    public class XmlSchemaChoiceWrapper : XmlSchemaGroupBaseWrapper
    {
        private XmlSchemaChoice m_choice;

        public XmlSchemaWrapper Selected { get; set; }

        protected override void InternalDrill()
        {
            // Check if this choice is optional, if so - add NULL by default
            if (m_choice.MinOccurs == 0)
                Children.Add(new XmlSchemaNullChoice(this));

            base.InternalDrill();
        }

        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice, XmlSchemaWrapper parent)
            : base(choice, NodeType.Choice, parent)
        {
            m_choice = choice;
            OnGroupDrill += XmlSchemaChoiceWrapper_OnGroupDrill;
        }

        private void XmlSchemaChoiceWrapper_OnGroupDrill()
        {
            if (Children.Count != 0)
                Selected = Children[0];
        }

        public override string ToString()
        {
            return base.ToString() + "->" + Selected;
        }
    }

    public class XmlSchemaSequenceArray : XmlSchemaGroupBaseWrapper, IList<XmlSchemaSequenceWrapper>//, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private XmlSchemaSequence m_sequence;

        public XmlSchemaSequenceArray(XmlSchemaSequence sequence, XmlSchemaWrapper parent)
            : base(sequence, NodeType.Sequence, parent)
        {
            m_sequence = sequence;
        }

        #region IList<> Interface

        public int IndexOf(XmlSchemaSequenceWrapper item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, XmlSchemaSequenceWrapper item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public XmlSchemaSequenceWrapper this[int index]
        {
            get
            {
                return (XmlSchemaSequenceWrapper)Children[index];
            }
            set
            {
                Children[index] = value;

            }
        }

        public void Add(XmlSchemaSequenceWrapper item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(XmlSchemaSequenceWrapper item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(XmlSchemaSequenceWrapper[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Children.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(XmlSchemaSequenceWrapper item)
        {
            return Children.Remove(item);
        }

        public IEnumerator<XmlSchemaSequenceWrapper> GetEnumerator()
        {
            return (IEnumerator<XmlSchemaSequenceWrapper>)Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #endregion

        protected override void InternalDrill()
        {
            foreach (var seq in Children)
            {
                seq.DrillOnce();
            }
        }

        public void AddNewWrapper()
        {
            var newSeq = new XmlSchemaSequenceWrapper(m_sequence, this, this.Count);
            this.Add(newSeq);
            newSeq.DrillOnce();
        }

        // TODO : Need those interfaces for WPF ??
        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        //public event PropertyChangedEventHandler PropertyChanged;
    }

    public class XmlSchemaSequenceWrapper : XmlSchemaGroupBaseWrapper
    {
        public int Index { get; private set; }
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence, XmlSchemaSequenceArray parent, int index)
            : base(sequence, NodeType.SequenceItem, parent)
        {
            Index = index;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}
