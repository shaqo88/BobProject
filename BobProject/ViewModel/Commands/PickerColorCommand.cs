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
        private readonly ConfigurationViewModel m_viewModelConf;
        private readonly MainWindowViewModel m_viewModelMain;

        #endregion

        #region Constructor

        public PickerColorCommand(ConfigurationViewModel _viewModelConf, MainWindowViewModel _viewModelMain)
        {
            m_viewModelConf = _viewModelConf;
            m_viewModelMain = _viewModelMain;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            var selectedItem = m_viewModelConf.SelectedItem;
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
            var selectedItem = m_viewModelConf.SelectedItem;

            //load pick color dialog
            Microsoft.Samples.CustomControls.ColorPickerDialog cPicker
              = new Microsoft.Samples.CustomControls.ColorPickerDialog();
            cPicker.StartingColor = selectedItem.Value;
            cPicker.Owner = m_viewModelConf.Parent;

            bool? dialogResult = cPicker.ShowDialog();
            if (dialogResult != null && (bool)dialogResult == true)
            {
                //set to selected color
                Color selectedColor = cPicker.SelectedColor;
                ObservableDictionary<string, Color> TypesColor = m_viewModelConf.TypesColor;
                TypesColor[selectedItem.Key] = selectedColor;

                //update treeview GUI
                m_viewModelConf.TypesColor = TypesColor;
                m_viewModelMain.TypesColor = TypesColor;

            }
        }

        #endregion
    }
}
