using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BobProject.UtilityClasses;

namespace BobProject.ViewModel
{
    public class ReportsViewModel : ViewModelBase
    {
        #region Fields

        private string pathFile;

        private string editorName;

        #endregion

        #region Constructor

        public ReportsViewModel()
        {
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

        public ICommand SelectRootDir { get; private set; }
        public ICommand SearchFiles { get; private set; }
        

        #endregion

        #region Private Methods

        private void Initialize()
        {
            //init commands
            /*
            SelectRootDir = new SelectXmlExportFileCommand(this);
            SearchFiles = new ExportXmlCommand(this);
            */

            //init members
            EditorName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;             
        }

        #endregion

    }
}
