using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private IXmlSchemaTypeWrapper Type { get; set; }

        public decimal MinOccurs { get; set; }

        public string MaxOccursString { get; set; }

        public XmlSchemaGroupBaseWrapper Group { get; set; }

        public XmlSchemaElementWrapper Parent { get; private set; }

        public ObservableCollection<XmlSchemaElementWrapper> Children { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public XmlSchemaElementWrapper(XmlSchemaElement element, XmlSchemaElementWrapper parent) : 
            base(element.Name, NodeType.Element)
        {
            ElementObject = element;
            MinOccurs = element.MinOccurs;
            MaxOccursString = element.MaxOccursString;
            Parent = parent;
            int index = 0;            
            HandleGroups(ref index);
            Type = XmlSchemaSimpleTypeWrapper.SchemaWrappersFactory(ElementObject.ElementSchemaType);
        }

        public void PrintElement(string offset="", int? index=null)
        {
            Console.WriteLine("{0}|{1}|==>Element: {2}", offset, index.HasValue ? index.Value.ToString() : "", this.Name);
            Console.WriteLine("{0}Min/Max Occurs: {1}/{2}\n", offset, this.MinOccurs, this.MaxOccursString);
            this.Type.PrintAttrs(offset);
        }

        public ObservableCollection<XmlSchemaElementWrapper> HandleGroups(ref int index)
        {
            var result = new ObservableCollection<XmlSchemaElementWrapper>();
            if (this.Group != null)
            {
                var group = this.Group.IterateGroups("----", ref index);
               foreach (var i in group)
                    result.Add(i);
            }
            Children = result;
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
