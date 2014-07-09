﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using DAL.SchemaDataAccess;
using BL.SchemaLogic.SchemaTypes;
using System.Collections.ObjectModel;
using BL.XmlLogic;

namespace BL.SchemaLogic
{
    public class SchemaDescriber
    {
        private XsdReader XsdReader { get; set; }

        private XmlWrappersSearcher Searcher { get; set; }

        public ObservableCollection<XmlSchemaElementWrapper> Elements { get; set; }

        public SchemaDescriber(string schemaPath)
        {
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

        public bool ValidateSchema(string schemaPath)
        {
            return XsdReader.ValidateSchema(schemaPath);
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
    }
}
