using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ReportViewModel : INotifyPropertyChanged
        {
            private string _issueName;
            private string _location;
            private string _selectedCategory;
            private string _description;
            private ObservableCollection<ReportModel> _reports;

            public string IssueName
            {
                get => _issueName;
                set
                {
                    if (_issueName != value)
                    {
                        _issueName = value;
                        OnPropertyChanged(nameof(IssueName));
                    }
                }
            }

            public string Location
            {
                get => _location;
                set
                {
                    if (_location != value)
                    {
                        _location = value;
                        OnPropertyChanged(nameof(Location));
                    }
                }
            }

            public string SelectedCategory
            {
                get => _selectedCategory;
                set
                {
                    if (_selectedCategory != value)
                    {
                        _selectedCategory = value;
                        OnPropertyChanged(nameof(SelectedCategory));
                    }
                }
            }

            public string Description
            {
                get => _description;
                set
                {
                    if (_description != value)
                    {
                        _description = value;
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            public ObservableCollection<ReportModel> Reports
            {
                get => _reports;
                set
                {
                    if (_reports != value)
                    {
                        _reports = value;
                        OnPropertyChanged(nameof(Reports));
                    }
                }
            }

            public ICommand AttachMediaCommand { get; private set; }
            public ICommand SubmitCommand { get; private set; }
            public ICommand NavigateToHomeCommand { get; private set; }

          

            private object _currentView;
            public object CurrentView
            {
                get => _currentView;
                set
                {
                    if (_currentView != value)
                    {
                        _currentView = value;
                        OnPropertyChanged(nameof(CurrentView));
                    }
                }
            }

            public ReportViewModel()
            {
                Reports = new ObservableCollection<ReportModel>();
                AttachMediaCommand = new RelayCommand(AttachMedia);
                SubmitCommand = new RelayCommand(Submit);

                // Commands for navigation
                NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            }

            private void NavigateToHome()
            {
                // Close the current ReportView window and open the MainWindow
                Window reportWindow = Application.Current.Windows.OfType<ReportView>().FirstOrDefault();
                reportWindow?.Close();

                Window mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                mainWindow?.Show();
            }


            private void AttachMedia()
        {
            // Handle media attachment logic
        }

        private void Submit()
        {
            var newReport = new ReportModel
            {
                reportName = IssueName,
                reportLocation = Location,
                reportDescription = Description,
                reportCategory = SelectedCategory,
                Media = new List<string>() // Add attached media paths here
            };

            Reports.Add(newReport);

            // Optionally clear the input fields
            IssueName = string.Empty;
            Location = string.Empty;
            Description = string.Empty;
            SelectedCategory = null;

            Console.WriteLine("User input has been saved.");

            // Output the saved user input
            Console.WriteLine("Saved Report:");
            Console.WriteLine($"Name: {newReport.reportName}");
            Console.WriteLine($"Location: {newReport.reportLocation}");
            Console.WriteLine($"Description: {newReport.reportDescription}");
            Console.WriteLine($"Category: {newReport.reportCategory}");
            Console.WriteLine($"Media: {string.Join(", ", newReport.Media)}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
