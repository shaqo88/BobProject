﻿using System;
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
    public class SelectionPropertyToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            //convert parameters
            NodeType nodeType = (NodeType)value;
            string parm = (string)parameter;

            //check if node type equals to parameter. if so - visible. otherwise, collapse.
            if ((nodeType == NodeType.Element) && (parm == "Element"))
                return Visibility.Visible;
            else if ((nodeType == NodeType.Choice) && (parm == "Choice"))
                return Visibility.Visible;
            else if ((nodeType == NodeType.Sequence) && (parm == "Sequence"))
                return Visibility.Visible;
            else if (((nodeType == NodeType.NULL || nodeType == NodeType.SequenceItem) && (parm == "Null")))
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
