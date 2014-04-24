using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic
{
    public class XmlSchemaChoiceWrapper : XmlSchemaGroupBaseWrapper
    {
        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice)
            : base(choice)
        {
        }
    }

    public class XmlSchemaSequenceWrapper : XmlSchemaGroupBaseWrapper
    {
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence)
            : base(sequence)
        {
        }
    }
    /// <summary>
    /// For <choice> and <sequence> tags
    /// </summary>
    public abstract class XmlSchemaGroupBaseWrapper : XmlSchemaWrapper
    {
        private XmlSchemaGroupBase Group { get; set; }

        public List<XmlSchemaWrapper> InnerItems { get; set; }

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group)
        {
            Group = group;
            InnerItems = new List<XmlSchemaWrapper>();
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
        }

        public List<XmlSchemaElementWrapper> IterateGroups(string offset, ref int index)
        {
            List<XmlSchemaElementWrapper> elements = new List<XmlSchemaElementWrapper>();

            foreach (var innerItem in this.InnerItems)
            {
                Console.WriteLine("{0}Element group type: {1}", offset, this.GetType());

                if (innerItem is XmlSchemaGroupBaseWrapper)
                {
                    offset += "----";
                    elements.AddRange((innerItem as XmlSchemaGroupBaseWrapper).IterateGroups(offset, ref index));
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
    }
}
