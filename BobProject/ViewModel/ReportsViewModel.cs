using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.SchemaLogic;
using BL.XmlLogic;
using BobProject.UtilityClasses;
using BobProject.ViewModel.Commands;

namespace BobProject.ViewModel
{
    public class ReportsViewModel : ViewModelBase
    {
        #region Fields

        private string m_pathFile;

        private string m_editorName;

        private DateTime m_startDate;

        private DateTime m_endDate;

        private ObservableCollection<XmlMetaData> m_lastSearchResults;

        private bool m_isEditorSearch;

        private bool m_isDateSearch;        

        #endregion

        #region Constructor

        public ReportsViewModel(SchemaDescriber schema)
        {
            this.Initialize();
            SchemaDescriber = schema;
        }

        
        #endregion

        #region Properties

        public SchemaDescriber SchemaDescriber { get; private set; }

        public bool IsEditorSearch 
        {
            get
            {
                return m_isEditorSearch;
            }
            set
            {
                m_isEditorSearch = value;
                base.RaisePropertyChangedEvent("IsEditorSearch");
            }
        }

        public bool IsDateSearch 
        { 
            get
            {
                return m_isDateSearch;
            }
            set
            {
                m_isDateSearch = value;
                base.RaisePropertyChangedEvent("IsDateSearch");
            }
        }

        public string PathFile 
        {
            get
            {
                return m_pathFile;
            }
            set
            {
                m_pathFile = value;
                base.RaisePropertyChangedEvent("PathFile");
            }
        }

        public string EditorName
        {
            get
            {
                return m_editorName;
            }
            set
            {
                m_editorName = value;
                base.RaisePropertyChangedEvent("EditorName");
            }
        }

        public DateTime StartDate
        {
            get
            {
                return m_startDate;
            }
            set
            {
                m_startDate = value;
                base.RaisePropertyChangedEvent("StartDate");
            }
        }

        public DateTime EndDate
        {
            get
            {
                return m_endDate;
            }
            set
            {
                m_endDate = value;
                base.RaisePropertyChangedEvent("StartDate");
            }
        }

        public ObservableCollection<XmlMetaData> LastSearchResults 
        {
            get
            {
                return m_lastSearchResults;
            }
            set
            {
                m_lastSearchResults = value;
                base.RaisePropertyChangedEvent("LastSearchResults");
            }
        }
         

        public ICommand SearchFiles { get; private set; }
        

        #endregion

        #region Private Methods

        private void Initialize()
        {
            //init commands
            SearchFiles = new SearchReportsCommand(this);

            //init members
            EditorName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            StartDate = DateTime.Now;
            EndDate   = DateTime.Now;
            IsEditorSearch = true;
            m_isDateSearch = true;
        }

        #endregion

    }
}
