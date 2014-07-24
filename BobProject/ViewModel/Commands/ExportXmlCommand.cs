using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class ExportXmlCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly SaveXmlViewModel viewModel;

        #endregion


        #region Constructor

        public ExportXmlCommand(SaveXmlViewModel _viewModel)
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
            //check if all child attribute filed
            if (viewModel.SchemaDescriber.RootElement != null)
                return viewModel.SchemaDescriber.RootElement.AllChildAttributesFilled;
            return false;
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
            if (viewModel.PathFile == null || viewModel.PathFile == string.Empty)
            {
                MessageBox.Show("Invalid Path File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                bool isExportOk = viewModel.SchemaDescriber.ExportXmlNow(viewModel.PathFile,null,viewModel.EditorName);
                if (isExportOk)
                    MessageBox.Show("Export File Succeeded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export File Failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
