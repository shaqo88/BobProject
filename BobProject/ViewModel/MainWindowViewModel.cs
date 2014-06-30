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
        private NodeType selectedItem;
        private XmlSchemaElementWrapper lastElementSelected;
        private XmlSchemaChoiceWrapper lastChoiceSelected;
        private XmlSchemaSequenceWrapper lastSequenceSelected;
        private string LastError = "";
        private string permission;
        private bool isShowSearchBar = true;
        public ICommand ShowProperties { get; set; }
        public ICommand UpdateTree { get; set; } 
        public bool IsShowSearchBar
        {
            get { return isShowSearchBar; }
            set
            {
                isShowSearchBar = value;
                base.RaisePropertyChangedEvent("IsShowSearchBar");
            }
        }
        public NodeType SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                base.RaisePropertyChangedEvent("SelectedItem");
            }
        }

        public XmlSchemaElementWrapper LastElementSelected
        {
            get { return lastElementSelected; }
            set
            {
                lastElementSelected = value;
                base.RaisePropertyChangedEvent("LastElementSelected");
            }
        }

        public XmlSchemaChoiceWrapper LastChoiceSelected
        {
            get { return lastChoiceSelected; }
            set
            {
                lastChoiceSelected = value;
                base.RaisePropertyChangedEvent("LastChoiceSelected");
            }
        }

        public XmlSchemaSequenceWrapper LastSequenceSelected
        {
            get { return lastSequenceSelected; }
            set
            {
                lastSequenceSelected = value;
                base.RaisePropertyChangedEvent("LastSequenceSelected");
            }
        }

        public string Permit
        {
            get { return Permission.Instance.GetCurrPermisssion().ToString(); }
            set
            {
                permission = Permission.Instance.GetCurrPermisssion().ToString();
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
                base.RaisePropertyChangedEvent("GetCurrtypesList");
            }
        }
        #endregion



        #region Methods
        private void Initialize()
        {
            // Initialize commands
            ShowProperties = new ShowPropertiesCommand(this);
            UpdateTree = new UpdateTreeCommand();

            // Create types List
            typesLst = new ObservableCollection<XmlSchemaWrapper>();
            typesColor = ConfigurationData.Instance.TypesColor;
            string schemaPath = ConfigurationData.Instance.SchemaPath;


            //Load schema
            try
            {
                var describer = new SchemaDescriber(schemaPath);

                typesLst.Add(describer.Elements[0]);
                SelectedItem = describer.Elements[0].NodeType;

                //DEBUG
                LastElementSelected = (XmlSchemaElementWrapper)describer.Elements[0];
                LastSequenceSelected = (XmlSchemaSequenceWrapper)LastElementSelected.Children[0];
                LastChoiceSelected = (XmlSchemaChoiceWrapper)LastSequenceSelected.Children[1];
                //END DEBUG



                TypesList = typesLst;

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





