using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic;
using BL.SchemaLogic.SchemaTypes;
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
        public string SchemaPath { get; set; }
        private const string PathReg = "Bob\\Configuration";
        private string LastError = "";

        #endregion

        #region Command Properties

        /// <summary>
        /// Deletes the currently-selected item from the types list.
        /// </summary>
        public ICommand UpdateTypes { get; set; }

        #endregion


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

            // Create types List
            CurrTypesLst = new ObservableCollection<XmlSchemaElementWrapper>();
            RootTypesLst = new ObservableCollection<XmlSchemaElementWrapper>();

            
            //get schema path
            if (!ReadSchemaPath())
                return;

            //Load schema
            try
            {
                var describer = new SchemaDescriber(SchemaPath);
                RootTypesLst = describer.Elements;
                CurrTypesLst = describer.Elements; 
                // TODO : complete logic
                //int index = 0;
                //ObservableCollection<XmlSchemaElementWrapper> elements = CurrTypesLst[0].HandleGroups(ref index);                
                //var currElement2 = elements[3];
                //currElement2.DrillOnce();
                                               
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

        private bool ReadSchemaPath()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(PathReg);
            if (regKey != null)
                SchemaPath = (string)regKey.GetValue("SchemaPath");
            else
            {
                LastError = "Error get value from registry";
                return false;
            }
            return true;
        }


        #endregion


        

    }
}
