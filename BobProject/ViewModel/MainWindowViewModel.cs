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
using BL.SchemaLogic;
using BL.SchemaLogic.SchemaTypes;
using BL.UtilityClasses;
using BobProject.UtilityClasses;
using BobProject.ViewModel.Commands;
using Microsoft.Win32;

namespace BobProject.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Fields

        // Property variables
        private ObservableCollection<XmlSchemaElementWrapper> RootTypesLst;
        private ObservableCollection<XmlSchemaElementWrapper> CurrTypesLst;
        private ObservableDictionary<string, Color> typesColor;
        public string SchemaPath { get; set; }
        private string LastError = "";
        private string permission;
        public ICommand ShowAbout { get; set; }
        public ICommand SwitchUser { get; set; }
        public string Permit
        {
            get { return Permission.Instance.GetCurrPermisssion().ToString(); }
            set
            {
                permission = Permission.Instance.GetCurrPermisssion().ToString();
                base.RaisePropertyChangedEvent("Permit");
            }
        }
        public bool isAboutOpen
        {
            get
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is About)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Command Properties

        /// <summary>
        /// Deletes the currently-selected item from the types list.
        /// </summary>
        public ICommand UpdateTypes { get; set; }

        #endregion

        public ObservableDictionary<string, Color> TypesColor
        {
            get { return typesColor; }
            set
            {
                typesColor = value;
                base.RaisePropertyChangedEvent("TypesColor");
            }
        }


        #region Constructor

        public MainWindowViewModel()
        {
            this.Initialize();
        }

        #endregion


        #region Data Properties

        /// <summary>
        /// A types list.
        /// </summary>
        public ObservableCollection<XmlSchemaElementWrapper> GetCurrtypesList
        {
            get { return CurrTypesLst; }

            set
            {
                CurrTypesLst = value;
                base.RaisePropertyChangedEvent("GetCurrtypesList");
            }
        }
        #endregion



        #region Methods
        private void Initialize()
        {
            // Initialize commands
            this.UpdateTypes = new UpdateTreeCommand(this);
            ShowAbout = new ShowAboutCommand(this);
            SwitchUser = new SwitchUserCommand();

            // Create types List
            CurrTypesLst = new ObservableCollection<XmlSchemaElementWrapper>();
            RootTypesLst = new ObservableCollection<XmlSchemaElementWrapper>();
            typesColor = ConfigurationData.Instance.TypesColor;
            SchemaPath = ConfigurationData.Instance.SchemaPath;


            //Load schema
            try
            {
                var describer = new SchemaDescriber(SchemaPath);
                RootTypesLst = describer.Elements;
                CurrTypesLst = describer.Elements;
                var r = describer.Elements[0].Children[0].Children[1].Children[1];
                r.DrillOnce();

                GetCurrtypesList = CurrTypesLst;

            }
            catch (Exception)
            {
                LastError = "Error loading schema";
                return;
            }
            // Update bindings
            base.RaisePropertyChangedEvent("GetCurrtypesList");

        }


        public bool IsErrorOccur()
        {
            return LastError != "";
        }




        #endregion


    }
}





