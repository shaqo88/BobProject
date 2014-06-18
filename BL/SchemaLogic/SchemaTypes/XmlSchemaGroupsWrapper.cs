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

        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice, XmlSchemaWrapper parent)
            : base(choice, NodeType.Choice, parent)
        {
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

    public class XmlSchemaSequenceWrapper : XmlSchemaGroupBaseWrapper
    {
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence, XmlSchemaWrapper parent)
            : base(sequence, NodeType.Sequence, parent)
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

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group, NodeType nodeType, XmlSchemaWrapper parent)
            : base(nodeType.ToString(), nodeType, parent)
        {
            Group = group;
        }

        public static XmlSchemaGroupBaseWrapper SchemaGroupWrappersFactory(XmlSchemaGroupBase group, XmlSchemaWrapper parent)
        {
            if (group is XmlSchemaSequence)
                return new XmlSchemaSequenceWrapper(group as XmlSchemaSequence, parent);
            else if (group is XmlSchemaChoice)
                return new XmlSchemaChoiceWrapper(group as XmlSchemaChoice, parent);

            return null;
        }

        public override void DrillOnce()
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
                        Children.Add(new XmlSchemaSequenceWrapper(item as XmlSchemaSequence, this));
                    }
                    ((XmlSchemaGroupBaseWrapper)Children.Last()).DrillOnce();
                }
            }

            if (OnGroupDrill != null)
                OnGroupDrill();
        }
    }

    public abstract class XmlSchemaWrapper
    {
        public string Name { get; private set; }

        public NodeType NodeType { get; protected set; }

        public ObservableCollection<XmlSchemaWrapper> Children { get; protected set; }

        public XmlSchemaWrapper Parent { get; set; }

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
