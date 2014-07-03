using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.Commands
{
    public class ShowPropertiesCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel viewModel;

        #endregion

        #region Constructor

        public ShowPropertiesCommand(MainWindowViewModel _viewModel)
        {
            viewModel = _viewModel;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Fires when the CanExecute status of this command changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {
            XmlSchemaWrapper schemaWr = (XmlSchemaWrapper)parameter;

            //check if conversion parameter to XmlSchemaWrapper succeed
            if (schemaWr != null)
            {
                viewModel.SelectedItem = schemaWr;

                switch (schemaWr.NodeType)
                {
                    case NodeType.Element:
                        XmlSchemaElementWrapper element = (XmlSchemaElementWrapper)schemaWr;
                        MainWindow.Instance.ElementName.DataContext = element;
                        MainWindow.Instance.ElementAttributes.DataContext = element.Attributes;
                        MainWindow.Instance.ElementAttributes.ItemsSource = element.Attributes;
                        break;
                    case NodeType.Choice:
                        XmlSchemaChoiceWrapper choice = (XmlSchemaChoiceWrapper)schemaWr;
                        MainWindow.Instance.ChoiceComboBox.DataContext = choice;
                        break;
                    case NodeType.Sequence:
                        XmlSchemaSequenceArray seqArr = (XmlSchemaSequenceArray)schemaWr;
                        MainWindow.Instance.SequenceSize.Value = seqArr.Count;
                        break;
                    case NodeType.SequenceItem:
                        break;
                    case NodeType.NULL:
                        break;
                    default:
                        break; 
                }
               
            }                        


        }

        #endregion
    }
}
