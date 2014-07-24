using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{

    public class XmlSchemaChoiceWrapper : XmlSchemaGroupBaseWrapper
    {
        private XmlSchemaChoice m_choice;

        private XmlSchemaWrapper m_selected;

        public XmlSchemaWrapper Selected
        {
            get
            {
                return m_selected;
            }
            set
            {
                // Unregister from old Selected's event
                if (m_selected != null)
                    m_selected.PropertyChanged -= Selected_PropertyChanged;

                SetProperty(ref m_selected, value);

                // Register to new Selected's event
                if (m_selected != null)
                    m_selected.PropertyChanged += Selected_PropertyChanged;

                RaiseAllProperties();
            }
        }

        void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllChildAttributesFilled" || e.PropertyName == "AllChildrenDrilled")
            {
                RaisePropertyChangedEvent(e.PropertyName);
            }
        }

        public override bool AllChildAttributesFilled
        {
            get
            {
                return Selected.AllChildAttributesFilled;
            }
        }

        public override bool AllChildrenDrilled
        {
            get
            {
                return Selected.AllChildrenDrilled;
            }
        }

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
