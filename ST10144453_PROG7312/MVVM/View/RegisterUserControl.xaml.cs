using Microsoft.Win32;
using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class RegisterUserControl : UserControl
    { 
        private LoginRegisterViewModel ViewModel => DataContext as LoginRegisterViewModel;
        public RegisterUserControl()
        {
            InitializeComponent();
            DataContext = new LoginRegisterViewModel();
            
        }

        private void OnRegistrationSuccessful()
        {
            // Navigate to the login user control
            var parent = this.Parent as Panel;
            if (parent != null)
            {
                parent.Children.Clear();
                parent.Children.Add(new LoginUserControl());
            }
        }

        private void ProfilePhotoBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string filePath = files[0];
                    if (ValidateImageFile(filePath))
                    {
                        try
                        {
                            // Convert the image to a BitmapImage
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(filePath);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();

                            // Update the UI
                            ProfilePhotoImage.ImageSource = bitmap;
                            
                            // Hide the upload instructions
                            UploadInstructionsPanel.Visibility = Visibility.Collapsed;

                            // Update the ViewModel
                            ((LoginRegisterViewModel)DataContext).ProfilePhoto = filePath;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void ProfilePhotoBorder_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void SetProfilePhoto(string filePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filePath);
            bitmap.EndInit();
            //ProfilePhoto.Source = bitmap;
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (TogglePasswordVisibility.IsChecked == true)
            {
                PlainTextPasswordBox.Text = PasswordBoxInput.Password;
                PasswordBoxInput.Visibility = Visibility.Collapsed;
                PlainTextPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBoxInput.Password = PlainTextPasswordBox.Text;
                PasswordBoxInput.Visibility = Visibility.Visible;
                PlainTextPasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleConfirmPasswordVisibility.IsChecked == true)
            {
                PlainTextConfirmPasswordBox.Text = ConfirmPasswordBox.Password;
                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
                PlainTextConfirmPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                ConfirmPasswordBox.Password = PlainTextConfirmPasswordBox.Text;
                ConfirmPasswordBox.Visibility = Visibility.Visible;
                PlainTextConfirmPasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var viewModel = DataContext as LoginRegisterViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var viewModel = DataContext as LoginRegisterViewModel;
                if (viewModel != null)
                {
                    viewModel.ConfirmPassword = passwordBox.Password;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as MainWindow;
            if (parent != null)
            {
                parent.MainContentControl.Content = new LoginRegisterMenu();
            }
        }

        private void UploadImageButton_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (ValidateImageFile(filePath))
                {
                    try
                    {
                        // Convert the image to a BitmapImage
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(filePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        // Update the UI
                        ProfilePhotoImage.ImageSource = bitmap;
                        
                        // Hide the upload instructions
                        UploadInstructionsPanel.Visibility = Visibility.Collapsed;

                        // Update the ViewModel
                        ((LoginRegisterViewModel)DataContext).ProfilePhoto = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool ValidateImageFile(string filePath)
        {
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();
                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(extension))
                {
                    MessageBox.Show("Please select a valid image file (JPG, JPEG, or PNG)", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length > 5 * 1024 * 1024) // 5MB limit
                {
                    MessageBox.Show("Image size must be less than 5MB", "File Too Large", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
