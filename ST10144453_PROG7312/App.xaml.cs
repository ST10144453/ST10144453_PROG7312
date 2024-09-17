using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ST10144453_PROG7312
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ReportViewModel SharedReportViewModel { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SharedReportViewModel = new ReportViewModel();
        }
    }
}
