using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class SelectedChoiceChangeCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel viewModel;

        #endregion

        #region Constructor

        public SelectedChoiceChangeCommand(MainWindowViewModel _viewModel)
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
            viewModel.SelectedItem.DrillOnce();
            foreach (var item in viewModel.SelectedItem.Children)
            {
                item.DrillOnce();
            }

            //Call Update treeview Colors
            var typeColor = viewModel.TypesColor;
            viewModel.TypesColor = typeColor;
        }

        #endregion
    }
}
