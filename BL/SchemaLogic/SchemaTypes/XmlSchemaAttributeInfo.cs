using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaAttributeInfo
    {
        public string Name { get; set; }

        public XmlSchemaUse Use { get; set; }

        public bool IsRequired { get { return Use == XmlSchemaUse.Required; } }

        public bool IsAttributeValid { get { return !IsRequired || IsAttributeFilled; } }

        public bool IsAttributeFilled { get { return Value != null && Value != string.Empty; } }

        public Type SimpleType { get; set; }

        public string Value { get; set; }
    }
}
