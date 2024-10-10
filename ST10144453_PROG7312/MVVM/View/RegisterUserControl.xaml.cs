using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class RegisterUserControl : UserControl
    {
        public RegisterUserControl()
        {
            InitializeComponent();
        }

        private void ProfilePhotoBorder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    SetProfilePhoto(files[0]);
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
            ProfilePhoto.Source = bitmap;
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (TogglePasswordVisibility.IsChecked == true)
            {
                PlainTextPasswordBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PlainTextPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = PlainTextPasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
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


        private void Register_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as MainWindow;
            if (parent != null)
            {
                parent.MainContentControl.Content = new LoginRegisterMenu();
            }
        }

        private void UploadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SetProfilePhoto(openFileDialog.FileName);
            }
        }
    }
}
