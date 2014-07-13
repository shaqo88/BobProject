using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using DAL.SchemaDataAccess;
using BL.SchemaLogic.SchemaTypes;
using System.Collections.ObjectModel;
using BL.XmlLogic;
using DAL.XmlWrapper;

namespace BL.SchemaLogic
{
    public class SchemaDescriber
    {
        private XsdReader XsdReader { get; set; }

        private XmlWrappersSearcher Searcher { get; set; }

        public ObservableCollection<XmlSchemaElementWrapper> Elements { get; private set; }

        public XmlSchemaElementWrapper RootElement
        {
            get
            {
                if (Elements != null && Elements.Count > 0)
                    return Elements[0];
                else
                    return null;
            }
        }

        public SchemaDescriber(string schemaPath)
        {
            LoadSchema(schemaPath);
        }

        /// <summary>
        /// Loads schema by validating it and creating all BL objects by it
        /// </summary>
        /// <param name="schemaPath">Path of the schema to load</param>
        public void LoadSchema(string schemaPath)
        {
            // Validate the schema and throw exception if fails (sent by the last parameter
            ValidateSchema(schemaPath, true);

            XsdReader = new XsdReader(schemaPath);

            Elements = new ObservableCollection<XmlSchemaElementWrapper>();

            Searcher = new XmlWrappersSearcher();

            foreach (var element in XsdReader.Schema.Elements.Values)
            {
                var wrappedElement = new XmlSchemaElementWrapper((XmlSchemaElement)element, null);
                Elements.Add(wrappedElement);
                wrappedElement.DrillOnce();
            }
        }

        public bool ValidateSchema(string schemaPath, bool throwException = false)
        {
            return XsdReader.ValidateSchema(schemaPath, throwException);
        }

        public List<XmlSchemaWrapper> SearchXml(XmlSchemaWrapper startingNode, string query, SearchEnum searchBy)
        {
            List<XmlSchemaWrapper> result = new List<XmlSchemaWrapper>();

            switch (searchBy)
            {
                case SearchEnum.Attribute:
                    result.AddRange(Searcher.SearchXmlByAttribute(startingNode, query));
                    break;
                case SearchEnum.NodeName:
                    result.AddRange(Searcher.SearchXmlByNodeName(startingNode, query));
                    break;
                case SearchEnum.All:
                    result.AddRange(Searcher.SearchXmlByAttribute(startingNode, query));
                    result.AddRange(Searcher.SearchXmlByNodeName(startingNode, query));
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Exports the current situation of the tree to XML
        /// </summary>
        /// <param name="xmlPath">Destination path for export</param>
        /// <returns>True if succeeded to export, otherwise - probably exception will occur</returns>
        public bool ExportXmlNow(string xmlPath)
        {
            // TODO : add validation in advance if we're in valid situation (all drilled and all attributes valid)
            // TODO : commented out for debug purposes, remember to return
            //if (!RootElement.AllChildAttributesFilled)
                //throw new Exception("Could not export XML because not all required attributes filled yet");

            // Create the Xml object from wrapper
            var doc = XmlExportLogic.SchemaWrapperToXmlDocument(RootElement);

            // Export the XML to the desired path
            return XmlWriterWrapper.WriteXml(doc, xmlPath);
        }
    }
}
