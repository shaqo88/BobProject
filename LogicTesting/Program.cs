﻿using System; // Third checkin Eyal branch
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using BL.SchemaLogic;
using DAL.XmlWrapper;
using BL.SchemaLogic.SchemaTypes;
using BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite;
using System.Collections.ObjectModel;
using BL.XmlLogic;

namespace BL
{
    class LogicTesting
    {
        static ObservableCollection<XmlSchemaWrapper> flatChildrenList = new ObservableCollection<XmlSchemaWrapper>();
        static int index = 0;
        static string offset = string.Empty;
        const string OFFSET_STR = "--";

        static void Main()
        {
            var describer = new SchemaDescriber("../../copperhead.xsd");
            var doc = DAL.XmlWrapper.XmlReaderWrapper.ReadXml("../../ch.xml");

            // Probably won't be in use
            //var elementsQuery = XmlSearcher.SearchXml(doc, new XmlQueryPartType() { QueriedNode = "type", ReturnedNode = "typedef", AttributeName = "ref", AttributeValue = "FullName_t" });


            var xml = new XmlDocument();
            var element = xml.CreateElement("newElement");
            var attr = xml.CreateAttribute("newAttr");
            attr.Value = "valueA";
            element.Attributes.Append(attr);
            element.InnerText = "Inner";
            xml.AppendChild(element);

            DAL.XmlWrapper.XmlWriterWrapper.WriteXml(xml, "../../ch_out.xml");

            int operation = 1;

            XmlSchemaWrapper currNode = describer.Elements[0];

            do
            {
                PrintNode(currNode, true);

                if (currNode is XmlSchemaElementWrapper)
                    PrintAttrs(currNode as XmlSchemaElementWrapper);

                PrintInstructions();

                if (!int.TryParse(Console.ReadLine(), out operation) || operation < -3 || operation >= flatChildrenList.Count)
                {
                    Console.WriteLine("Invalid choice!");
                }
                else
                {
                    if (operation == -3)
                    {
                        var searchResults = describer.SearchXml(currNode, "type", SearchEnum.NodeName);
                        foreach (var res in searchResults)
                        {
                            Console.WriteLine("Result: {0}, Parent: {1}", res.Name, res.Parent);
                        }
                    }
                    else if (operation == -2) // Go up one layer
                    {
                        if (currNode.Parent == null)
                            Console.WriteLine("This is the root, can't go up");
                        else
                            currNode = currNode.Parent;
                    }
                    else if (operation == -1) // Exit
                        return;
                    else
                    {
                        currNode = flatChildrenList[operation];
                        currNode.DrillOnce();
                    }
                }
                Console.WriteLine("****************************");

                flatChildrenList.Clear();
                index = 0;

            } while (operation != -1);
        }

        private static void PrintNode(XmlSchemaWrapper node, bool isRoot = false)
        {
            if (!(node is XmlSchemaElementWrapper) || isRoot)
                Console.WriteLine("{0}|X|==>{1}, IsDrill: {2}, HasBeen: {3}, All: {4}, Attrs: {5}", offset, node.ToString(), node.IsDrillable, node.HasBeenDrilled, node.AllChildrenDrilled, node.AllChildAttributesFilled);
            else
                Console.WriteLine("{0}|{1}|==>{2}, IsDrill: {3}, HasBeen: {4}, All: {5}, Attrs: {5}", offset, index, node.ToString(), node.IsDrillable, node.HasBeenDrilled, node.AllChildrenDrilled, node.AllChildAttributesFilled);

            offset += OFFSET_STR;

            if (node is XmlSchemaSequenceArray)
            {
                var seqArr = node as XmlSchemaSequenceArray;
                if (seqArr.Count == 0)
                    seqArr.AddNewWrapper();
            }

            if (isRoot || node is XmlSchemaGroupBaseWrapper)
            {
                foreach (var child in node.Children)
                {
                    PrintNode(child);
                    if (child is XmlSchemaElementWrapper)
                    {
                        flatChildrenList.Add(child);
                        ++index;
                    }
                }
            }

            offset = offset.Remove(0, OFFSET_STR.Length);
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Choose element to expand");
            Console.WriteLine("or -2 parent");
            Console.WriteLine("or -1 to exit");
            Console.WriteLine("==================================");
        }

        private static void PrintAttrs(XmlSchemaElementWrapper element)
        {
            Console.WriteLine("\t\t\t||==============================================||");
            Console.WriteLine("\t\t\t||Attributes of the element {0}\t\t||", element.Name);
            Console.WriteLine("\t\t\t||==============================================||");

            foreach (var attr in element.Attributes)
            {
                Console.WriteLine("\t\t\t||{0}, \t Type: {1}\t\t||", attr.Name, attr.SimpleType);
            }
            Console.WriteLine("\t\t\t||==========\t\t\t\t\t||", offset);

            if (element.DotNetType != null)
            {
                Console.WriteLine("\t\t\t||Simple Type: {0}||", element.DotNetType);
                Console.WriteLine("\t\t\t||==========\t\t\t\t\t||");
            }

            Console.WriteLine("\t\t\t||==============================================||");

        }
    }
}