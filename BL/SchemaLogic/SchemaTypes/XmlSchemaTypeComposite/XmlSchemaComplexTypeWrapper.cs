﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public XmlSchemaComplexTypeWrapper(XmlSchemaComplexType complexType)
        {
            this.Name = complexType.Name;
            this.SchemaType = complexType;
            this.ComplexType = complexType;
        }

        public static ObservableCollection<XmlSchemaAttributeInfo> GetAllAttributes(IXmlSchemaTypeWrapper type)
        {
            var result = new ObservableCollection<XmlSchemaAttributeInfo>();

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
                    var innerTypes = GetAllAttributes(compType.InnerType);
                    foreach (var i in innerTypes)
                        result.Add(i);
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
    }
}
