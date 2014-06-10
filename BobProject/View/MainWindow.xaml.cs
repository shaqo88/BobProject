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

        public class Field {
            public string Name { get; set; }
            public int Length { get; set; }
            public bool Required { get; set; }
        }


        private MainWindowViewModel ViewModel;

        public ObservableCollection<Field> Fields { get; set; }


        public static readonly DependencyProperty CompanyNameProperty =
        DependencyProperty.Register("Perm", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty));

        public string Perm
        {
            get { return (string)this.GetValue(CompanyNameProperty); }
            set { this.SetValue(CompanyNameProperty, value); }
        }


        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            Perm = Permission.Instance.GetCurrPermisssion().ToString();

            InitializeComponent();
                    
            trvTypes.DataContext = ViewModel.GetCurrtypesList;
      

            Fields = new ObservableCollection<Field>();
            Fields.Add(new Field() { Name = "Username", Length = 100, Required = true });
            Fields.Add(new Field() { Name = "Password", Length = 80, Required = true });
            Fields.Add(new Field() { Name = "City", Length = 100, Required = false });
            Fields.Add(new Field() { Name = "State", Length = 40, Required = false });
            Fields.Add(new Field() { Name = "Zipcode", Length = 60, Required = false });

            FieldsListBox.ItemsSource = Fields;

            
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void OnSwitchUser(object sender, RoutedEventArgs e)
        {
            /*Login login = new Login();
            login.Show();*/
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

           /* ObservableCollection<XmlSchemaElementWrapper> x = ViewModel.GetCurrtypesList;
            x[0].DrillOnce();            
            x.Add(x[0]);
            ViewModel.GetCurrtypesList = x;*/

        }

    }

}
