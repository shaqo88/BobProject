using BL.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.XmlLogic
{
    /// <summary>
    /// Represents XMl's metadata like its user name, date and path
    /// </summary>
    public class XmlMetaData : PropertyNotifyObject
    {
        #region Properties

        private string m_xmlPath;

        public string XmlPath
        {
            get { return m_xmlPath; }
            set { SetProperty(ref m_xmlPath, value); }
        }

        private Version m_version;

        public Version Version
        {
            get { return m_version; }
            set { SetProperty(ref m_version, value); }
        }

        private DateTime m_date;

        public DateTime Date
        {
            get { return m_date; }
            set { SetProperty(ref m_date, value); }
        }

        private string m_userName;

        public string UserName
        {
            get { return m_userName; }
            set { SetProperty(ref m_userName, value); }
        }

        #endregion

        #region Constructor

        public XmlMetaData(string xmlPath, Version version, DateTime date, string userName)
        {
            XmlPath = xmlPath;
            Version = version;
            Date = date;
            UserName = userName;
        }

        #endregion
    }
}
