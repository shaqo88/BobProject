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
        private string permission;
        private string schemaPath;
        private bool isShowSearchBar = true;
        private SchemaDescriber schemaDescriber;
        public ICommand ShowProperties { get; private set; }
        public ICommand UpdateSeqNumItems { get; private set; }
        public ICommand SelectedChoiceChange { get; private set; }
        public ICommand DeleteSeqItem { get; private set; }
        public ICommand LoadNewFile { get; private set; }
        public ICommand ShowViews { get; private set; }
        

        public SchemaDescriber SchemaDescriber
        {
            get { return schemaDescriber; }
        }

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


        #region Private Methods
        private void Initialize()
        {
            // Initialize commands
            ShowProperties = new ShowPropertiesCommand(this);
            SelectedChoiceChange = new SelectedChoiceChangeCommand(this);
            UpdateSeqNumItems = new UpdateSeqNumItemsCommand();
            DeleteSeqItem = new DeleteSeqItemCommand();
            LoadNewFile = new LoadNewSchemaCommand(this);
            ShowViews = new ShowViewsCommand(this);

            // Create types List and color list 
            typesLst = new ObservableCollection<XmlSchemaWrapper>();
            TypesColor = ConfigurationData.Instance.TypesColor;
            SchemaPath = ConfigurationData.Instance.SchemaPath;

            //Load schema
            schemaDescriber = new SchemaDescriber(SchemaPath);
            this.LoadSchema();           

        }

        public bool LoadSchema()
        {           
            bool isValidateSchema = true;
            try
            {
                isValidateSchema = schemaDescriber.ValidateSchema(SchemaPath, true);
                schemaDescriber.LoadSchema(SchemaPath);
            }
            catch (Exception)
            {
                throw new Exception( "Schema Invalid" );
            }
            

            if (isValidateSchema)
            {
                TypesList.Clear();
                XmlSchemaElementWrapper rootElement = schemaDescriber.RootElement;
                if (rootElement != null)
                {
                    TypesList.Add(rootElement);
                    SelectedItem = rootElement;                        
                }
                base.RaisePropertyChangedEvent("TypesList");                                
            }


            return isValidateSchema;

            
        }




        #endregion


    }
}





