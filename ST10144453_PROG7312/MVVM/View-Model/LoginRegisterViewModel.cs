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
        private string _usernameError;
        private string _emailError;
        private string _passwordError;
        private string _confirmPasswordError;
        private string _loginError;

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
                if (_profilePhoto != value)
                {
                    _profilePhoto = value;
                    OnPropertyChanged(nameof(ProfilePhoto));
                }
            }
        }

        public string UsernameError
        {
            get => _usernameError;
            set
            {
                _usernameError = value;
                OnPropertyChanged();
            }
        }

        public string EmailError
        {
            get => _emailError;
            set
            {
                _emailError = value;
                OnPropertyChanged();
            }
        }

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set
            {
                _confirmPasswordError = value;
                OnPropertyChanged();
            }
        }

        public string LoginError
        {
            get => _loginError;
            set
            {
                _loginError = value;
                OnPropertyChanged();
            }
        }

        // Use a static collection to ensure it's shared across instances
        public static ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>(UserModel.Users);

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        // Add validation requirement properties
        public string UsernameRequirements => "Username must be at least 3 characters long";
        public string PasswordRequirements => "Password must be at least 8 characters long and contain at least one uppercase letter, one number, and one special character";
        public string EmailRequirements => "Please enter a valid email address";
        
        private bool _showUsernameError;
        private bool _showEmailError;
        private bool _showPasswordError;
        private bool _showConfirmPasswordError;
        private bool _showLoginError;

        public bool ShowUsernameError
        {
            get => _showUsernameError;
            set
            {
                _showUsernameError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowEmailError
        {
            get => _showEmailError;
            set
            {
                _showEmailError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowPasswordError
        {
            get => _showPasswordError;
            set
            {
                _showPasswordError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowConfirmPasswordError
        {
            get => _showConfirmPasswordError;
            set
            {
                _showConfirmPasswordError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLoginError
        {
            get => _showLoginError;
            set
            {
                _showLoginError = value;
                OnPropertyChanged();
            }
        }

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

        private bool ValidatePassword(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsDigit) &&
                   password.Any(c => !char.IsLetterOrDigit(c));
        }

        private bool ValidateRegistration()
        {
            bool isValid = true;
            
            // Username validation
            if (string.IsNullOrEmpty(UserName))
            {
                UsernameError = "Username is required";
                ShowUsernameError = true;
                isValid = false;
            }
            else if (UserName.Length < 3)
            {
                UsernameError = "Username must be at least 3 characters";
                ShowUsernameError = true;
                isValid = false;
            }
            else
            {
                ShowUsernameError = false;
            }

            // Email validation
            if (string.IsNullOrEmpty(Email))
            {
                EmailError = "Email is required";
                ShowEmailError = true;
                isValid = false;
            }
            else if (!Email.Contains("@") || !Email.Contains("."))
            {
                EmailError = "Please enter a valid email address";
                ShowEmailError = true;
                isValid = false;
            }
            else
            {
                ShowEmailError = false;
            }

            // Password validation
            if (string.IsNullOrEmpty(Password))
            {
                PasswordError = "Password is required";
                ShowPasswordError = true;
                isValid = false;
            }
            else if (!ValidatePassword(Password))
            {
                PasswordError = "Password does not meet requirements";
                ShowPasswordError = true;
                isValid = false;
            }
            else
            {
                ShowPasswordError = false;
            }

            // Confirm password validation
            if (Password != ConfirmPassword)
            {
                ConfirmPasswordError = "Passwords do not match";
                ShowConfirmPasswordError = true;
                isValid = false;
            }
            else
            {
                ShowConfirmPasswordError = false;
            }

            return isValid;
        }

        private void Register(Action onSuccess)
        {
            if (!ValidateRegistration())
            {
                return;
            }

            // Check if username already exists
            if (Users.Any(u => u.userName.Equals(UserName, StringComparison.OrdinalIgnoreCase)))
            {
                UsernameError = "Username already exists";
                return;
            }

            // Check if email already exists
            if (Users.Any(u => u.email.Equals(Email, StringComparison.OrdinalIgnoreCase)))
            {
                EmailError = "Email already exists";
                return;
            }

            var newUser = new GuestUserModel
            {
                userName = UserName,
                email = Email,
                password = Password,
                profilePhoto = ConvertImageToBase64(ProfilePhoto),
                isStaff = false
            };

            Users.Add(newUser);
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            onSuccess?.Invoke();
        }

        private void Login()
        {
            ShowLoginError = false;
            
            // Validate empty fields
            if (string.IsNullOrWhiteSpace(LoginUserNameOrEmail))
            {
                LoginError = "Please enter your username or email";
                ShowLoginError = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(LoginPassword))
            {
                LoginError = "Please enter your password";
                ShowLoginError = true;
                return;
            }

            // Check if user exists
            var userByUsername = Users.FirstOrDefault(u => u.userName.Equals(LoginUserNameOrEmail, StringComparison.OrdinalIgnoreCase));
            var userByEmail = Users.FirstOrDefault(u => u.email.Equals(LoginUserNameOrEmail, StringComparison.OrdinalIgnoreCase));
            
            if (userByUsername == null && userByEmail == null)
            {
                LoginError = "Account not found. Please check your username/email or register a new account";
                ShowLoginError = true;
                return;
            }

            // Check password
            var user = userByUsername ?? userByEmail;
            if (user.password != LoginPassword)
            {
                LoginError = "Incorrect password";
                ShowLoginError = true;
                return;
            }

            // Successful login
            LoginError = null;
            ShowLoginError = false;
            UserSession.CurrentUser = user;

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (user.isStaff)
            {
                var staffMenu = new StaffMenu(user)
                {
                    DataContext = new StaffMenuViewModel(user)
                };
                mainWindow.Navigate(staffMenu);
            }
            else
            {
                var homeView = new HomeView(user);
                mainWindow.Navigate(homeView);
            }
        }



        private string ConvertImageToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return null;

            try
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64String = Convert.ToBase64String(imageBytes);
                
                // Validate image size (e.g., 5MB limit)
                if (imageBytes.Length > 5 * 1024 * 1024)
                {
                    throw new Exception("Image size must be less than 5MB");
                }
                
                return base64String;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
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
