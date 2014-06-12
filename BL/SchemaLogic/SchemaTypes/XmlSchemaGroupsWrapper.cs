using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public enum NodeType { Element, Choice, Sequence }

    public class XmlSchemaChoiceWrapper : XmlSchemaGroupBaseWrapper
    {

        public XmlSchemaWrapper Selected { get; set; }

        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice)
            : base(choice, NodeType.Choice)
        {
            OnGroupDrill += XmlSchemaChoiceWrapper_OnGroupDrill;
        }

        private void XmlSchemaChoiceWrapper_OnGroupDrill()
        {
            if (InnerItems.Count != 0)
                Selected = InnerItems[0];
        }

        public override string ToString()
        {
            return base.ToString() + "->" + Selected;
        }
    }

    public class XmlSchemaSequenceWrapper : XmlSchemaGroupBaseWrapper
    {
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence)
            : base(sequence, NodeType.Sequence)
        {
        }
    }
    /// <summary>
    /// For <choice> and <sequence> tags
    /// </summary>
    public abstract class XmlSchemaGroupBaseWrapper : XmlSchemaWrapper
    {
        protected event Action OnGroupDrill;
        private XmlSchemaGroupBase Group { get; set; }

        public ObservableCollection<XmlSchemaWrapper> InnerItems { get; set; }

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group, NodeType nodeType)
            : base(nodeType.ToString(), nodeType)
        {
            Group = group;
            InnerItems = new ObservableCollection<XmlSchemaWrapper>();
        }

        public static XmlSchemaGroupBaseWrapper SchemaGroupWrappersFactory(XmlSchemaGroupBase group)
        {
            if (group is XmlSchemaSequence)
                return new XmlSchemaSequenceWrapper(group as XmlSchemaSequence);
            else if (group is XmlSchemaChoice)
                return new XmlSchemaChoiceWrapper(group as XmlSchemaChoice);

            return null;
        }

        public void DrillOnce(XmlSchemaElementWrapper parent)
        {
            foreach (var item in Group.Items)
            {
                if (item is XmlSchemaElement)
                {
                    InnerItems.Add(new XmlSchemaElementWrapper(item as XmlSchemaElement, parent));
                }
                else if (item is XmlSchemaGroupBase)
                {
                    if (item is XmlSchemaChoice)
                    {
                        InnerItems.Add(new XmlSchemaChoiceWrapper(item as XmlSchemaChoice));
                    }
                    else if (item is XmlSchemaSequence)
                    {
                        InnerItems.Add(new XmlSchemaSequenceWrapper(item as XmlSchemaSequence));
                    }
                    ((XmlSchemaGroupBaseWrapper)InnerItems.Last()).DrillOnce(parent);
                }
            }

            if (OnGroupDrill != null)
                OnGroupDrill();
        }

        public ObservableCollection<XmlSchemaElementWrapper> IterateGroups(string offset, ref int index)
        {
            ObservableCollection<XmlSchemaElementWrapper> elements = new ObservableCollection<XmlSchemaElementWrapper>();

            foreach (var innerItem in this.InnerItems)
            {
                Console.WriteLine("{0}Element group type: {1}", offset, this.GetType());

                if (innerItem is XmlSchemaGroupBaseWrapper)
                {
                    offset += "----";
                    var innerItems = (innerItem as XmlSchemaGroupBaseWrapper).IterateGroups(offset, ref index);
                    foreach (var i in innerItems)
                        elements.Add(i);
                }
                else
                {
                    if (innerItem is XmlSchemaElementWrapper)
                    {
                        var element = innerItem as XmlSchemaElementWrapper;
                        elements.Add(element);
                        element.PrintElement(offset, index);
                        ++index;
                    }
                }
            }

            return elements;
        }
    }

    public abstract class XmlSchemaWrapper
    {
        public string Name { get; private set; }

        public NodeType NodeType { get; protected set; }

        public XmlSchemaWrapper(string name, NodeType nodeType)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
