using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace BobProject
{
    /// <summary>
    /// Message listener, singlton pattern.
    /// Inherit from DependencyObject to implement DataBinding.
    /// </summary>
    public class MessageListener : DependencyObject
    {

        #region Fields

        private static MessageListener m_Instance;

        #endregion

        #region Constructor

        private MessageListener()
        {

        }

        #endregion

        #region Singleton instance

        /// <summary>
        /// Get MessageListener instance
        /// </summary>
        public static MessageListener Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new MessageListener();
                return m_Instance;
            }
        }

        #endregion

        #region public methods

        public void ReceiveMessage(string message)
        {
            Message = message;
            //Debug.WriteLine(Message);
            DispatcherHelper.DoEvents();
        }

        /// <summary>
        /// Get or set received message
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));

        #endregion

    }
}
