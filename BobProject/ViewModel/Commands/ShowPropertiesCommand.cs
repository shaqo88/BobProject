using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class ShowPropertiesCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel ViewModel;

        #endregion

        #region Constructor

        public ShowPropertiesCommand(MainWindowViewModel _viewModel)
        {
            ViewModel = _viewModel;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;//(ViewModel.SelectedItem != null);
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
            /*var selectedItem = m_ViewModel.SelectedItem;
            m_ViewModel.GroceryList.Remove(selectedItem);*/
        }

        #endregion
    }
}
