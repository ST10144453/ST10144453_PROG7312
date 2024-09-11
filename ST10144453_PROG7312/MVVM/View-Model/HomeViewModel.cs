using ST10144453_PROG7312.MVVM.View;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ST10144453_PROG7312.Core;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ShowUnderDevelopmentPopupCommand { get; private set; }

        public HomeViewModel()
        {
            // Initialize the command in the constructor
            ShowUnderDevelopmentPopupCommand = new RelayCommand(ShowUnderDevelopmentPopup);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ShowUnderDevelopmentPopup()
        {
            var popup = new UnderDevelopmentPopup
            {
                Owner = Application.Current.MainWindow
            };
            popup.ShowDialog();
        }
    }
}
