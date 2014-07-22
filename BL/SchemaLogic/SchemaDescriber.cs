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
using System.Xml;

namespace BL.SchemaLogic
{
    public class SchemaDescriber
    {
        #region Public Properties

        public ObservableCollection<XmlSchemaElementWrapper> Elements { get; private set; }

        public Version XmlVersion { get; private set; }

        public string UserName { get; private set; }

        public DateTime LastEditDate { get; private set; }

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

        #endregion

        #region Private Properties

        private XsdReader XsdReader { get; set; }

        private XmlWrappersSearcher Searcher { get; set; }

        #endregion

        #region Constructor

        public SchemaDescriber(string schemaPath, string userName = null)
        {
            XmlVersion = new Version(1, 0);
            UserName = userName;
            LoadSchema(schemaPath);
        }

        #endregion

        #region Public Methods

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

        public void ClearXml()
        {
            foreach (var element in Elements)
            {
                ClearWrapper(element);
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

        public bool LoadExistingXml(string xmlPath)
        {
            var doc = XmlLoaderWrapper.LoadXml(xmlPath);
            string errorMessage;

            if (!XsdReader.IsXmlMatchSchema(doc, out errorMessage))
                throw new Exception(string.Format("Given XML doesn't match the loaded schema (XSD). Path: {0}, Details: {1}", xmlPath, errorMessage));

            this.ClearXml();

            XmlVersion = XmlImportLogic.GetVersionOfXml(doc);
            UserName = XmlImportLogic.GetUserName(doc);
            LastEditDate = XmlImportLogic.GetDateTime(doc);

            Elements.Add(XmlImportLogic.XmlDocumentToSchemaWrapper(doc));

            return true;
        }

        /// <summary>
        /// Exports the current situation of the tree to XML
        /// </summary>
        /// <param name="xmlPath">Destination path for export</param>
        /// <returns>True if succeeded to export, otherwise - probably exception will occur</returns>
        public bool ExportXmlNow(string xmlPath, Version version = null, string userName = null)
        {
            // TODO : check also if all children drilled and filled correctly
            // Check if all attributes filled correcty before trying to export
            if (!RootElement.AllChildAttributesFilled)
                throw new Exception("Could not export XML because not all required attributes filled yet");

            // Create the Xml object from wrapper
            var doc = XmlExportLogic.SchemaWrapperToXmlDocument(RootElement,
                                                                version != null ? version : XmlVersion,
                                                                string.IsNullOrEmpty(userName) ? UserName : userName,
                                                                true);

            // Export the XML to the desired path
            return XmlWriterWrapper.WriteXml(doc, xmlPath);
        }

        /// <summary>
        /// Gets the current XmlDocument from the data in the system
        /// </summary>
        /// <returns>The XmlDocument object that represents the current XML</returns>
        public XmlDocument GetCurrentXmlDocument()
        {
            return XmlExportLogic.SchemaWrapperToXmlDocument(RootElement, XmlVersion, UserName, false);
        }

        #endregion

        #region Private Methods

        private void ClearWrapper(XmlSchemaWrapper wrapper)
        {
            if (wrapper is XmlSchemaElementWrapper)
            {
                var element = wrapper as XmlSchemaElementWrapper;

                foreach (var attr in element.Attributes)
                {
                    attr.Value = null;
                }
            }
            else if (wrapper is XmlSchemaChoiceWrapper)
            {
                var choice = wrapper as XmlSchemaChoiceWrapper;

                if (choice.Children != null && choice.Children.Count > 0)
                    choice.Selected = choice.Children[0];
            }
            else if (wrapper is XmlSchemaSequenceArray)
            {
                var seqArr = wrapper as XmlSchemaSequenceArray;
                seqArr.Clear();
            }

            foreach (var child in wrapper.Children)
            {
                ClearWrapper(child);
            }
        }

        #endregion
    }
}
