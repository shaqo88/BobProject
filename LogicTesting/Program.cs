using System; // Third checkin Eyal branch
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

namespace BL
{
    class LogicTesting
    {
        static void Main()
        {
            var describer = new SchemaDescriber("../../copperhead.xsd");
            var doc = DAL.XmlWrapper.XmlReaderWrapper.ReadXml("../../ch.xml");

            var elementsQuery = XmlSearcher.SearchXml(doc, new XmlQueryPartType() { QueriedNode = "type", ReturnedNode = "typedef", AttributeName = "ref", AttributeValue = "FullName_t" });

            var xml = new XmlDocument();
            var element = xml.CreateElement("newElement");
            var attr = xml.CreateAttribute("newAttr");
            attr.Value = "valueA";
            element.Attributes.Append(attr);
            element.InnerText = "Inner";
            xml.AppendChild(element);

            DAL.XmlWrapper.XmlWriterWrapper.WriteXml(xml, "../../ch_out.xml");

            int operation = 1;

            XmlSchemaWrapper currElement = describer.Elements[0];

            do
            {
                PrintElement(currElement as XmlSchemaElementWrapper);

                var elements = HandleGroups(currElement as XmlSchemaElementWrapper, ref index);

                PrintInstructions();

                if (!int.TryParse(Console.ReadLine(), out operation) || operation < -2 || operation >= elements.Count)
                {
                    Console.WriteLine("Invalid choice!");
                }
                else
                {
                    if (operation == -2) // Go up one layer
                        currElement = currElement.Parent;
                    else if (operation == -1) // Exit
                        return;
                    else
                    {
                        currElement = elements[operation];
                        currElement.DrillOnce();
                    }
                }
                Console.WriteLine("****************************");

                index = 0;
                elements.Clear();

            } while (operation != -1);
        }

        static int index = 0;

        private static void PrintInstructions()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Choose element to expand");
            Console.WriteLine("or -2 parent");
            Console.WriteLine("or -1 to exit");
            Console.WriteLine("==================================");
        }

        private static void PrintAttrs(XmlSchemaElementWrapper element, string offset)
        {
            offset += "++";
            Console.WriteLine("{0}Attributes", offset);
            Console.WriteLine("{0}==========", offset);

            foreach (var attr in element.Attributes)
            {
                Console.WriteLine("{0}{1}, \t Type: {2}", offset, attr.Name, attr.SimpleType);
            }
            Console.WriteLine("{0}==========", offset);

            if (element.DotNetType != null)
            {
                offset += "++";

                Console.WriteLine("{0}Simple Type: {1}", offset, element.DotNetType);
                Console.WriteLine("{0}==========", offset);
            }
        }

        private static void PrintElement(XmlSchemaElementWrapper element, string offset = "", int? index = null)
        {
            Console.WriteLine("{0}|{1}|==>Element: {2}", offset, index.HasValue ? index.Value.ToString() : "", element.Name);
            Console.WriteLine("{0}Min/Max Occurs: {1}/{2}\n", offset, element.MinOccurs, element.MaxOccursString);
            PrintAttrs(element, offset);
        }

        private static ObservableCollection<XmlSchemaElementWrapper> IterateGroups(XmlSchemaGroupBaseWrapper group, string offset, ref int index)
        {
            ObservableCollection<XmlSchemaElementWrapper> elements = new ObservableCollection<XmlSchemaElementWrapper>();

            foreach (var innerItem in group.Children)
            {
                Console.WriteLine("{0}Element group type: {1}", offset, group.GetType());

                if (innerItem is XmlSchemaGroupBaseWrapper)
                {
                    offset += "----";
                    var innerItems = IterateGroups(innerItem as XmlSchemaGroupBaseWrapper, offset, ref index);
                    foreach (var i in innerItems)
                        elements.Add(i);
                }
                else
                {
                    if (innerItem is XmlSchemaElementWrapper)
                    {
                        var element = innerItem as XmlSchemaElementWrapper;
                        elements.Add(element);
                        PrintElement(element, offset, index);
                        ++index;
                    }
                }
            }

            return elements;
        }

        private static ObservableCollection<XmlSchemaElementWrapper> HandleGroups(XmlSchemaElementWrapper element, ref int index)
        {
            var result = new ObservableCollection<XmlSchemaElementWrapper>();
            if (element.Children.Count > 0)
            {
                var groupList = IterateGroups(element.Children[0] as XmlSchemaGroupBaseWrapper, "----", ref index);
                foreach (var i in groupList)
                    result.Add(i);
            }

            return result;
        }
    }
}