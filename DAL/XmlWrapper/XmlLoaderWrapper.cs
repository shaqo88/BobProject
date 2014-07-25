using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DAL.XmlWrapper
{
    public static class XmlLoaderWrapper
    {
        /// <summary>
        /// Loads XML from given path to XmlDocument object
        /// </summary>
        /// <param name="xmlPath">Path of the XML to load</param>
        /// <returns>XmlDocument object that represents the XML</returns>
        public static XmlDocument LoadXml(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();

            // Try to load XML and throw exception if fails
            try
            {
                doc.Load(xmlPath);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failed to load XML from path: {0}", xmlPath), ex);
            }

            return doc;
        }
    }
}
