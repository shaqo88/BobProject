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
        #region Members

        private XmlSchemaChoice m_choice;

        private XmlSchemaWrapper m_selected;


        #endregion

        #region Properties

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

        #endregion

        #region Constructor

        public XmlSchemaChoiceWrapper(XmlSchemaChoice choice, XmlSchemaWrapper parent)
            : base(choice, NodeType.Choice, parent)
        {
            m_choice = choice;
            OnGroupDrill += XmlSchemaChoiceWrapper_OnGroupDrill;
        }

        #endregion

        #region Methods

        protected override void InternalDrill()
        {
            // Check if this choice is optional, if so - add NULL by default
            if (m_choice.MinOccurs == 0)
                Children.Add(new XmlSchemaNullChoice(this));

            base.InternalDrill();
        }

        public override string ToString()
        {
            return base.ToString() + "->" + Selected;
        }

        #endregion

        #region Handlers

        void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllChildAttributesFilled" || e.PropertyName == "AllChildrenDrilled")
            {
                RaisePropertyChangedEvent(e.PropertyName);
            }
        }

        private void XmlSchemaChoiceWrapper_OnGroupDrill()
        {
            if (Children.Count != 0)
                Selected = Children[0];
        }

        #endregion
    }

    public class XmlSchemaNullChoice : XmlSchemaWrapper
    {
        #region Constructor

        public XmlSchemaNullChoice(XmlSchemaChoiceWrapper parent)
            : base("NULL", NodeType.NULL, parent, true)
        {
        }

        #endregion

        #region Properties

        public override bool IsDrillable
        {
            get { return false; }
        }

        #endregion

        #region Methods

        protected override void InternalDrill()
        {
        }

        #endregion
    }
}
