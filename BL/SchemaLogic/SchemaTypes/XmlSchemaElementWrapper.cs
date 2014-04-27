using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using BL.SchemaLogic.SchemaTypes.XmlSchemaTypeComposite;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaElementWrapper : XmlSchemaWrapper
    {
        private XmlSchemaElement ElementObject { get; set; }

        public string Name { get; private set; }

        public IXmlSchemaTypeWrapper Type { get; private set; }

        public string MinOccursString { get; set; }

        public string MaxOccursString { get; set; }

        public XmlSchemaGroupBaseWrapper Group { get; set; }

        public XmlSchemaElementWrapper Parent { get; private set; }

        public XmlSchemaElementWrapper(XmlSchemaElement element, XmlSchemaElementWrapper parent)
        {
            ElementObject = element;
            Name = element.Name;
            MinOccursString = element.MinOccursString;
            MaxOccursString = element.MaxOccursString;
            Parent = parent;

            Type = XmlSchemaSimpleTypeWrapper.SchemaWrappersFactory(ElementObject.ElementSchemaType);
        }

        public void PrintElement(string offset="", int? index=null)
        {
            Console.WriteLine("{0}|{1}|==>Element: {2}", offset, index.HasValue ? index.Value.ToString() : "", this.Name);
            Console.WriteLine("{0}Min/Max Occurs: {1}/{2}\n", offset, this.MinOccursString, this.MaxOccursString);
            this.Type.PrintAttrs(offset);
        }

        public List<XmlSchemaElementWrapper> HandleGroups(ref int index)
        {
            var result = new List<XmlSchemaElementWrapper>();
            if (this.Group != null)
                result.AddRange(this.Group.IterateGroups("----", ref index));

            return result;
        }

        public void DrillOnce()
        {
            Group = Type.DrillOnce(this);
            //if (this.Type.SchemaType is XmlSchemaComplexType)
            //{
            //    var compType = this.Type.SchemaType as XmlSchemaComplexType;

            //    if (compType.ContentTypeParticle != null)
            //    {
            //        if (compType.ContentTypeParticle is XmlSchemaSequence)
            //        {
            //            var seq = compType.ContentTypeParticle as XmlSchemaSequence;
            //            this.Group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(seq);
            //            this.Group.DrillOnce(this);
            //        }
            //        else if (compType.ContentTypeParticle is XmlSchemaChoice)
            //        {
            //            var choice = compType.ContentTypeParticle as XmlSchemaChoice;
            //            this.Group = XmlSchemaGroupBaseWrapper.SchemaGroupWrappersFactory(choice);
            //            this.Group.DrillOnce(this);
            //        }
            //    }
            //}
        }
    }
}
