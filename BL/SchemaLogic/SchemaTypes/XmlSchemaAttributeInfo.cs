using BL.UtilityClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace BL.SchemaLogic.SchemaTypes
{
    public class XmlSchemaAttributeInfo : PropertyNotifyObject, INotifyHighLevelPropertyChanged
    {
        public event PropertyChangedEventHandler HighLevelPropertyChanged;

        private string m_name;

        public string Name
        {
            get { return m_name; }
            set { SetProperty(ref m_name, value); }
        }

        private XmlSchemaUse m_use;

        public XmlSchemaUse Use
        {
            get { return m_use; }
            set { SetProperty(ref m_use, value); }
        }

        public bool IsRequired { get { return Use == XmlSchemaUse.Required; } }

        public bool IsAttributeValid { get { return !IsRequired || IsAttributeFilled; } }

        public bool IsAttributeFilled { get { return Value != null && Value != string.Empty; } }

        public Type SimpleType { get; set; }

        private string m_value;

        public string Value
        {
            get { return m_value; }
            set { SetProperty(ref m_value, value); }
        }

        public XmlSchemaAttributeInfo()
        {
            PropertyChanged += XmlSchemaAttributeInfo_PropertyChanged;
        }

        void XmlSchemaAttributeInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // IsAttributeValid raised if IsRequired or is IsAttributeFilled raised, i.e. Use or Value
            if (e.PropertyName != "Name")
                RaiseHighLevelPropertyChanged("IsAttributeValid");

            if (e.PropertyName == "Use")
                // IsRequired depends on Use
                RaiseHighLevelPropertyChanged("IsRequired");
            else if (e.PropertyName == "Value")
                // IsAttributeFilled depends on Value
                RaiseHighLevelPropertyChanged("IsAttributeFilled");
        }

        protected void RaiseHighLevelPropertyChanged(string propertyName)
        {
            if (HighLevelPropertyChanged != null)
                HighLevelPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
