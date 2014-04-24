using System; // First checkin
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using BL.SchemaLogic;
using DAL.XmlWrapper;

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

            var currElement = describer.Elements[0];

            do
            {
                currElement.PrintElement();

                var elements = currElement.HandleGroups(ref index);

                PrintInstructions();

                if (!int.TryParse(Console.ReadLine(), out operation) || operation < -2 || operation >= elements.Count)
                {
                    Console.WriteLine("Invalid choice!");
                }
                else
                {
                    if (operation == -2)
                        currElement = currElement.Parent;
                    else if (operation == -1)
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
    }
}