using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace BL.SchemaLogic.XmlSchemaTypeComposite
{
    public interface IXmlSchemaTypeWrapper
    {
        string Name { get; }
        //XmlSchemaType SchemaType { get; }
        Type DotNetType { get; } // The restriction base, relevant only for simple type

        void PrintAttrs(string offset);
        List<XmlSchemaAttributeInfo> Attributes { get; }
        XmlSchemaGroupBaseWrapper DrillOnce(XmlSchemaElementWrapper parent);
    }
}
