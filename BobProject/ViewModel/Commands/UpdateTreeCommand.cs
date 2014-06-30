using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;

namespace BobProject.ViewModel.Commands
{
    public class UpdateTreeCommand : ICommand
    {

        #region Constructor

        public UpdateTreeCommand()
        {
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
            //Update Properties GUI
            MainWindow.Instance.HierarchyTreeTypesView.Items.Refresh();
            MainWindow.Instance.HierarchyTreeTypesView.UpdateLayout();

        }

        #endregion
    }
}
