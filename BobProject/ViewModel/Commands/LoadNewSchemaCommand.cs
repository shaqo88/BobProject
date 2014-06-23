using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.RegistryConfig;
using BL.UtilityClasses;

namespace BobProject.ViewModel.Commands
{
    public class LoadNewSchemaCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly ConfigurationViewModel viewModel;

        #endregion

        #region Constructor

        public LoadNewSchemaCommand(ConfigurationViewModel _viewModel)
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
            return (Permission.Instance.GetCurrPermisssion() == Permission.PermissionType.Manager);
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
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xsd";
            dlg.Filter = "Schema Files (*.xsd)|*.xsd";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                ////////////////////////////////check valid schema

                viewModel.SchemaPath = filename;
            }
        }

        #endregion
    }
}
