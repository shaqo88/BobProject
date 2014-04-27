using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema; //hello

namespace DAL.SchemaDataAccess
{
    public class XsdReader
    {
        public XmlSchema Schema { get; set; }

        public string Path { get; set; }

        public XsdReader(string schemaPath)
        {
            Path = schemaPath;

            var schemaSet = CompileSchema(schemaPath);
            // Retrieve the compiled XmlSchema object from the XmlSchemaSet 
            // by iterating over the Schemas property.
            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                Schema = schema;
            }
        }

        /// <summary>
        /// Validates that a given schema is valid
        /// </summary>
        /// <param name="schemaPath">Path of the *.xsd file to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateSchema(string schemaPath)
        {
            try
            {
                CompileSchema(schemaPath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether a schema is valid and creates XmlSchemaSet object if valid
        /// </summary>
        /// <param name="schemaPath">Path of the schema</param>
        /// <returns>XmlSchemaSet object that describes the schema if valid, throws excpetion otherwise</returns>
        private static XmlSchemaSet CompileSchema(string schemaPath)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet();

            schemaSet.Add(null, schemaPath);
            schemaSet.Compile();

            return schemaSet;
        }
    }
}
