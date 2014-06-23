using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL.RegistryConfig;
using BobProject.UtilityClasses;
using Microsoft.Win32;

namespace BobProject
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        public Login()
        {
            if (Permission.Instance.IsErrorLoading() || ConfigurationData.Instance.IsErrorLoadingSchema ||
                ConfigurationData.Instance.IsErrorLoadingColors)
            {
                MessageBox.Show("Configuration Error. Please ReInstall Application", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            InitializeComponent();

            /////////////DEBUG
            Send_Click(this, null);
            ////////////END DEBUG
        }




        [STAThread()]
        private void ShowSplashScreen()
        {
            this.Hide();
            MainWindow.Instance.IsFirstEnter = true;
            Splasher.Splash = new SplashScreen();
            Splasher.ShowSplash();

            for (int i = 0; i < 2; i++) ////////////////15
            {
                if (i % 4 == 0)
                    MessageListener.Instance.ReceiveMessage(string.Format("Loading"));
                else if (i % 4 == 1)
                    MessageListener.Instance.ReceiveMessage(string.Format("Loading."));
                else if (i % 4 == 2)
                    MessageListener.Instance.ReceiveMessage(string.Format("Loading.."));
                else if (i % 4 == 3)
                    MessageListener.Instance.ReceiveMessage(string.Format("Loading..."));
                Thread.Sleep(250);
            }

            Splasher.CloseSplash();
            MainWindow.Instance.Show();
            this.Close();
        }


        private void Send_Click(object sender, RoutedEventArgs e)
        {
            bool okPermission = Permission.Instance.CheckPermission(userNameTxt.Text, passwordTxt.Password);

            if (okPermission && ConfigurationData.Instance.SchemaPath == "" && Permission.Instance.GetCurrPermisssion() != Permission.PermissionType.Manager)
            {
                MessageBox.Show("Schema Path Empty. Please Contact Manager.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (okPermission)
                failLogin.Visibility = Visibility.Hidden;
            else
                failLogin.Visibility = Visibility.Visible;

            if (failLogin.Visibility == Visibility.Hidden)
            {
                MainWindow.Instance.ViewModel.Permit = Permission.Instance.GetCurrPermisssion().ToString();
                ShowSplashScreen();

            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            userNameTxt.Clear();
            passwordTxt.Clear();
            failLogin.Visibility = Visibility.Hidden;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send_Click(sender, e);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (!MainWindow.Instance.IsFirstEnter)
                MainWindow.Instance.OnExit(this, e);
        }

    }
}
