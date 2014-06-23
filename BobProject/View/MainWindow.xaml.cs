using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL.SchemaLogic;
using BL.SchemaLogic.SchemaTypes;
using BobProject.UtilityClasses;
using BobProject.ViewModel;

namespace BobProject
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel { get; private set; }


        public bool IsFirstEnter { get; set; }
        private static MainWindow instance;

        public static MainWindow Instance
        {
            get
            {
                if (instance == default(MainWindow))
                    instance = new MainWindow();
                return instance;
            }
        }


        private MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            IsFirstEnter = true;
            
            InitializeComponent();

            LstColors.ItemsSource = ViewModel.TypesColor;
            HierarchyTreeView.ItemsSource = ViewModel.GetCurrtypesList;      
            
        }

        private void OnSwitchUser(object sender, RoutedEventArgs e)
        {
            IsFirstEnter = false;
            this.Hide();
        }

        private void OpenConfiguration(object sender, RoutedEventArgs e)
        {
            Configuration conf = new Configuration();
            conf.Show();
        }

        private void OpenReports(object sender, RoutedEventArgs e)
        {
            Reports report = new Reports();
            report.Show();
        }

        private void OpenSaveAs(object sender, RoutedEventArgs e)
        {
            SaveXML saveXml = new SaveXML();

            saveXml.Show();
        }

        private void OnTreeNodeDoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is TreeViewItem)
            {
                if (!((TreeViewItem)sender).IsSelected)
                {
                    return;
                }
            }

        }

        public void OnExit(object sender, EventArgs e)
        {
            Close();
            foreach (Window w in Application.Current.Windows)
            {
                w.Close();
            }

        }



    }

}
