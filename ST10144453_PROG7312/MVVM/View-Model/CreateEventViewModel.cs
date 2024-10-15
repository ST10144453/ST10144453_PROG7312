using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class CreateEventViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _location;
        private string _description;
        private DateTime? _date;
        private ObservableCollection<TagsModel> _tags;
        private List<string> _photos;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public ObservableCollection<TagsModel> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }

        public ICommand CreateEventCommand { get; }
        public ICommand AddPhotosCommand { get; }

        private ObservableCollection<EventModel> _events;
        public ObservableCollection<EventModel> Events => EventList.Instance.Events;

        public CreateEventViewModel()
        {
            Tags = new ObservableCollection<TagsModel>(TagsModel.Tags);
            _photos = new List<string>();
            CreateEventCommand = new RelayCommand(CreateEvent);
            AddPhotosCommand = new RelayCommand(AddPhotos);

            Console.WriteLine("CreateEventViewModel initialized");
            Console.WriteLine($"Initial Events Count: {Events.Count}");
        }

        private void CreateEvent()
        {
            var newEvent = new EventModel
            {
                eventTitle = Title,
                eventLocation = Location,
                eventDescription = Description,
                eventDate = Date ?? DateTime.MinValue,
                eventTags = GetSelectedTags(),
                eventPhotos = _photos.Any() ? _photos : new List<string> { "defaultPhoto.jpg" }
            };

            SaveEvent(newEvent);

            ResetFields();
        }

        private List<TagsModel> GetSelectedTags()
        {
            return Tags.Where(tag => tag.IsSelected).ToList();
        }

        private void SaveEvent(EventModel newEvent)
        {
            if (EventList.Instance.Events == null)
            {
                throw new InvalidOperationException("EventList.Events is not initialized.");
            }

            EventList.Instance.Events.Add(newEvent);
            OnPropertyChanged(nameof(Events));

            Console.WriteLine("Event created successfully");
            Console.WriteLine($"Event Title: {newEvent.eventTitle}");
            Console.WriteLine($"Event Location: {newEvent.eventLocation}");
            Console.WriteLine($"Event Description: {newEvent.eventDescription}");
            Console.WriteLine($"Event Date: {newEvent.eventDate.ToShortDateString()}");

            for (int i = 0; i < newEvent.eventTags.Count; i++)
            {
                Console.WriteLine($"Event Tag {i + 1}: {newEvent.eventTags[i].TagName}");
            }

            for (int i = 0; i < newEvent.eventPhotos.Count; i++)
            {
                Console.WriteLine($"Event Photo {i + 1}: {newEvent.eventPhotos[i]}");
            }

            Console.WriteLine($"Total Events Count: {Events.Count}");
            for (int i = 0; i < Events.Count; i++)
            {
                Console.WriteLine($"Event {i + 1}: {Events[i].eventTitle}");
            }
        }

        private void ResetFields()
        {
            Title = string.Empty;
            Location = string.Empty;
            Description = string.Empty;
            Date = DateTime.Now;
            _photos.Clear();

            foreach (var tag in Tags)
            {
                tag.IsSelected = false;
            }
        }

        private void AddPhotos()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    byte[] imageBytes = File.ReadAllBytes(filename);
                    string base64String = Convert.ToBase64String(imageBytes);
                    _photos.Add(base64String);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
