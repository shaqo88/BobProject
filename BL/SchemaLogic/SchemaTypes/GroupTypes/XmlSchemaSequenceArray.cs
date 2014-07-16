using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaSequenceArray : XmlSchemaGroupBaseWrapper, IList<XmlSchemaSequenceWrapper>
    {
        private XmlSchemaSequence m_sequence;
        private int m_count;

        public XmlSchemaSequenceArray(XmlSchemaSequence sequence, XmlSchemaWrapper parent)
            : base(sequence, NodeType.Sequence, parent)
        {
            m_sequence = sequence;
            Children.CollectionChanged += Children_CollectionChanged;
            Count = Children.Count;
        }

        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Count = Children.Count;
            for (int i = 0; i < Children.Count; i++)
            {
                ((XmlSchemaSequenceWrapper)Children[i]).Index = i;
            }
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
            get { return m_count; }
            set { SetProperty(ref m_count, value); }
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
            return Children.ToList().ConvertAll((c) => (XmlSchemaSequenceWrapper)c).GetEnumerator();
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
    }
}
