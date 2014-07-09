using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaSequenceWrapper : XmlSchemaGroupBaseWrapper
    {
        public int Index { get; private set; }
        
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence, XmlSchemaSequenceArray parent, int index)
            : base(sequence, NodeType.SequenceItem, parent)
        {
            Index = index;
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}
