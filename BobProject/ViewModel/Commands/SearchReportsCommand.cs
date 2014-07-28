using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BL.UtilityClasses;
using BL.XmlLogic;

namespace BobProject.ViewModel.Commands
{
    public class SearchReportsCommand : ICommand
    {

        #region Fields

        // Member variables
        private readonly ReportsViewModel m_viewModel;

        #endregion

        #region Constructor

        public SearchReportsCommand(ReportsViewModel _viewModel)
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
            //check if path not empty
            return ((m_viewModel.PathFile != null) && (m_viewModel.PathFile != string.Empty));
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
            string editorName = null;
            DateRange dateRange = null;

            //check sections and update temp variables 
            if (m_viewModel.IsEditorSearch)
            {                
                editorName = m_viewModel.EditorName;
                if (editorName == null || editorName == string.Empty)
                {
                    MessageBox.Show("Editor name empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (m_viewModel.IsDateSearch)
                dateRange = new DateRange(m_viewModel.StartDate, m_viewModel.EndDate);

            //produceReports
            m_viewModel.LastSearchResults =  m_viewModel.SchemaDescriber.ProduceReport(m_viewModel.PathFile,editorName,dateRange);
        }

        #endregion
    }
}

