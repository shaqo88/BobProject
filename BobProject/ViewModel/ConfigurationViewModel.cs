using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using BL.RegistryConfig;
using BL.UtilityClasses;
using BobProject.UtilityClasses;
using BobProject.ViewModel.Commands;

namespace BobProject.ViewModel
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region Fields

        private ObservableDictionary<string, Color> m_typesColor;

        #endregion

        #region Properties

        public ICommand PickerColor { get; set; }

        public ICommand SaveConfig { get; set; }

        public ICommand LoadNewSchema { get; set; }

        public KeyValuePair<string, Color> SelectedItem { get; private set; }

        public Window Parent { get; set; }

        public bool IsManagerPerm { get; private set; }
       
        public ObservableDictionary<string, Color> TypesColor
        {
            get { return m_typesColor; }
            set
            {
                m_typesColor = value;
                base.RaisePropertyChangedEvent("TypesColor");
            }
        }

        #endregion

        #region Constructor

        public ConfigurationViewModel()
        {
            //init members and commands
            m_typesColor = ConfigurationData.Instance.TypesColor;
            SaveConfig = new SaveConfigCommand();
            PickerColor = new PickerColorCommand(this, MainWindow.Instance.ViewModel);
            LoadNewSchema = new LoadNewSchemaCommand(MainWindow.Instance.ViewModel);
            IsManagerPerm = Permission.Instance.CurrPermission == Permission.PermissionType.Manager;
        }

        #endregion

    }
}
