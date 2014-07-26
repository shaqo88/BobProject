using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic.SchemaTypes;
using BobProject.UtilityClasses;

namespace BobProject.ViewModel.Commands
{
    public class TreeViewItemCommand : ICommand
    {

        #region Constructor

        public TreeViewItemCommand()
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
            XmlSchemaWrapper schemaWr = (XmlSchemaWrapper)parameter;
            //MainWindow.Instance.HierarchyTreeTypesView
            //TreeViewHelper.Expand
            
        }

        #endregion
    }
}
