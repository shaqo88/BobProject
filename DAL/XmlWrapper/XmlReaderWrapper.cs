using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DAL.XmlWrapper
{
    public static class XmlReaderWrapper
    {
        /// <summary>
        /// Reads an xml file and produces an object to work with it
        /// </summary>
        /// <param name="xmlPath">The path of the Xml to read</param>
        /// <returns>XmlDocument object for easy access</returns>
        public static XmlDocument ReadXml(string xmlPath)
        {
            var document = new XmlDocument();
            using (var stream = new System.IO.StreamReader(xmlPath))
            {
                document.Load(stream);
            }

            return document;
        }
    }
}
