using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BL.RegistryConfig
{
    /// <summary>
    /// Permission - Class that represent the various permissions types.
    /// the permission types store in the registry.
    /// </summary>
    public partial class Permission
    {
        #region enums

        public enum PermissionType { Viewer, Editor, Manager };
        private enum Regkeys { EditorUser, ViewerUser, ManagerUser, EditorPass, ViewerPass, ManagerPass };

        #endregion

        #region members

        private static Permission instance;
        private string editorUser;
        private string viewerUser;
        private string managerUser;
        private string editorPass;
        private string viewerPass;
        private string managerPass;
        private const string pathReg = "Bob\\Users"; //path of configuraion in the registry

        #endregion       

        #region Constructor

        /// <summary>
        /// singleton design
        /// </summary>
        private Permission()
        {
            ReadPermissionFromRegistry();
        }

        #endregion

        #region Properties

        public bool IsErrorLoading { get; private set; }

        public PermissionType CurrPermission { get; private set; }

        public static Permission Instance
        {
            get
            {
                if (instance == default(Permission))
                    instance = new Permission();
                return instance;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// check if input user and password are correct
        /// </summary>
        /// <param name="user">user name to be checked</param>
        /// <param name="pass">password to be checked</param>
        /// <returns>return true if user and password are correct. otherwise,false</returns>
        public bool CheckPermission(string user, string pass)
        {
            //check if it is manager permission
            if ((user == managerUser) && (pass == managerPass))
            {
                CurrPermission = PermissionType.Manager;
                return true;
            }
            //check if it is editor permission
            else if ((user == editorUser) && (pass == editorPass))
            {
                CurrPermission = PermissionType.Editor;
                return true;
            }
            //check if it is viewer permission
            else if ((user == viewerUser) && (pass == viewerPass))
            {
                CurrPermission = PermissionType.Viewer;
                return true;
            }

            //no permission
            return false;

        }

        #endregion

        #region private methods

        /// <summary>
        /// read permission values from registry
        /// </summary>
        private void ReadPermissionFromRegistry()
        {
            //read all permissions
            editorUser = GetRegisterValue(Regkeys.EditorUser);
            viewerUser = GetRegisterValue(Regkeys.ViewerUser);
            managerUser = GetRegisterValue(Regkeys.ManagerUser);
            editorPass = GetRegisterValue(Regkeys.EditorPass);
            viewerPass = GetRegisterValue(Regkeys.ViewerPass);
            managerPass = GetRegisterValue(Regkeys.ManagerPass);

            //check if error occured while loading
            if ((editorUser == null) || (viewerUser == null) || (managerUser == null) ||
                (editorPass == null) || (viewerPass == null) || (managerPass == null))
                IsErrorLoading = true;
            else
                IsErrorLoading = false;

        }


        /// <summary>
        /// get register value by key
        /// </summary>
        /// <param name="key">permission type</param>
        /// <returns>return the value from registry</returns>
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
                //error while reading value
                return null;
            }
        }

        #endregion

    }
}
