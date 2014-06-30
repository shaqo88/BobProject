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
using BL.RegistryConfig;
using BL.SchemaLogic;
using BL.SchemaLogic.SchemaTypes;
using BobProject.UtilityClasses;
using BobProject.ViewModel;
using UIControls;

namespace BobProject
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel { get; private set; }


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

            InitializeComponent();

            LstColors.ItemsSource = ViewModel.TypesColor;
            HierarchyTreeTypesView.ItemsSource = ViewModel.TypesList;

            if (ViewModel.TypesList.Count == 0)
                return;

            //Right Panel - DEBUG            
            ElementName.DataContext = ViewModel.LastElementSelected;
            ElementAttributes.DataContext = ViewModel.LastElementSelected.Attributes;
            ElementAttributes.ItemsSource = ViewModel.LastElementSelected.Attributes;
            ChoiceComboBox.DataContext = ViewModel.LastChoiceSelected;
            //END DEBUG

            SelectComboChange.Command = ViewModel.UpdateTree;

            // Supply the control with the list of sections
            List<string> sections = new List<string> { "Name", "Attribute Value" };
            SearchBar.SectionsList = sections;

            // Choose a style for displaying sections
            SearchBar.SectionsStyle = SearchTextBox.SectionsStyles.RadioBoxStyle;

            // Add a routine handling the event OnSearch
            SearchBar.OnSearch += new RoutedEventHandler(OnSearch);

        }

        List<TreeViewItem> allItems = new List<TreeViewItem>();
        private List<TreeViewItem> GetAllItemContainers(ItemsControl itemsControl)
        {

            for (int i = 0; i < itemsControl.Items.Count; i++)
            {

                TreeViewItem item = (TreeViewItem)(itemsControl.ItemContainerGenerator.ContainerFromItem(itemsControl.Items[i]));
                if (item == null)
                    continue;
                if (item.HasItems)
                {
                    allItems.Add(item);
                    GetAllItemContainers(item);
                }
                else
                {
                    allItems.Add(item);
                }
            }
            return allItems;
        }


        private void OnSearch(object sender, RoutedEventArgs e)
        {
            SearchEventArgs searchArgs = e as SearchEventArgs;
            Color searchColor = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.Search);

            GetAllItemContainers((ItemsControl)HierarchyTreeTypesView);

            if (searchArgs.Sections[0] == "Name")
            {
                /*foreach (TreeViewItem item in allItems)
                {
                    if (item.Header.ToString().ToLower() == searchArgs.Keyword.ToLower())
                        item.Background = new SolidColorBrush(searchColor);
                }*/
            }
            else if (searchArgs.Sections[0] == "Attribute Value")
            {

            }

          
        }

        //DEBUG - sequence - numeric
        private int _numValue = 0;
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                SequenceSize.Text = value.ToString();
            }
        }
        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue > 0)
                NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(SequenceSize.Text, out _numValue))
                SequenceSize.Text = _numValue.ToString();
        }
        //END DEBUG

        private void OnSwitchUser(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Owner = this;
            //DEBUG
            //login.ShowDialog();
            //END DEBUG

        }

        private void OpenConfiguration(object sender, RoutedEventArgs e)
        {
            Configuration conf = new Configuration();
            conf.Owner = this;
            conf.ShowDialog();
        }

        private void OpenReports(object sender, RoutedEventArgs e)
        {
            Reports report = new Reports();
            report.Owner = this;
            report.ShowDialog();
        }

        private void OpenSaveAs(object sender, RoutedEventArgs e)
        {
            SaveXML saveXml = new SaveXML();
            saveXml.Owner = this;
            saveXml.ShowDialog();
        }


        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Owner = this;
            about.ShowDialog();
        }

        private void ShowSearchBar(object sender, RoutedEventArgs e)
        {
            ViewModel.IsShowSearchBar = !ViewModel.IsShowSearchBar;
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



        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            if (tvi != null)
            {
                if (tvi.Header != null)
                {
                    XmlSchemaWrapper schemaWr = tvi.Header as XmlSchemaWrapper;
                    if (!(schemaWr is XmlSchemaSequenceWrapper))
                    {
                        if (!schemaWr.AllChildrenDrilled)
                        {
                            schemaWr.DrillOnce();
                            foreach (var item in schemaWr.Children)
                            {
                                item.DrillOnce();
                            }
                        }
                    }

                }
            }
        }

    }

}
