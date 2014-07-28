using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL.RegistryConfig;
using BL.SchemaLogic;
using BobProject.UtilityClasses;
using BobProject.ViewModel.Commands;

namespace BobProject.ViewModel
{
    public class SaveXmlViewModel : ViewModelBase
    {

        #region Fields

        private string m_pathFile;

        private string m_editorName;        

        #endregion

        #region Constructor

        public SaveXmlViewModel(SchemaDescriber schema)
        {
            SchemaDescriber = schema;
            this.Initialize();
        }

        
        #endregion

        #region Properties

        public SchemaDescriber SchemaDescriber { get; private set; }  

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

        public ICommand SelectXmlExportFile { get; private set; }

        public ICommand ExportXml { get; private set; }
        

        #endregion

        #region Private Methods

        private void Initialize()
        {
            //init commands
            SelectXmlExportFile = new SelectXmlExportFileCommand(this);
            ExportXml = new ExportXmlCommand(this);

            //init members
            EditorName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;             
        }

        #endregion

    }
}
