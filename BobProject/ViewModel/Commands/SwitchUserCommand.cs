using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class SwitchUserCommand : ICommand
    {
        #region Fields

        // Member variables
        private Login login;

        #endregion

        #region Constructor

        public SwitchUserCommand()
        {

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
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {
            login = new Login();
            /////////////DEBUG
            //login.Show();     
            //end debug

        }

        #endregion
    }
}
