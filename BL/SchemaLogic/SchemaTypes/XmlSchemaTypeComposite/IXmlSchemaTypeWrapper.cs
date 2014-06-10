using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite
{
    public interface IXmlSchemaTypeWrapper
    {
        string Name { get; }
        //XmlSchemaType SchemaType { get; }
        Type DotNetType { get; } // The restriction base, relevant only for simple type

        void PrintAttrs(string offset);
        ObservableCollection<XmlSchemaAttributeInfo> Attributes { get; }
        XmlSchemaGroupBaseWrapper DrillOnce(XmlSchemaElementWrapper parent);
    }
}
