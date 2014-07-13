﻿using System;
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
        protected event Action OnGroupDrill;
        private XmlSchemaGroupBase Group { get; set; }
        public override bool IsDrillable { get { return true; } }

        public XmlSchemaGroupBaseWrapper(XmlSchemaGroupBase group, NodeType nodeType, XmlSchemaWrapper parent)
            : base(nodeType.ToString(), nodeType, parent)
        {
            Group = group;
        }

        public static XmlSchemaGroupBaseWrapper SchemaGroupWrappersFactory(XmlSchemaGroupBase group, XmlSchemaWrapper parent)
        {
            if (group is XmlSchemaSequence)
                return new XmlSchemaSequenceArray(group as XmlSchemaSequence, parent);
            else if (group is XmlSchemaChoice)
                return new XmlSchemaChoiceWrapper(group as XmlSchemaChoice, parent);

            throw new Exception(string.Format("Group factory failed! Group could not be created, unknown type: {0}", group.GetType()));
        }

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
                    if (item is XmlSchemaChoice)
                    {
                        Children.Add(new XmlSchemaChoiceWrapper(item as XmlSchemaChoice, this));
                    }
                    else if (item is XmlSchemaSequence)
                    {
                        Children.Add(new XmlSchemaSequenceArray(item as XmlSchemaSequence, this));
                    }
                    ((XmlSchemaGroupBaseWrapper)Children.Last()).DrillOnce();
                }
            }

            if (OnGroupDrill != null)
                OnGroupDrill();
        }
    }


}