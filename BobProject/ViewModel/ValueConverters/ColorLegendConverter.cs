using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using BL.RegistryConfig;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.ValueConverters
{
    public class ColorLegendConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) 
                return Brushes.Black; // Default color

            XmlSchemaWrapper schemaWr = (XmlSchemaWrapper)value;
            
            if (schemaWr != null)
            {
                NodeType type = schemaWr.NodeType;
                Color color;
                switch (type)
                {
                    case NodeType.Choice:                        
                        XmlSchemaChoiceWrapper schemaWrChoice = (XmlSchemaChoiceWrapper)value;
                        if (schemaWrChoice.Selected.NodeType == NodeType.NULL)
                            color = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.ChoiceNull);
                        else
                            color = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.Choice);                   
                        break;
                    case NodeType.Element:
                        color = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.Element);
                        break;
                    case NodeType.SequenceItem:
                    case NodeType.Sequence:
                        color = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.Sequence);
                        break;
                    case NodeType.NULL:
                        color = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.ChoiceNull);
                        break;
                    default:
                        return Brushes.Black; // Default color
                }
                                                                      
                return new SolidColorBrush(color);
            }
            return Brushes.Black; // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
