using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.ViewMode
{ 
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object _currentView;

        public MainWindowViewModel()
        {
            // Set the initial view to LoginRegisterMenu
            CurrentView = new LoginRegisterViewModel();
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowHomeViewCommand => new RelayCommand(ShowHomeView);

        private void ShowHomeView()
        {
            // Assuming 'user' is a valid variable or property
            var user = new UserModel(); // Replace 'User' with the actual user class

            CurrentView = new HomeViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}