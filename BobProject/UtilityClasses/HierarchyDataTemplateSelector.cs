using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.UtilityClasses
{
    public class HierarchyDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate retval = null;
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null)
            {
                if (item is XmlSchemaElementWrapper)
                {
                    retval = element.FindResource("XmlSchemaElementWrapperTemplate") as DataTemplate;
                }
                else if (item is XmlSchemaNullChoice)
                {
                    retval = element.FindResource("XmlSchemaChoiceNullWrapperTemplate") as DataTemplate;
                }
                else if (item is XmlSchemaChoiceWrapper)
                {
                    retval = element.FindResource("XmlSchemaChoiceWrapperTemplate") as DataTemplate;
                }
                else if (item is XmlSchemaSequenceWrapper)
                {
                    retval = element.FindResource("XmlSchemaSequenceWrapperTemplate") as DataTemplate;
                }
            }

            return retval;
        }
    }
}
