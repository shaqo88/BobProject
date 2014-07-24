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
        #region Fields

        private static MainWindow instance;

        private List<TreeViewItem> m_lastItemsResult;
        
        #endregion

        #region DependencyProperty

        public int CurrentResultCount
        {
            get { return (int)GetValue(CurrentResultCountProperty); }
            set { SetValue(CurrentResultCountProperty, value); }
        }

        public static readonly DependencyProperty CurrentResultCountProperty =
        DependencyProperty.Register("CurrentResultCountProperty", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));

        public int TotalResultCount
        {
            get { return (int)GetValue(TotalResultCountProperty); }
            set { SetValue(TotalResultCountProperty, value); }
        }

        public static readonly DependencyProperty TotalResultCountProperty =
        DependencyProperty.Register("TotalResultCountProperty", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));

        #endregion

        #region Properties

        public MainWindowViewModel ViewModel { get; private set; }


        public static MainWindow Instance
        {
            get
            {
                if (instance == default(MainWindow))
                    instance = new MainWindow();
                return instance;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor - singleton design
        /// </summary>
        private MainWindow()
        {
            InitializeComponent();

            //Initialize bindings controls and members
            this.Initialize();            
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Initialize bindings controls and members
        /// </summary>
        private void Initialize()
        {
            //Initialize members
            m_lastItemsResult = new List<TreeViewItem>();
            CurrentResultCount = 0;
            TotalResultCount = 0;  

            try
            {
                ViewModel = new MainWindowViewModel();
            }
            catch (Exception ex)
            {
                //invalid schema - error
                MessageBox.Show(ex.Message , "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
            //set data context to UI controls
            DataContext = ViewModel;
            LstColors.ItemsSource = ViewModel.TypesColor;
            HierarchyTreeTypesView.ItemsSource = ViewModel.TypesList;
            CurrentResult.DataContext = this;
            TotalResult.DataContext = this;

            // Supply the control with the list of sections
            List<string> sections = new List<string> { "Name", "Attribute Value" };
            SearchBar.SectionsList = sections;

            // Choose a style for displaying sections
            SearchBar.SectionsStyle = SearchTextBox.SectionsStyles.CheckBoxStyle;

            // Add a routine handling the event OnSearch
            SearchBar.OnSearch += new RoutedEventHandler(OnSearch);

            XMLViewer.XmlContents = ViewModel.SchemaDescriber.CurrentXmlDocument;

        }

        /// <summary>
        /// Search command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearch(object sender, RoutedEventArgs e)
        {
            //get search arguments
            SearchEventArgs searchArgs = e as SearchEventArgs;

            //get background color of search from configuration
            Color searchColor = ConfigurationData.Instance.GetColorConfiguration(ConfigurationData.Regkeys.Search);

            //get search text
            string searchTxt = SearchBar.Text.ToLower();

            //check if section count is not zero
            if (searchArgs.Sections.Count == 0)
            {
                MessageBox.Show("Section Count is zero. You must select at least one section", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //delete last search
            EraseAllLastSearchResult();

            //Search xml by section
            List<XmlSchemaWrapper> resultLst = new List<XmlSchemaWrapper>();
            BL.XmlLogic.SearchEnum searchEnum = BL.XmlLogic.SearchEnum.All;
            if (searchArgs.Sections.Count == SearchBar.SectionsList.Count)
            {
                searchEnum = BL.XmlLogic.SearchEnum.All;
            }
            else  if (searchArgs.Sections[0] == "Name")
            {
                searchEnum = BL.XmlLogic.SearchEnum.NodeName;
            }
            else if (searchArgs.Sections[0] == "Attribute Value")
            {
                searchEnum = BL.XmlLogic.SearchEnum.Attribute;                
            }
            resultLst = ViewModel.SchemaDescriber.SearchXml(ViewModel.TypesList[0], searchTxt, searchEnum);                

            //run over the result and paint items background
            m_lastItemsResult = TreeViewHelper.SelectItems(HierarchyTreeTypesView, resultLst.Cast<object>().ToArray(), searchColor);
            TotalResultCount = m_lastItemsResult.Count;

            //view the first result
            ViewNextResult();

            //show search result text            
            ResultPanel.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Bring Front the next result item
        /// </summary>
        private void ViewNextResult()
        {
            if (TotalResultCount == 0)
                return;

            //check if we got to end of array
            if ( CurrentResultCount >= TotalResultCount )
                CurrentResultCount = 0;

            //bring to front of tree view
            m_lastItemsResult.ToArray()[CurrentResultCount].BringIntoView();
            m_lastItemsResult.ToArray()[CurrentResultCount].IsSelected = true;
            m_lastItemsResult.ToArray()[CurrentResultCount].IsExpanded = true;
            m_lastItemsResult.ToArray()[CurrentResultCount].Focus();

            //update to next view
            CurrentResultCount++;
        }


        /// <summary>
        /// Erase all last search result items
        /// </summary>
        private void EraseAllLastSearchResult()
        {
            //paint background of all last search items
            foreach (TreeViewItem item in m_lastItemsResult)
            {
                item.Background = Brushes.Transparent;
            }

            //remove all items
            m_lastItemsResult.Clear();

            //hide search result 
            ResultPanel.Visibility = Visibility.Hidden;

            //init search result counts
            CurrentResultCount = 0;
            TotalResultCount = 0;
        }


        /// <summary>
        /// on erase result click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEraseResult(object sender, RoutedEventArgs e)
        {
            //delete search result from treeview
            EraseAllLastSearchResult();

            //init search bar text
            SearchBar.Text = string.Empty;
        }

        /// <summary>
        /// on show next result event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowNextResult(object sender, RoutedEventArgs e)
        {
            //show next result on treeview
            ViewNextResult();
        }


        private void ShowSearchBar(object sender, RoutedEventArgs e)
        {
            ViewModel.IsShowSearchBar = !ViewModel.IsShowSearchBar;
        }


        /// <summary>
        /// Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnExit(object sender, EventArgs e)
        {
            //close all sub windows
            Close();
            foreach (Window w in Application.Current.Windows)
            {
                w.Close();
            }
        }


        /// <summary>
        /// on tree expand command - drill tree items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
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

            System.Xml.XmlDocument XMLdoc = null;
            try
            {
                XMLdoc = ViewModel.SchemaDescriber.GetCurrentXmlDocument();
            }
            catch (Exception)
            {
                
            }

            XMLViewer.XmlContents = XMLdoc;
            /*var xml = ViewModel.SchemaDescriber.CurrentXmlDocument;
            ViewModel.SchemaDescriber.CurrentXmlDocument = xml;*/

        }

        /// <summary>
        /// expand all tree view items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandTree(object sender, RoutedEventArgs e)
        {
            TreeViewHelper.ExpandAll(HierarchyTreeTypesView);
        }


        /// <summary>
        /// collapse all tree view items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollapseTree(object sender, RoutedEventArgs e)
        {
            TreeViewHelper.CollapseAll(HierarchyTreeTypesView);
        }

        /// <summary>
        /// on expand or collapse  tree view node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExpandCollapseNode(object sender, RoutedEventArgs e)
        {
            /*need to add contextmenu to treeview*/
           // HierarchyTreeTypesView.
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            /*string value = (string)((MenuItem)sender).Tag;
            if (value == "Expand")
                TreeViewHelper.ExpandCollapseNode(tvi, true);
            else if(value == "Collapse")
                TreeViewHelper.ExpandCollapseNode(tvi, false);*/
            
        }


        #endregion


    }

}
