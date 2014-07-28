using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BobProject.ViewModel.ValueConverters
{
    public class PermissionToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            //convert parameters
            string Permit = (string)value;
            string parm = (string)parameter;

            //check if parameter equals to permission.
            if ((Permit == "Manager") && (parm == "Manager"))
                return Visibility.Visible;
            if (((Permit == "Manager") || (Permit == "Editor")) && (parm == "Editor"))
                return Visibility.Visible;

            //not match - collapse
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
