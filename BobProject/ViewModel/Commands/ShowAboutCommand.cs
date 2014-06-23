using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BobProject.ViewModel.Commands
{
    class ShowAboutCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly MainWindowViewModel viewModel;
        private About about;

        #endregion

        #region Constructor

        public ShowAboutCommand(MainWindowViewModel _viewModel)
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
            return true;//(ViewModel.SelectedItem != null);
        }

        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {

            foreach (Window w in Application.Current.Windows)
            {
                if (w is About)
                {
                    w.Activate();
                }
            }

            if (!viewModel.isAboutOpen)
            {
                about = new About();
                about.Show();
            }
        }

        #endregion
    }
}
