using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using BobProject.ViewModel;


namespace BobProject
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {

        public ConfigurationViewModel ViewModel { get; private set; }

        public Configuration()
        {

            ViewModel = new ConfigurationViewModel();
            ViewModel.Parent = this;
            DataContext = ViewModel;

            InitializeComponent();

            LstColors.ItemsSource = ViewModel.TypesColor;
            SchemaPath.DataContext = MainWindow.Instance.ViewModel;


        }


    }
}
