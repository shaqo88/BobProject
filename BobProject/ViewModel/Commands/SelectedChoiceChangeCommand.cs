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
        private readonly MainWindowViewModel m_viewModel;

        #endregion

        #region Constructor

        public SelectedChoiceChangeCommand(MainWindowViewModel _viewModel)
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
            //drill for new selected item
            m_viewModel.SelectedItem.DrillOnce();
            foreach (var item in m_viewModel.SelectedItem.Children)
            {
                item.DrillOnce();
            }

            //Call Update treeview Colors
            var typeColor = m_viewModel.TypesColor;
            m_viewModel.TypesColor = typeColor;
        }

        #endregion
    }
}
