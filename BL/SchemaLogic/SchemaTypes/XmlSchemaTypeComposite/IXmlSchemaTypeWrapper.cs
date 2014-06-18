using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite
{
    public interface IXmlSchemaTypeWrapper
    {
        string Name { get; }
        Type DotNetType { get; } // The restriction base, relevant only for simple type
    }
}
