using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
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

        public ObservableCollection<XmlSchemaWrapper> InnerItems { get; set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group)
        {
            Group = group;
            InnerItems = new ObservableCollection<XmlSchemaWrapper>();
            Name = "Group";
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
    }
}
