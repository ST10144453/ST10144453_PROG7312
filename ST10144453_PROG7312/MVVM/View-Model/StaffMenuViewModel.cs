using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class StaffMenuViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _email;
        private string _profilePhoto;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
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

        public string ProfilePhoto
        {
            get => _profilePhoto;
            set
            {
                _profilePhoto = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Parameterless constructor
        public StaffMenuViewModel()
        {
        }

        // Constructor to initialize with user data
        public StaffMenuViewModel(UserModel user)
        {
            Username = user.userName;
            Email = user.email;
            ProfilePhoto = user.profilePhoto;
        }
    }

}
