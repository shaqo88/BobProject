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
    public static class XmlSearcher
    {
        /// <summary>
        /// Queries the given Xml according to the defined query
        /// </summary>
        /// <param name="document">Xml document object to query</param>
        /// <param name="query">Query definitions</param>
        /// <returns>List of nodes that answer the query requests</returns>
        public static List<XmlNode> SearchXml(XmlDocument document, XmlQueryPartType query)
        {
            // Make sure that QueriedNode is not null
            if (query.QueriedNode == null)
                throw new Exception("Error during query: query.QueriedNode cannot be null!");

            var xDoc = XDocument.Load(new XmlNodeReader(document));

            // Take only the nodes that requested to return. If null, we use the queried node, which are lower in hierarchy
            var qNodes = xDoc.Root.Descendants(query.ReturnedNode != null ? query.ReturnedNode : query.QueriedNode);
            
            IEnumerable<XElement> filteredNodes = null;

            // If requested certain attribute, check it, if null - just return the list as it is
            if (query.AttributeName != null)
                filteredNodes = qNodes.Where(n => n.DescendantsAndSelf().Any(n1 => (string)n1.Attribute(query.AttributeName) == query.AttributeValue));
            else
                filteredNodes = qNodes;


            return filteredNodes.ConvertToXmlNodes();
        }
    }

    public class XmlQueryPartType
    {
        public string QueriedNode { get; set; }

        // if null, QueriedNode is returned. 
        // Note: if hierarchy contains multiple node with this name - top most is returned
        public string ReturnedNode { get; set; }
        public string AttributeName { get; set; } // If null, no restriction on attributes
        public string AttributeValue { get; set; }
    }
}
