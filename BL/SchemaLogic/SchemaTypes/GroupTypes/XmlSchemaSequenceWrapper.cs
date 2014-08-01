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
        #region Properties

        private int m_index;

        public int Index {
            get 
            {
                return m_index;
            }
            set 
            {
                SetProperty(ref m_index, value);
            }
        }

        #endregion

        #region Constructor

        public XmlSchemaSequenceWrapper(XmlSchemaSequence sequence, XmlSchemaSequenceArray parent, int index)
            : base(sequence, NodeType.SequenceItem, parent)
        {
            Index = index;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Index.ToString();
        }

        #endregion
    }
}
