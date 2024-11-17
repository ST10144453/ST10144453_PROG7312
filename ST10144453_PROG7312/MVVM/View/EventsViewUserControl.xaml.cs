using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class EventsViewUserControl : UserControl
    {
        private UserModel _currentUser;

        public EventsViewUserControl(UserModel user)
        {
            _currentUser = user;
            InitializeComponent();
            DataContext = new EventsViewModel(); // Set the DataContext
        }

        private void Event_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is EventModel eventModel)
            {
                var popup = new EventDetailsPopup(eventModel)
                {
                    Owner = Window.GetWindow(this)
                };
                popup.ShowDialog();
            }
        }

        private void NavigateToReportIssue_Click(object sender, RoutedEventArgs e)
        {
            // Navigation logic
        }

        private void NavigateToAllReports_Click(object sender, RoutedEventArgs e)
        {
            // Navigation logic
        }

        private void NavigateHome_Click(object sender, RoutedEventArgs e)
        {
            // Assuming MainContentControl is the container in the parent window or control
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var mainContentControl = parentWindow.FindName("MainContentControl") as ContentControl;
                if (mainContentControl != null)
                {
                    mainContentControl.Content = new HomeView(_currentUser);
                }
            }
        }
    }
}
