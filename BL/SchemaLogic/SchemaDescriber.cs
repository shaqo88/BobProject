using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using DAL.SchemaDataAccess;
using BL.SchemaLogic.SchemaTypes;

namespace BL.SchemaLogic
{
    public class SchemaDescriber
    {
        private XsdReader XsdReader { get; set; }

        public List<XmlSchemaElementWrapper> Elements { get; set; }

        public SchemaDescriber(string schemaPath)
        {
            XsdReader = new XsdReader(schemaPath);

            Elements = new List<XmlSchemaElementWrapper>();

            foreach (var element in XsdReader.Schema.Elements.Values)
            {
                var wrappedElement = new XmlSchemaElementWrapper((XmlSchemaElement)element, null);
                Elements.Add(wrappedElement);
                wrappedElement.DrillOnce();
            }
        }

        public bool ValidateSchema(string schemaPath)
        {
            return XsdReader.ValidateSchema(schemaPath);
        }
    }
}
