using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DAL.XmlWrapper
{
    /// <summary>
    /// Extension class to manipulate XElement and XmlNode
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Converts XmlNode to XElement
        /// </summary>
        /// <param name="element">the XElement to convert</param>
        /// <returns>Converted XmlNode object</returns>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        /// <summary>
        /// Converts a list of XElement to a list of XmlNode
        /// </summary>
        /// <param name="elements">The list of XElement</param>
        /// <returns>A list of XmlNode</returns>
        public static List<XmlNode> ConvertToXmlNodes(this IEnumerable<XElement> elements)
        {
            List<XmlNode> result = new List<XmlNode>();
            foreach (var node in elements)
            {
                result.Add(node.GetXmlNode());
            }

            return result;
        }
    }
}
