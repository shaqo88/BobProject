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

        private string pathFile;

        private string editorName;

        public SchemaDescriber SchemaDescriber { get; private set; }        

        #endregion

        #region Constructor

        public SaveXmlViewModel(SchemaDescriber schema)
        {
            SchemaDescriber = schema;
            this.Initialize();
        }

        
        #endregion


        #region Properties

        public string PathFile 
        {
            get
            {
                return pathFile;
            }
            set
            {
                pathFile = value;
                base.RaisePropertyChangedEvent("PathFile");
            }
        }

        public string EditorName
        {
            get
            {
                return editorName;
            }
            set
            {
                editorName = value;
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
