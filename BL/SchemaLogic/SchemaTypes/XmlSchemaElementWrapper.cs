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

        public bool IsSimple { get { return ElementObject.ElementSchemaType is XmlSchemaSimpleType; } }

        public string InnerText { get; set; }

        public Type DotNetType { get; private set; }

        public decimal MinOccurs { get; set; }

        public string MaxOccursString { get; set; }

        public ObservableCollection<XmlSchemaAttributeInfo> Attributes { get; private set; }

        public bool AllAttributesFilled
        {
            get
            {
                foreach (var attr in Attributes)
                {
                    if (!attr.IsAttributeValid)
                        return false;
                }

                return true;
            }
        }

        // TODO : doesn't work yet
        public override bool AllChildAttributesFilled
        {
            get
            {
                if (!this.AllAttributesFilled)
                    return false;

                return base.AllChildAttributesFilled;
            }
        }

        public override bool IsDrillable { get { return GetDrillableComplexType() != null; } }

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

        protected override void InternalDrill()
        {
            var groupBase = GetDrillableComplexType();

            if (groupBase == null)
                return;

            // Element is complex type and has a group under it, add it to children (it's the only child) and drill it
            var group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(groupBase, this);
            Children.Add(group);
            group.DrillOnce();
        }

        private XmlSchemaGroupBase GetDrillableComplexType()
        {
            if (Type is XmlSchemaComplexTypeWrapper)
            {
                var complexType = Type as XmlSchemaComplexTypeWrapper;

                if (complexType.SchemaType.ContentTypeParticle is XmlSchemaSequence ||
                    complexType.SchemaType.ContentTypeParticle is XmlSchemaChoice)
                    return complexType.SchemaType.ContentTypeParticle as XmlSchemaGroupBase;
            }

            return null;
        }
    }
}
