using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaElementWrapper : XmlSchemaWrapper
    {
        private XmlSchemaElement ElementObject { get; set; }

        private IXmlSchemaTypeWrapper Type { get; set; }

        public Type DotNetType { get; private set; }

        public decimal MinOccurs { get; set; }

        public string MaxOccursString { get; set; }

        private XmlSchemaGroupBaseWrapper Group { get; set; }

        public ObservableCollection<XmlSchemaAttributeInfo> Attributes { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public XmlSchemaElementWrapper(XmlSchemaElement element, XmlSchemaWrapper parent) :
            base(element.Name, NodeType.Element, parent)
        {
            ElementObject = element;
            MinOccurs = element.MinOccurs;
            MaxOccursString = element.MaxOccursString;
            Parent = parent;
            Type = XmlSchemaSimpleTypeWrapper.SchemaWrappersFactory(ElementObject.ElementSchemaType);
            Attributes = XmlSchemaComplexTypeWrapper.GetAllAttributes(Type);
            DotNetType = XmlSchemaSimpleTypeWrapper.GetDotNetType(Type);
        }

        public override void DrillOnce()
        {
            if (Type is XmlSchemaComplexTypeWrapper)
            {
                var complexType = Type as XmlSchemaComplexTypeWrapper;

                if (complexType.SchemaType.ContentTypeParticle != null)
                {
                    if (complexType.SchemaType.ContentTypeParticle is XmlSchemaSequence)
                    {
                        var seq = complexType.SchemaType.ContentTypeParticle as XmlSchemaSequence;
                        Group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(seq, this);
                        Children.Add(Group);
                        Group.DrillOnce();
                    }
                    else if (complexType.SchemaType.ContentTypeParticle is XmlSchemaChoice)
                    {
                        var choice = complexType.SchemaType.ContentTypeParticle as XmlSchemaChoice;
                        Group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(choice, this);
                        Children.Add(Group);
                        Group.DrillOnce();
                    }
                }
            }
        }
    }
}
