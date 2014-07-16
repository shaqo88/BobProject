using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.ValueConverters
{
    public class SelectedItemToArrayConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            XmlSchemaWrapper schemaWr = (XmlSchemaWrapper)value;

            //check if convertion is ok
            if (schemaWr == null)
                return null;

            //create list and insert selected item
            ObservableCollection<XmlSchemaWrapper> selectedArr = new ObservableCollection<XmlSchemaWrapper>();
            selectedArr.Add(schemaWr);

            //return list
            return selectedArr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
