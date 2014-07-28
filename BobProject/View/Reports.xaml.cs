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
using BL.SchemaLogic;
using BobProject.ViewModel;

namespace BobProject
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {

        #region Properties

        public ReportsViewModel ViewModel { get; private set; }

        #endregion

        #region Constructor

        public Reports(SchemaDescriber schema)
        {
            InitializeComponent();

            ViewModel = new ReportsViewModel(schema);
            DataContext = ViewModel;
        }

        #endregion

        #region private methods

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            //show browse dir dialog
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //update to selected dir
                ViewModel.PathFile = dialog.SelectedPath;
            }

        }

        #endregion
    }
}
