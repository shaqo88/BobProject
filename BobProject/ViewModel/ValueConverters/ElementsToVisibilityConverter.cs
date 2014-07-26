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
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {

            XmlSchemaElementWrapper element = value as XmlSchemaElementWrapper;
            if (element == null)
                return Visibility.Collapsed;

            string parm = (string)parameter;
            if (element.Attributes.Count > 0 && parm.Equals("Attributes"))
                return Visibility.Visible;
            else if (element.Attributes.Count == 0 && parm.Equals("Attributes"))
                return Visibility.Collapsed;

            if (element.IsSimple && parm.Equals("Inner"))
                return Visibility.Visible;

            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
