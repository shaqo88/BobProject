using BL.SchemaLogic.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL.XmlLogic
{
    public static class XmlExportLogic
    {
        private static XmlDocument doc = new XmlDocument();

        /// <summary>
        /// Converts the BL objects of XmlSchemaWrapper types to XmlDocument to get ready for writing to file
        /// </summary>
        /// <param name="rootWrapper">The root object to start with the building</param>
        /// <returns>The XmlDocument which represents the translated tree</returns>
        public static XmlDocument SchemaWrapperToXmlDocument(XmlSchemaElementWrapper rootWrapper)
        {
            // TODO : add version from a global property
            // Declaration that is required in XMLs
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "iso-8859-8", null);
            XmlElement decElement = doc.DocumentElement;
            doc.InsertBefore(declaration, decElement);

            // Create the root object before recursion
            var rootXmlElement = CreateElementNode(rootWrapper);

            // Build all children recursively
            foreach (var child in rootWrapper.Children)
            {
                RecusiveXmlBuilder(child, rootXmlElement);
            }

            // Append the ready root with all its children to the document
            doc.AppendChild(rootXmlElement);

            return doc;
        }

        /// <summary>
        /// Builds recursively the XML object from the wrapper classes
        /// </summary>
        /// <param name="currWrapper">Current object to handle</param>
        /// <param name="parentXmlElement">The parent of the current object</param>
        private static void RecusiveXmlBuilder(XmlSchemaWrapper currWrapper, XmlElement parentXmlElement)
        {
            if (currWrapper is XmlSchemaElementWrapper)
            {
                // If element, add all its attributes, append it to the output and go recursively with all its children
                var elementWrapper = currWrapper as XmlSchemaElementWrapper;
                var xmlElement = CreateElementNode(elementWrapper);

                parentXmlElement.AppendChild(xmlElement);

                foreach (var child in elementWrapper.Children)
                {
                    RecusiveXmlBuilder(child, xmlElement);
                }

                // If the element requires inner text, fill it
                if (elementWrapper.IsSimple)
                    xmlElement.InnerText = elementWrapper.InnerText;
            }
            else if (currWrapper is XmlSchemaChoiceWrapper)
            {
                // For choice, just continue with recursion with the selected element
                var choice = currWrapper as XmlSchemaChoiceWrapper;

                if (!(choice.Selected is XmlSchemaNullChoice))
                    RecusiveXmlBuilder(choice.Selected, parentXmlElement);
            }
            else if (currWrapper is XmlSchemaSequenceWrapper)
            {
                // For a sequence item, go recursively with all its children
                var seq = currWrapper as XmlSchemaSequenceWrapper;

                foreach (var child in seq.Children)
                {
                    RecusiveXmlBuilder(child, parentXmlElement);
                }
            }
            else if (currWrapper is XmlSchemaSequenceArray)
            {
                // With the head array of the sequence, go recursively with its sequence items
                var seqArr = currWrapper as XmlSchemaSequenceArray;

                foreach (var seq in seqArr)
                {
                    RecusiveXmlBuilder(seq, parentXmlElement);
                }
            }
            else
            {
                throw new Exception(string.Format("Error while exporting XML. Unknown type: {0}", currWrapper.GetType()));
            }
        }

        /// <summary>
        /// Create Xml object from element wrapper
        /// </summary>
        /// <param name="elementWrapper">The element wrapper to convert</param>
        /// <returns>The XmlElement with all the wrapper's informaion</returns>
        private static XmlElement CreateElementNode(XmlSchemaElementWrapper elementWrapper)
        {
            XmlElement newXmlElement = doc.CreateElement(elementWrapper.Name);

            // Go over all element's attributes and add them to XML if they are filled
            foreach (var attr in elementWrapper.Attributes)
            {
                if (attr.IsAttributeValid)
                {
                    if (attr.IsAttributeFilled)
                    {
                        newXmlElement.SetAttribute(attr.Name, attr.Value);
                    }
                }
                else
                {
                    throw new Exception(string.Format("Error exporting XML. Node: {0} with Attribute: {1} is not filled correctly", elementWrapper.Name, attr.Name));
                }
            }

            return newXmlElement;
        }
    }
}
