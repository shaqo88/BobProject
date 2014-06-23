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
        private string schemaPath;
        private ObservableDictionary<string, Color> typesColor;
        public ICommand PickerColor { get; set; }
        public ICommand SaveConfig { get; set; }
        public ICommand LoadNewSchema { get; set; }
        public KeyValuePair<string, Color> SelectedItem { get; private set; }
        public Window Parent { get; set; }
        public bool IsManagerPerm { get; private set; }

        public string SchemaPath
        {
            get { return schemaPath; }
            set
            {
                schemaPath = value;
                ConfigurationData.Instance.SchemaPath = value;
                base.RaisePropertyChangedEvent("SchemaPath");
            }
        }


        public ObservableDictionary<string, Color> TypesColor
        {
            get { return typesColor; }
            set
            {
                typesColor = value;
                base.RaisePropertyChangedEvent("TypesColor");
            }
        }

        public ConfigurationViewModel()
        {
            schemaPath = ConfigurationData.Instance.SchemaPath;
            typesColor = ConfigurationData.Instance.TypesColor;
            SaveConfig = new SaveConfigCommand();
            PickerColor = new PickerColorCommand(this, MainWindow.Instance.ViewModel);
            LoadNewSchema = new LoadNewSchemaCommand(this);
            IsManagerPerm = Permission.Instance.GetCurrPermisssion() == Permission.PermissionType.Manager;
        }

    }
}
