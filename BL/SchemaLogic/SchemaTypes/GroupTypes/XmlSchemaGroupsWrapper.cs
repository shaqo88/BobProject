using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    /// <summary>
    /// For <choice> and <sequence> tags
    /// </summary>
    public abstract class XmlSchemaGroupBaseWrapper : XmlSchemaWrapper
    {
        #region Events

        protected event Action OnGroupDrill;

        #endregion

        #region Properties

        public override bool IsDrillable { get { return true; } }
        private XmlSchemaGroupBase Group { get; set; }

        #endregion

        #region Constructor

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group, NodeType nodeType, XmlSchemaWrapper parent)
            : base(nodeType.ToString(), nodeType, parent)
        {
            Group = group;
        }

        #endregion

        #region Methods

        public static XmlSchemaGroupBaseWrapper SchemaGroupWrappersFactory(XmlSchemaGroupBase group, XmlSchemaWrapper parent)
        {
            if (group is XmlSchemaSequence)
                return new XmlSchemaSequenceArray(group as XmlSchemaSequence, parent);
            else if (group is XmlSchemaChoice)
                return new XmlSchemaChoiceWrapper(group as XmlSchemaChoice, parent);

            throw new Exception(string.Format("Group factory failed! Group could not be created, unknown type: {0}", group.GetType()));
        }

        /// <summary>
        /// Internal implementation of the Decorator
        /// </summary>
        protected override void InternalDrill()
        {
            foreach (var item in Group.Items)
            {
                if (item is XmlSchemaElement)
                {
                    Children.Add(new XmlSchemaElementWrapper(item as XmlSchemaElement, this));
                }
                else if (item is XmlSchemaGroupBase)
                {
                    XmlSchemaGroupBaseWrapper groupItem = null;
                    if (item is XmlSchemaChoice)
                    {
                        groupItem = new XmlSchemaChoiceWrapper(item as XmlSchemaChoice, this);
                    }
                    else if (item is XmlSchemaSequence)
                    {
                        groupItem = new XmlSchemaSequenceArray(item as XmlSchemaSequence, this);
                    }

                    if (groupItem != null)
                    {
                        groupItem.DrillOnce();
                        Children.Add(groupItem);
                    }
                }
            }

            if (OnGroupDrill != null)
                OnGroupDrill();
        }

        #endregion
    }


}
