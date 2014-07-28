using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.Commands
{
    public class DeleteSeqItemCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel m_viewModel;

        #endregion

        #region Constructor

        public DeleteSeqItemCommand(MainWindowViewModel _viewModel)
        {
            m_viewModel = _viewModel;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            //check permission
            if (m_viewModel.Permit.Equals("Viewer"))
                return false;

            //convert parameter
            XmlSchemaWrapper scWr = (XmlSchemaWrapper)parameter;

            //check if parameter is XmlSchemaWrapper type
            if (scWr != null)
            {
                //check if parameter is XmlSchemaSequenceWrapper type
                if (scWr is XmlSchemaSequenceWrapper)
                {                   
                    XmlSchemaWrapper parent = scWr.Parent;

                    //check if parent is XmlSchemaSequenceArray
                    if (parent is XmlSchemaSequenceArray)
                        return true;
                }
            }
            return false;
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
            var scWr = (XmlSchemaSequenceWrapper)parameter;
            var parent = (XmlSchemaSequenceArray)scWr.Parent;

            int index = scWr.Index + 1;

            //check if user want to delete
            MessageBoxResult result = MessageBox.Show("Would you like to delete item " + index.ToString() + " ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                //delete the selected item from array
                parent.RemoveAt(scWr.Index);

                //update Xxl view
                MainWindow.Instance.UpdateXMLView();
            }
        }

        #endregion
    }
}