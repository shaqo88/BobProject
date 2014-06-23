using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BL.UtilityClasses;

namespace BobProject.ViewModel.Commands
{
    public class PickerColorCommand : ICommand
    {
        #region Fields

        // Member variables
        private readonly ConfigurationViewModel viewModelConf;
        private readonly MainWindowViewModel viewModelMain;

        #endregion

        #region Constructor

        public PickerColorCommand(ConfigurationViewModel _viewModelConf, MainWindowViewModel _viewModelMain)
        {
            viewModelConf = _viewModelConf;
            viewModelMain = _viewModelMain;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            var selectedItem = viewModelConf.SelectedItem;
            return (selectedItem.Key != null);
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
            var selectedItem = viewModelConf.SelectedItem;

            Microsoft.Samples.CustomControls.ColorPickerDialog cPicker
              = new Microsoft.Samples.CustomControls.ColorPickerDialog();
            cPicker.StartingColor = selectedItem.Value;
            cPicker.Owner = viewModelConf.Parent;

            bool? dialogResult = cPicker.ShowDialog();
            if (dialogResult != null && (bool)dialogResult == true)
            {
                Color selectedColor = cPicker.SelectedColor;
                viewModelConf.TypesColor[selectedItem.Key] = selectedColor;
                viewModelMain.TypesColor[selectedItem.Key] = selectedColor;
            }
        }

        #endregion
    }
}
