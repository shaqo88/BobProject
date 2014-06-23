using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BL.RegistryConfig
{
    public partial class Permission
    {
        public enum PermissionType { Viewer, Editor, Manager };
        private enum Regkeys { EditorUser, ViewerUser, ManagerUser, EditorPass, ViewerPass, ManagerPass };
        private static Permission instance;
        private string editorUser;
        private string viewerUser;
        private string managerUser;
        private string editorPass;
        private string viewerPass;
        private string managerPass;
        private bool isError;
        private PermissionType currPermission;
        private const string pathReg = "Bob\\Users";

        public static Permission Instance
        {
            get
            {
                if (instance == default(Permission))
                    instance = new Permission();
                return instance;
            }
        }


        #region ctor

        private Permission()
        {
            ReadPermissionFromRegistry();
        }

        #endregion

        #region public method

        public bool IsErrorLoading()
        {
            return isError;
        }

        public PermissionType GetCurrPermisssion()
        {
            return currPermission;
        }


        public bool CheckPermission(string user, string pass)
        {
            if ((user == managerUser) && (pass == managerPass))
            {
                currPermission = PermissionType.Manager;
                return true;
            }
            else if ((user == editorUser) && (pass == editorPass))
            {
                currPermission = PermissionType.Editor;
                return true;
            }
            else if ((user == viewerUser) && (pass == viewerPass))
            {
                currPermission = PermissionType.Viewer;
                return true;
            }

            return false;

        }

        #endregion

        #region private method

        private void ReadPermissionFromRegistry()
        {
            editorUser = GetRegisterValue(Regkeys.EditorUser);
            viewerUser = GetRegisterValue(Regkeys.ViewerUser);
            managerUser = GetRegisterValue(Regkeys.ManagerUser);
            editorPass = GetRegisterValue(Regkeys.EditorPass);
            viewerPass = GetRegisterValue(Regkeys.ViewerPass);
            managerPass = GetRegisterValue(Regkeys.ManagerPass);

            if ((editorUser == null) || (viewerUser == null) || (managerUser == null) ||
                (editorPass == null) || (viewerPass == null) || (managerPass == null))
                isError = true;
            else
                isError = false;

        }


        private string GetRegisterValue(Regkeys key)
        {
            string subkey = "";
            switch (key)
            {
                case Regkeys.EditorUser:
                    subkey = Regkeys.EditorUser.ToString();
                    break;
                case Regkeys.ViewerUser:
                    subkey = Regkeys.ViewerUser.ToString();
                    break;
                case Regkeys.ManagerUser:
                    subkey = Regkeys.ManagerUser.ToString();
                    break;
                case Regkeys.EditorPass:
                    subkey = Regkeys.EditorPass.ToString();
                    break;
                case Regkeys.ViewerPass:
                    subkey = Regkeys.ViewerPass.ToString();
                    break;
                case Regkeys.ManagerPass:
                    subkey = Regkeys.ManagerPass.ToString();
                    break;
            }

            try
            {
                // The name of the key must include a valid root. 
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(pathReg);
                if (regKey != null)
                    return (string)regKey.GetValue(subkey);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion



    }
}
