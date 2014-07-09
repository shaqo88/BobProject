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
        private int m_index;
        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence, XmlSchemaSequenceArray parent, int index)
            : base(sequence, NodeType.SequenceItem, parent)
        {
            m_index = index;
        }

        public override string ToString()
        {
            return m_index.ToString();
        }
    }
}
