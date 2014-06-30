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
            viewModel.SelectedItem = schemaWr.NodeType;

            if (schemaWr.NodeType == NodeType.Element)
                viewModel.LastElementSelected = (XmlSchemaElementWrapper)schemaWr;
            else if (schemaWr.NodeType == NodeType.Choice)
                viewModel.LastChoiceSelected = (XmlSchemaChoiceWrapper)schemaWr;
            else if (schemaWr.NodeType == NodeType.Sequence)
                viewModel.LastSequenceSelected = (XmlSchemaSequenceWrapper)schemaWr;


            //Update Properties GUI
            MainWindow.Instance.ElementName.DataContext = viewModel.LastElementSelected;
            MainWindow.Instance.ElementAttributes.DataContext = viewModel.LastElementSelected.Attributes;
            MainWindow.Instance.ElementAttributes.ItemsSource = viewModel.LastElementSelected.Attributes;
            MainWindow.Instance.ChoiceComboBox.DataContext = viewModel.LastChoiceSelected;


        }

        #endregion
    }
}
