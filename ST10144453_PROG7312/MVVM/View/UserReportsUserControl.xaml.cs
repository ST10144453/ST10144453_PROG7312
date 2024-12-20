﻿using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserReportsUserControl.xaml
    /// </summary>
    public partial class UserReportsUserControl : UserControl
    {
        private readonly UserModel _currentUser;

        public UserReportsUserControl(UserModel currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = new ReportViewModel(_currentUser);
        }
    }
}
