using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite
{
    public class XmlSchemaComplexTypeWrapper : IXmlSchemaTypeWrapper
    {
        public string Name { get; private set; }

        public XmlSchemaComplexType SchemaType { get; private set; }

        public Type DotNetType { get; private set; } // The restriction base, relevant only for simple type

        public XmlSchemaComplexType ComplexType { get; set; }

        public IXmlSchemaTypeWrapper InnerType { get; set; }

        public List<XmlSchemaAttributeInfo> Attributes { get; private set; }

        public XmlSchemaComplexTypeWrapper(XmlSchemaComplexType complexType)
        {
            this.Name = complexType.Name;
            this.SchemaType = complexType;
            this.ComplexType = complexType;
            GetAllAttributes();
        }

        private void GetAllAttributes()
        {
            Attributes = GetAllAttributes(this);
        }

        private static List<XmlSchemaAttributeInfo> GetAllAttributes(IXmlSchemaTypeWrapper type)
        {
            var result = new List<XmlSchemaAttributeInfo>();

            if (type is XmlSchemaComplexTypeWrapper)
            {
                // <complexType>
                var compType = type as XmlSchemaComplexTypeWrapper;
                var compSchemaType = compType.SchemaType as XmlSchemaComplexType;
                foreach (var attr in compSchemaType.Attributes)
                {
                    var schemaAttr = attr as XmlSchemaAttribute;
                    result.Add(new XmlSchemaAttributeInfo() { Name = schemaAttr.Name, Use = schemaAttr.Use, SimpleType = schemaAttr.AttributeSchemaType.Datatype.ValueType });
                }

                if (compSchemaType.BaseXmlSchemaType.Name != null)
                {
                    compType.InnerType = XmlSchemaSimpleTypeWrapper.SchemaWrappersFactory(compSchemaType.BaseXmlSchemaType);
                    result.AddRange(GetAllAttributes(compType.InnerType));
                }

                if (compSchemaType.ContentModel != null)
                {
                    if (compSchemaType.ContentModel.Content is XmlSchemaComplexContentExtension)
                    {
                        var extension = compSchemaType.ContentModel.Content as XmlSchemaComplexContentExtension;
                        foreach (var attr in extension.Attributes)
                        {
                            var schemaAttr = attr as XmlSchemaAttribute;
                            result.Add(new XmlSchemaAttributeInfo() { Name = schemaAttr.Name, Use = schemaAttr.Use, SimpleType = schemaAttr.AttributeSchemaType.Datatype.ValueType });
                        }
                    }
                }
            }

            return result;
        }

        public XmlSchemaGroupBaseWrapper DrillOnce(XmlSchemaElementWrapper parent)
        {
            XmlSchemaGroupBaseWrapper group = null;
            if (SchemaType.ContentTypeParticle != null)
            {
                if (SchemaType.ContentTypeParticle is XmlSchemaSequence)
                {
                    var seq = SchemaType.ContentTypeParticle as XmlSchemaSequence;
                    group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(seq);
                    group.DrillOnce(parent);
                }
                else if (SchemaType.ContentTypeParticle is XmlSchemaChoice)
                {
                    var choice = SchemaType.ContentTypeParticle as XmlSchemaChoice;
                    group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(choice);
                    group.DrillOnce(parent);
                }
            }

            return group;
        }

        //public XmlSchemaGroupBaseWrapper GetGroup()
        //{
        //    var compType = this.SchemaType as XmlSchemaComplexType;

        //    if (compType.ContentTypeParticle != null)
        //    {
        //        if (compType.ContentTypeParticle is XmlSchemaSequence)
        //        {
        //            var seq = compType.ContentTypeParticle as XmlSchemaSequence;
        //            return XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(seq);
        //        }
        //        else if (compType.ContentTypeParticle is XmlSchemaChoice)
        //        {
        //            var choice = compType.ContentTypeParticle as XmlSchemaChoice;
        //            return XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(choice);
        //        }
        //    }

        //    return null;
        //}

        public void PrintAttrs(string offset)
        {
            offset += "++";
            Console.WriteLine("{0}Attributes", offset);
            Console.WriteLine("{0}==========", offset);

            foreach (var attr in this.Attributes)
            {
                Console.WriteLine("{0}{1}, \t Type: {2}", offset, attr.Name, attr.SimpleType);
            }
            Console.WriteLine("{0}==========", offset);

        }
    }
}
