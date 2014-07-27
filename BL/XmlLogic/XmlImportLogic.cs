using BL.SchemaLogic.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL.XmlLogic
{
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

        public static Version GetVersionOfXml(XmlDocument xmlDoc)
        {
            Version ver;

            var versionComment = GetPropertyFromComment(XmlExportLogic.VERSION_FORMAT, xmlDoc);

            if (versionComment != null && Version.TryParse(versionComment, out ver))
                return ver;
            else
                return new Version(0, 0);
        }

        public static string GetUserName(XmlDocument xmlDoc)
        {
            return GetPropertyFromComment(XmlExportLogic.USER_NAME_FORMAT, xmlDoc);
        }

        public static DateTime GetDateTime(XmlDocument xmlDoc)
        {
            DateTime result;

            var dateTimeString = GetPropertyFromComment(XmlExportLogic.DATE_FORMAT, xmlDoc);

            if ((dateTimeString != null) && DateTime.TryParse(dateTimeString, out result))
                return result;
            else
                return new DateTime();
        }

        private static string GetPropertyFromComment(string format, XmlDocument xmlDoc)
        {
            XmlComment resultComment = null;
            string result = null;

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
