﻿using System;
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
    /// Interaction logic for SaveXML.xaml
    /// </summary>
    public partial class SaveXML : Window
    {
        #region Properties

        public SaveXmlViewModel ViewModel { get; private set; }

        #endregion

        #region Constructor

        public SaveXML(SchemaDescriber schema)
        {
            //init wpf members
            InitializeComponent();
            ViewModel = new SaveXmlViewModel(schema);
            DataContext = ViewModel;
        }

        #endregion

    }
}
