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

        private ObservableCollection<XmlSchemaWrapper> m_typesLst;

        private ObservableDictionary<string, Color> m_typesColor;

        private XmlSchemaWrapper m_selectedItem;

        private string m_permission;

        private string m_schemaPath;

        private bool m_isShowSearchBar = true;

        private SchemaDescriber m_schemaDescriber;

        #endregion

        #region Properties

        public ICommand ShowProperties { get; private set; }

        public ICommand UpdateSeqNumItems { get; private set; }

        public ICommand SelectedChoiceChange { get; private set; }

        public ICommand DeleteSeqItem { get; private set; }

        public ICommand LoadNewFile { get; private set; }

        public ICommand ShowViews { get; private set; }

        public SchemaDescriber SchemaDescriber
        {
            get { return m_schemaDescriber; }
        }

        public bool IsShowSearchBar
        {
            get { return m_isShowSearchBar; }
            set
            {
                m_isShowSearchBar = value;
                base.RaisePropertyChangedEvent("IsShowSearchBar");
            }
        }

        public XmlSchemaWrapper SelectedItem
        {
            get { return m_selectedItem; }
            set
            {
                m_selectedItem = value;
                base.RaisePropertyChangedEvent("SelectedItem");
            }
        }

        public string Permit
        {
            get { return Permission.Instance.CurrPermission.ToString(); }
            set
            {
                m_permission = Permission.Instance.CurrPermission.ToString();
                base.RaisePropertyChangedEvent("Permit");
            }
        }

        public string SchemaPath
        {
            get { return m_schemaPath; }
            set
            {
                m_schemaPath = value;
                ConfigurationData.Instance.SchemaPath = value;
                base.RaisePropertyChangedEvent("SchemaPath");
            }
        }        

        public ObservableDictionary<string, Color> TypesColor
        {
            get { return m_typesColor; }
            set
            {
                m_typesColor = value;
                base.RaisePropertyChangedEvent("TypesColor");
            }
        }

        public ObservableCollection<XmlSchemaWrapper> TypesList
        {
            get { return m_typesLst; }

            set
            {
                m_typesLst = value;
                base.RaisePropertyChangedEvent("TypesList");
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            this.Initialize();
        }

        #endregion
      
        #region Private Methods

        private void Initialize()
        {
            // Initialize commands
            ShowProperties = new ShowPropertiesCommand(this);
            SelectedChoiceChange = new SelectedChoiceChangeCommand(this);
            DeleteSeqItem = new DeleteSeqItemCommand(this);
            LoadNewFile = new LoadNewSchemaCommand(this);
            ShowViews = new ShowViewsCommand(this);            
            UpdateSeqNumItems = new UpdateSeqNumItemsCommand();
            

            // Create types List and color list 
            m_typesLst = new ObservableCollection<XmlSchemaWrapper>();
            TypesColor = ConfigurationData.Instance.TypesColor;
            SchemaPath = ConfigurationData.Instance.SchemaPath;

            //Load schema            
            this.LoadSchema();           

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load and reload schema from config path
        /// </summary>
        /// <returns></returns>
        public bool LoadSchema()
        {           
            bool isValidateSchema = true;
            try
            {
                m_schemaDescriber = new SchemaDescriber(SchemaPath);
                isValidateSchema = m_schemaDescriber.ValidateSchema(SchemaPath, true);
                //schemaDescriber.LoadSchema(SchemaPath);
            }
            catch (Exception)
            {
                throw new Exception( "Schema Invalid" );
            }
            

            if (isValidateSchema)
            {
                TypesList.Clear();
                XmlSchemaElementWrapper rootElement = m_schemaDescriber.RootElement;
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





