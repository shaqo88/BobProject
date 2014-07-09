using BL.SchemaLogic.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.XmlLogic
{
    public enum SearchEnum { Attribute, NodeName, All }

    /// <summary>
    /// Class that is responsible for searching inside the Objects of Xml schema
    /// </summary>
    public class XmlWrappersSearcher
    {
        public Func<string, string, bool> StringsCompareFunc { get; private set; }

        /// <summary>
        /// Initializes the searcher class
        /// </summary>
        /// <param name="compareFunc">Function that will be used to compare strings</param>
        public XmlWrappersSearcher(Func<string, string, bool> compareFunc = null)
        {
            // If parameter is null, use the default function
            if (compareFunc == null)
                StringsCompareFunc = DefaultStringsCompareFunc;
            else
                StringsCompareFunc = compareFunc;
        }

        /// <summary>
        /// Searches the Xml by an attribute value
        /// </summary>
        /// <param name="startingNode">The root node to search the attribute value</param>
        /// <param name="attributeValue">The value to look for</param>
        /// <returns>List of matching schema objects</returns>
        public List<XmlSchemaWrapper> SearchXmlByAttribute(XmlSchemaWrapper startingNode, string attributeValue)
        {
            List<XmlSchemaWrapper> result = new List<XmlSchemaWrapper>();

            // Only elements have attributes
            if (startingNode is XmlSchemaElementWrapper)
            {
                var element = startingNode as XmlSchemaElementWrapper;

                // Gets all attributes who's value match
                if (element.Attributes.Any((attr) => StringsCompareFunc(attr.Value, attributeValue)))
                    result.Add(element);
            }

            // If possible to drill the object - go recursively
            if (startingNode.IsDrillable)
            {
                foreach (var child in startingNode.Children)
                {
                    result.AddRange(SearchXmlByAttribute(child, attributeValue));
                }
            }

            return result;
        }

        /// <summary>
        /// Searches for a node with a given name
        /// </summary>
        /// <param name="startingNode">The root node to search the attribute value</param>
        /// <param name="nodeName">The name to look for</param>
        /// <returns>List of matching schema objects</returns>
        public List<XmlSchemaWrapper> SearchXmlByNodeName(XmlSchemaWrapper startingNode, string nodeName)
        {
            List<XmlSchemaWrapper> result = new List<XmlSchemaWrapper>();

            // Compare the current node and add it if matches
            if (StringsCompareFunc(startingNode.Name, nodeName))
                result.Add(startingNode);

            // If node is drillable - go recursively over its children
            if (startingNode.IsDrillable)
            {
                foreach (var child in startingNode.Children)
                {
                    result.AddRange(SearchXmlByNodeName(child, nodeName));
                }
            }

            return result;
        }

        /// <summary>
        /// The default string comparison just checks if the first string contains the second one
        /// </summary>
        /// <param name="first">The first string, suppose to be the container</param>
        /// <param name="second">The second string, the value to look for</param>
        /// <returns>True if first contains second, false otherwise</returns>
        protected bool DefaultStringsCompareFunc(string first, string second)
        {
            return first.Contains(second);
        }
    }
}
