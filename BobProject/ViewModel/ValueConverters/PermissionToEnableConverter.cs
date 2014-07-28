using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BobProject.ViewModel.ValueConverters
{
    public class PermissionToEnableConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            //convert parameters
            string currPermit = (string)value;
            string parm = (string)parameter;

            ////check if parameter equals to permission.
            if ((currPermit == "Manager") && (parm == "Manager"))
                return true;
            else if (((currPermit == "Manager") || (currPermit == "Editor")) && (parm == "Editor"))
                return true;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
