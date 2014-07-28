using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    public class ShowViewsCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel m_viewModel;
        private string[] views = { "Login", "Configuration", "Reports", "SaveXML", "About" };

        #endregion

        #region Constructor

        public ShowViewsCommand(MainWindowViewModel _viewModel)
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
            string viewName = (string)parameter;
            if (viewName != null)
            {
                foreach (string _viewName in views)
                {
                    if (viewName.Equals(_viewName))
                        return true;
                }
            }
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
            string viewName = (string)parameter;
            Window windowToShow = null;

            //check which window to show
            if (viewName.Equals("Login"))
                windowToShow = new Login();
            else if (viewName.Equals("Configuration"))
                windowToShow = new Configuration();
            else if (viewName.Equals("Reports"))
                windowToShow = new Reports(m_viewModel.SchemaDescriber);
            else if (viewName.Equals("SaveXML"))
            {
               if (m_viewModel.SchemaDescriber.RootElement != null)
               {
                   //check if all child attributes filled
                    bool isAllAttRootFill = m_viewModel.SchemaDescriber.RootElement.AllChildAttributesFilled;
                    if (isAllAttRootFill == false)
                    {
                        MessageBox.Show("Could not export XML because not all required attributes filled yet", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                        windowToShow = new SaveXML(m_viewModel.SchemaDescriber);
               }
            }
            else if (viewName.Equals("About"))
                windowToShow = new About();
            else return;

            //set owner window
            windowToShow.Owner = MainWindow.Instance;

            //DEBUG
            //if (!viewName.Equals("Login")) //END DEBUG
                windowToShow.ShowDialog();

               

        }

        #endregion
    }
}
