using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.RegistryConfig;
using BL.UtilityClasses;

namespace BobProject.ViewModel.Commands
{
    public class LoadNewSchemaCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel m_viewModelMain;

        #endregion

        #region Constructor

        public LoadNewSchemaCommand(MainWindowViewModel _viewModelMain)
        {
            m_viewModelMain = _viewModelMain;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            //check if parameters are ok
            string arg = (string)parameter;
            if (arg == "path")
                return (Permission.Instance.CurrPermission == Permission.PermissionType.Manager);
            else if (arg == "local")
                return (Permission.Instance.CurrPermission != Permission.PermissionType.Viewer);
            else return false;
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
            MessageBoxResult result = MessageBox.Show("Are you sure that you want to load a new schema? This action deletes previous data.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
             if (result == MessageBoxResult.Yes)
             {
                 string arg = (string)parameter;
                 string filename = "";
                 if (arg == "path")
                 {
                     // Create OpenFileDialog 
                     Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


                     // Set filter for file extension and default file extension 
                     dlg.DefaultExt = ".xsd";
                     dlg.Filter = "Schema Files (*.xsd)|*.xsd";


                     // Display OpenFileDialog by calling ShowDialog method 
                     Nullable<bool> resultDlg = dlg.ShowDialog();
                     // Get the selected file name and display in a TextBox 
                     if (resultDlg == true)
                     {
                         // Open document 
                         filename = dlg.FileName;
                     }
                     else return;
                 }
                 else if (arg == "local")
                     filename = m_viewModelMain.SchemaPath;

                 
                try
                {
                    //load new schena and chech validation
                    m_viewModelMain.LoadSchema();
                    m_viewModelMain.SchemaPath = filename;


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

             }
            
        }

        #endregion
    }
}
