using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class SelectXmlExportFileCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly SaveXmlViewModel viewModel;

        #endregion

        //Consts

        #region Const

        public const string DefualtFileName = "XmlDocument";

        #endregion

        #region Constructor

        public SelectXmlExportFileCommand(SaveXmlViewModel _viewModel)
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
           Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
           dlg.FileName = DefualtFileName; // Default file name
           dlg.DefaultExt = ".xml"; // Default file extension
           dlg.Filter = "XML Files (.xml)|*.xml"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Select document
                string filename = dlg.FileName;
                viewModel.PathFile = filename;
            }

        }

        #endregion
    }
}
