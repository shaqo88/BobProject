using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.RegistryConfig;

namespace BobProject.ViewModel.Commands
{
    public class SaveConfigCommand : ICommand
    {

        #region Constructor

        public SaveConfigCommand()
        {
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            //check permission - manager
            return (Permission.Instance.CurrPermission == Permission.PermissionType.Manager);
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
            //call save config  - in registry
            ConfigurationData.Instance.SaveConfig();
        }

        #endregion
    }
}