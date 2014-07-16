using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

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
        public static bool ValidateSchema(string schemaPath, bool throwException)
        {
            try
            {
                CompileSchema(schemaPath);
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw ex;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given XmlDocument matches the current loaded XSD schema
        /// </summary>
        /// <param name="doc">The Xml object to check</param>
        /// <param name="errorMessage">Error message to fill in case of error</param>
        /// <returns>True if XML matches schema, false if not</returns>
        public bool IsXmlMatchSchema(XmlDocument doc, out string errorMessage)
        {
            // Temp string because out parameter cannot be modified in lambda
            string tempMessage = string.Empty;
            
            errorMessage = string.Empty;
            bool result = true;

            // Add the schema to XML object and validate mathchingg
            doc.Schemas.Add(Schema);

            doc.Validate((o, e) =>
                     {
                         tempMessage = e.Message;
                         result = false;
                     });

            errorMessage = tempMessage;
            
            return result;
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
