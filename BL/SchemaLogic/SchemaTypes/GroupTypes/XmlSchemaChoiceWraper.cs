using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{

    public class XmlSchemaChoiceWrapper : XmlSchemaGroupBaseWrapper
    {
        private XmlSchemaChoice m_choice;

        public XmlSchemaWrapper Selected { get; set; }

        protected override void InternalDrill()
        {
            // Check if this choice is optional, if so - add NULL by default
            if (m_choice.MinOccurs == 0)
                Children.Add(new XmlSchemaNullChoice(this));

            base.InternalDrill();
        }

        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice, XmlSchemaWrapper parent)
            : base(choice, NodeType.Choice, parent)
        {
            m_choice = choice;
            OnGroupDrill += XmlSchemaChoiceWrapper_OnGroupDrill;
        }

        private void XmlSchemaChoiceWrapper_OnGroupDrill()
        {
            if (Children.Count != 0)
                Selected = Children[0];
        }

        public override string ToString()
        {
            return base.ToString() + "->" + Selected;
        }
    }

    public class XmlSchemaNullChoice : XmlSchemaWrapper
    {
        public XmlSchemaNullChoice(XmlSchemaChoiceWrapper parent)
            : base("NULL", NodeType.NULL, parent, true)
        {
        }

        public override bool IsDrillable
        {
            get { return false; }
        }

        protected override void InternalDrill()
        {
        }
    }
}
