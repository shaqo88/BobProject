using BL.SchemaLogic.SchemaTypes;
using BL.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL.XmlLogic
{
    /// <summary>
    /// Represents XMl's metadata like its user name, date and path
    /// </summary>
    public class XmlMetaData : PropertyNotifyObject
    {
        #region Properties

        private string m_xmlPath;

        public string XmlPath
        {
            get { return m_xmlPath; }
            set { SetProperty(ref m_xmlPath, value); }
        }

        private Version m_version;

        public Version Version
        {
            get { return m_version; }
            set { SetProperty(ref m_version, value); }
        }

        private DateTime m_date;

        public DateTime Date
        {
            get { return m_date; }
            set { SetProperty(ref m_date, value); }
        }

        private string m_userName;

        public string UserName
        {
            get { return m_userName; }
            set { SetProperty(ref m_userName, value); }
        }

        #endregion

        #region Constructor

        public XmlMetaData(string xmlPath, Version version, DateTime date, string userName)
        {
            XmlPath = xmlPath;
            Version = version;
            Date = date;
            UserName = userName;
        }

        #endregion
    }
    public static class XmlImportLogic
    {
        //public static XmlSchemaElementWrapper XmlDocumentToSchemaWrapper(XmlDocument xmlDoc, XmlSchemaElementWrapper rootSchema)
        //{
        //    if (rootSchema.Name == xmlDoc.DocumentElement.Name)
        //    {
        //        foreach (XmlAttribute attr in xmlDoc.DocumentElement.Attributes)
        //        {
        //            var attrToFill = rootSchema.Attributes.First(a => a.Name == attr.Name);

        //            if (attrToFill != null)
        //                attrToFill.Value = attr.Value;
        //        }
        //    }
        //    //XmlSchemaElementWrapper root = new XmlSchemaElementWrapper(xmlDoc.DocumentElement, null);
        //    foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
        //    {

        //    }

        //    return null;
        //}

        //private static 

        #region Getting Properties from XML

        /// <summary>
        /// Gets all metadate from given XML
        /// </summary>
        /// <param name="xmlDoc">XmlDocument object to use with search</param>
        /// <param name="path">Path if the XML</param>
        /// <returns>Full metadata object</returns>
        public static XmlMetaData GetAllProperties(XmlDocument xmlDoc, string path = "")
        {
            return new XmlMetaData(path, GetVersionOfXml(xmlDoc), GetDateTime(xmlDoc), GetUserName(xmlDoc));
        }

        /// <summary>
        /// Gets the version of the XML using the comment in the XML
        /// </summary>
        /// <param name="xmlDoc">XmlDocument object to use with search</param>
        /// <returns>Version of the XML</returns>
        public static Version GetVersionOfXml(XmlDocument xmlDoc)
        {
            Version ver;

            var versionComment = GetPropertyFromComment(XmlExportLogic.VERSION_FORMAT, xmlDoc);

            if (versionComment != null && Version.TryParse(versionComment, out ver))
                return ver;
            else
                // Version is 0.0 by default
                return new Version(0, 0);
        }

        /// <summary>
        /// Gets user name from the comment in the XML
        /// </summary>
        /// <param name="xmlDoc">XmlDocument object to use with search</param>
        /// <returns>User name of the editor of the XML</returns>
        public static string GetUserName(XmlDocument xmlDoc)
        {
            return GetPropertyFromComment(XmlExportLogic.USER_NAME_FORMAT, xmlDoc);
        }

        /// <summary>
        /// Gets date of when the XML was edited
        /// </summary>
        /// <param name="xmlDoc">XmlDocument object to use with search</param>
        /// <returns>Date when the XML was edited</returns>
        public static DateTime GetDateTime(XmlDocument xmlDoc)
        {
            DateTime result;

            var dateTimeString = GetPropertyFromComment(XmlExportLogic.DATE_FORMAT, xmlDoc);

            if ((dateTimeString != null) && DateTime.TryParse(dateTimeString, out result))
                return result;
            else
                return new DateTime();
        }

        /// <summary>
        /// General helper method that returns the data from the specified comment
        /// </summary>
        /// <param name="format">Format of the comment's text</param>
        /// <param name="xmlDoc">XmlDocument object to use with search</param>
        /// <returns>The desired property's string</returns>
        private static string GetPropertyFromComment(string format, XmlDocument xmlDoc)
        {
            XmlComment resultComment = null;
            string result = null;

            // Replace the fromat with empty string to get the data of the comment
            string formatStart = format.Replace("{0}", "");

            var comments = xmlDoc.ChildNodes.OfType<XmlComment>();

            if (comments != null)
            {
                resultComment = comments.FirstOrDefault(c => c.Data.Contains(formatStart));

                if (resultComment != null)
                    result = resultComment.Data.Replace(formatStart, "");
            }

            return result;
        }

        #endregion
    }
}
