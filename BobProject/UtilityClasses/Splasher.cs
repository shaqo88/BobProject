using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BobProject
{
    /// <summary>
    /// Helper to show or close given splash window
    /// </summary>
    public static class Splasher
    {

        #region Fields

        private static Window m_Splash;

        #endregion

        #region Window Splash Instance

        /// <summary>
        /// Get or set the splash screen window
        /// </summary>
        public static Window Splash
        {
            get
            {
                return m_Splash;
            }
            set
            {
                m_Splash = value;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Show splash screen
        /// </summary>
        public static void ShowSplash()
        {
            if (m_Splash != null)
            {
                m_Splash.Show();
            }
        }
        /// <summary>
        /// Close splash screen
        /// </summary>
        public static void CloseSplash()
        {
            if (m_Splash != null)
            {
                m_Splash.Close();

                if (m_Splash is IDisposable)
                    (m_Splash as IDisposable).Dispose();
            }
        }

        #endregion
    }
}
