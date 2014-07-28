using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.ValueConverters
{
    public class ElementsToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            //convert parametes
            XmlSchemaElementWrapper element = value as XmlSchemaElementWrapper;
            if (element == null)
                return Visibility.Collapsed;

            string parm = (string)parameter;

            //check if attribute list is not empty and check if param is to Attribute
            if (element.Attributes.Count > 0 && parm.Equals("Attributes"))
                return Visibility.Visible;
            //check if attribute list is  empty and check if param is to Attribute
            else if (element.Attributes.Count == 0 && parm.Equals("Attributes"))
                return Visibility.Collapsed;

            //check if is it simple type
            if (element.IsSimple && parm.Equals("Inner"))
                return Visibility.Visible;

            //error - collapse
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
