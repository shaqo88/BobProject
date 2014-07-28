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
using BL.UtilityClasses;
using System.ComponentModel;
using System.IO;

namespace BL.SchemaLogic
{
    public class SchemaDescriber : PropertyNotifyObject
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

        private XmlDocument m_currentXmlDocument;

        public XmlDocument CurrentXmlDocument
        {
            get
            {
                return m_currentXmlDocument;
            }
            set
            {
                SetProperty(ref m_currentXmlDocument, value);
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

        void RootElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.CurrentXmlDocument = GetCurrentXmlDocument();
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

            RootElement.PropertyChanged += RootElement_PropertyChanged;
        }

        /// <summary>
        /// Clears all main XML object's elements
        /// </summary>
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

        /// <summary>
        /// Searches the XML with given query
        /// </summary>
        /// <param name="startingNode">Node to start the recursive search with</param>
        /// <param name="query">The query sting to check with</param>
        /// <param name="searchBy">Method of the search - attribute, node name or both</param>
        /// <returns>List of wrappers that match the query</returns>
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

        //public bool LoadExistingXml(string xmlPath)
        //{
        //    var doc = XmlLoaderWrapper.LoadXml(xmlPath, XsdReader.Schema);
        //    //string errorMessage;

        //    //if (!XsdReader.IsXmlMatchSchema(doc, out errorMessage))
        //    //    throw new Exception(string.Format("Given XML doesn't match the loaded schema (XSD). Path: {0}, Details: {1}", xmlPath, errorMessage));

        //    this.ClearXml();

        //    XmlVersion = XmlImportLogic.GetVersionOfXml(doc);
        //    UserName = XmlImportLogic.GetUserName(doc);
        //    LastEditDate = XmlImportLogic.GetDateTime(doc);

        //    //Elements.Add(XmlImportLogic.XmlDocumentToSchemaWrapper(doc));

        //    return true;
        //}

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

        /// <summary>
        /// Prouces list of matching files according to input for reports
        /// </summary>
        /// <param name="folderPath">Folder to look for the files</param>
        /// <param name="userName">User name to find, if null or empty - any user</param>
        /// <param name="dates">Date range to serach for</param>
        /// <returns>List of matching files with their details</returns>
        public ObservableCollection<XmlMetaData> ProduceReport(string folderPath, string userName, DateRange dates)
        {
            ObservableCollection<XmlMetaData> result = new ObservableCollection<XmlMetaData>();

            // Check if folder is a real path
            if (!Directory.Exists(folderPath))
                throw new Exception(string.Format("Could not generate report, folder doesn't exist: {0}", folderPath));

            // Get all XMLs at this path
            var filePaths = Directory.GetFiles(folderPath, "*.xml");

            Func<string, bool> userNameCompareFunc;

            // An empty or null user name means any user will match, otherwise - checking if it's substring of the input
            if (string.IsNullOrEmpty(userName))
                userNameCompareFunc = (u) => { return true; };
            else
                userNameCompareFunc = (u) => { return u.Contains(userName); };

            foreach (var filePath in filePaths)
            {
                try
                {
                    var xmlFile = XmlLoaderWrapper.LoadXml(filePath);

                    var metaData = XmlImportLogic.GetAllProperties(xmlFile);
                    metaData.XmlPath = filePath;

                    // Check dates and compare user name
                    if (dates == null && userNameCompareFunc(metaData.UserName))
                        result.Add(metaData);
                    else if (dates.IsInRange(metaData.Date) && userNameCompareFunc(metaData.UserName))
                        result.Add(metaData);
                }
                catch
                {
                    // File is illegal XML, continue, no need to throw exeption
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Clears the given wrapper object from all data
        /// </summary>
        /// <param name="wrapper">The wrapper to clear</param>
        private void ClearWrapper(XmlSchemaWrapper wrapper)
        {
            // For element - clear all attributes
            if (wrapper is XmlSchemaElementWrapper)
            {
                var element = wrapper as XmlSchemaElementWrapper;

                foreach (var attr in element.Attributes)
                {
                    attr.Value = null;
                }
            }
            // For choice - clear the selected
            else if (wrapper is XmlSchemaChoiceWrapper)
            {
                var choice = wrapper as XmlSchemaChoiceWrapper;

                if (choice.Children != null && choice.Children.Count > 0)
                    choice.Selected = choice.Children[0];
            }
            // For sequence array - clear its children
            else if (wrapper is XmlSchemaSequenceArray)
            {
                var seqArr = wrapper as XmlSchemaSequenceArray;
                seqArr.Clear();
            }

            // For all objects - clear their children
            foreach (var child in wrapper.Children)
            {
                ClearWrapper(child);
            }
        }

        #endregion
    }
}
