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
        private ObservableCollection<XmlSchemaWrapper> typesLst;
        private ObservableDictionary<string, Color> typesColor;
        private XmlSchemaWrapper selectedItem;
        private string LastError = "";
        private string permission;
        private bool isShowSearchBar = true;
        private SchemaDescriber schemaDescriber;
        public ICommand ShowProperties { get; private set; }
        public ICommand UpdateSeqNumItems { get; private set; }
        public ICommand SelectedChoiceChange { get; private set; }
        public ICommand DeleteSeqItem { get; private set; }

        public bool IsShowSearchBar
        {
            get { return isShowSearchBar; }
            set
            {
                isShowSearchBar = value;
                base.RaisePropertyChangedEvent("IsShowSearchBar");
            }
        }
        public XmlSchemaWrapper SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                base.RaisePropertyChangedEvent("SelectedItem");
            }
        }

        public string Permit
        {
            get { return Permission.Instance.CurrPermission.ToString(); }
            set
            {
                permission = Permission.Instance.CurrPermission.ToString();
                base.RaisePropertyChangedEvent("Permit");
            }
        }


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


        public ObservableCollection<XmlSchemaWrapper> TypesList
        {
            get { return typesLst; }

            set
            {
                typesLst = value;
                base.RaisePropertyChangedEvent("TypesList");
            }
        }
        #endregion


        #region Methods
        private void Initialize()
        {
            // Initialize commands
            ShowProperties = new ShowPropertiesCommand(this);
            UpdateSeqNumItems = new UpdateSeqNumItemsCommand();
            SelectedChoiceChange = new SelectedChoiceChangeCommand(this);
            DeleteSeqItem = new DeleteSeqItemCommand();


            // Create types List
            typesLst = new ObservableCollection<XmlSchemaWrapper>();
            TypesColor = ConfigurationData.Instance.TypesColor;
            string schemaPath = ConfigurationData.Instance.SchemaPath;


            //Load schema
            try
            {
                schemaDescriber = new SchemaDescriber(schemaPath);                

                typesLst.Add(schemaDescriber.Elements[0]);
                SelectedItem = schemaDescriber.Elements[0];

                //DEBUG - TODO - first time select the first element
                /*if (ShowProperties.CanExecute(describer.Elements[0]))
                    ShowProperties.Execute(describer.Elements[0]);*/
                //schemaDescriber.LoadSchema
                //schemaDescriber.ValidateSchema
                //END DEBUG

                TypesList = typesLst;

            }
            catch (Exception)
            {
                LastError = "Error loading schema";
                return;
            }
            // Update bindings
            base.RaisePropertyChangedEvent("TypesList");

        }


        public bool IsErrorOccur()
        {
            return LastError != "";
        }


        #endregion


    }
}





