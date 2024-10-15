using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System;
using System.Linq;
using System.Windows;
using System.IO;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class LoginRegisterViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _profilePhoto;
        private string _loginUserNameOrEmail;
        private string _loginPassword;

        public string LoginUserNameOrEmail
        {
            get => _loginUserNameOrEmail;
            set
            {
                _loginUserNameOrEmail = value;
                OnPropertyChanged();
            }
        }

        public string LoginPassword
        {
            get => _loginPassword;
            set
            {
                _loginPassword = value;
                OnPropertyChanged();
            }
        }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePhoto
        {
            get => _profilePhoto;
            set
            {
                _profilePhoto = value;
                OnPropertyChanged();
            }
        }

        // Use a static collection to ensure it's shared across instances
        public static ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>(UserModel.Users);

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginRegisterViewModel()
        {
            RegisterCommand = new RelayCommand(() => Register(OnRegistrationSuccessful), CanRegister);
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private bool CanRegister()
        {
            return !string.IsNullOrEmpty(UserName) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password) &&
                   Password == ConfirmPassword;
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(LoginUserNameOrEmail) && !string.IsNullOrEmpty(LoginPassword);
        }

        private void Register(Action onSuccess)
        {
            Console.WriteLine("Register method called");

            var newUser = new GuestUserModel
            {
                userName = UserName,
                email = Email,
                password = Password,
                profilePhoto = ConvertImageToBase64(ProfilePhoto),
                isStaff = false
            };

            Users.Add(newUser);
            Console.WriteLine("User registered");

            foreach (var user in Users)
            {
                Console.WriteLine($"Username: {user.userName}");
                Console.WriteLine($"Email: {user.email}");
                Console.WriteLine($"Password: {user.password}");
                Console.WriteLine($"Profile Photo: {user.profilePhoto}");
                Console.WriteLine($"Is Staff: {user.isStaff}");
            }

            // Invoke the success callback
            onSuccess?.Invoke();
        }

        private void Login()
        {
            Console.WriteLine($"Attempting login with Username/Email: {LoginUserNameOrEmail}, Password: {LoginPassword}");

            foreach (var registeredUser in Users)
            {
                Console.WriteLine($"Stored User - Username: {registeredUser.userName}, Email: {registeredUser.email}, Password: {registeredUser.password}, ID: {registeredUser.userID}");
            }

            var user = Users.FirstOrDefault(u =>
                (u.userName == LoginUserNameOrEmail || u.email == LoginUserNameOrEmail) &&
                u.password == LoginPassword);

            if (user != null)
            {
                Console.WriteLine("Login successful!");
                UserSession.CurrentUser = user; // Set the current user

                var mainWindow = (MainWindow)Application.Current.MainWindow;
                if (user.isStaff)
                {
                    var staffMenu = new StaffMenu(user)
                    {
                        DataContext = new StaffMenuViewModel(user)
                    };
                    mainWindow.Navigate(staffMenu);
                    Console.WriteLine("Staff Login");
                }
                else
                {
                    var homeView = new HomeView(user);
                    mainWindow.Navigate(homeView);
                    Console.WriteLine("User Login");
                }
            }
            else
            {
                Console.WriteLine("Invalid username/email or password.");
            }
        }



        private string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return null;

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private void OnRegistrationSuccessful()
        {
            // Logic to execute upon successful registration
            Console.WriteLine("Registration successful!");
            // Navigate to LoginUserControl
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.Navigate(new LoginUserControl());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
