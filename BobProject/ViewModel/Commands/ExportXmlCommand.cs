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
        private readonly SaveXmlViewModel m_viewModel;

        #endregion

        #region Constructor

        public ExportXmlCommand(SaveXmlViewModel _viewModel)
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
            //check if all child attribute filed
            if (m_viewModel.SchemaDescriber.RootElement != null)
                return m_viewModel.SchemaDescriber.RootElement.AllChildAttributesFilled;
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
            //check if path is not empty
            if (m_viewModel.PathFile == null || m_viewModel.PathFile == string.Empty)
            {
                MessageBox.Show("Invalid Path File", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                //try to export xml
                bool isExportOk = m_viewModel.SchemaDescriber.ExportXmlNow(m_viewModel.PathFile,null,m_viewModel.EditorName);
                if (isExportOk)
                    MessageBox.Show("Export File Succeeded!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export File Failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            catch (Exception ex)
            {
                //not Succeeded
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}
