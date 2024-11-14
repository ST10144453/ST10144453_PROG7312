using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        private EventAVLTree _eventTree;
        private ObservableCollection<EventModel> _filteredEvents;
        private ObservableCollection<EventModel> _recommendedEvents;
        private Dictionary<string, HashSet<EventModel>> _eventsByTag;
        private HashSet<string> _uniqueCategories;
        private Stack<string> _searchHistory;
        private Dictionary<string, int> _searchFrequency;
        private Dictionary<int, double> _tagWeights;
        private ICommand _tagSelectedCommand;
        private ICommand _clearFiltersCommand;
        private ICommand _nextAnnouncementCommand;
        private ICommand _previousAnnouncementCommand;
        private ObservableCollection<AnnouncementModel> _announcements;
        private AnnouncementModel _currentAnnouncement;
        private int _currentAnnouncementIndex;
        private DispatcherTimer _announcementTimer;
        private string _searchQuery;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private List<TagsModel> _selectedTags;

        public ObservableCollection<TagsModel> Tags { get; private set; }

        public ObservableCollection<EventModel> FilteredEvents
        {
            get => _filteredEvents;
            set
            {
                _filteredEvents = value;
                OnPropertyChanged(nameof(FilteredEvents));
            }
        }

        public ObservableCollection<EventModel> RecommendedEvents
        {
            get => _recommendedEvents;
            set
            {
                _recommendedEvents = value;
                OnPropertyChanged(nameof(RecommendedEvents));
            }
        }

        public ICommand TagSelectedCommand => _tagSelectedCommand;
        public ICommand ClearFiltersCommand => _clearFiltersCommand;
        public ICommand NextAnnouncementCommand => _nextAnnouncementCommand;
        public ICommand PreviousAnnouncementCommand => _previousAnnouncementCommand;

        public ObservableCollection<AnnouncementModel> Announcements
        {
            get => _announcements;
            set
            {
                _announcements = value;
                OnPropertyChanged(nameof(Announcements));
            }
        }

        public AnnouncementModel CurrentAnnouncement
        {
            get => _currentAnnouncement;
            set
            {
                _currentAnnouncement = value;
                OnPropertyChanged(nameof(CurrentAnnouncement));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                ApplyFilters();
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                ApplyFilters();
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ApplyFilters();
            }
        }

        public EventsViewModel()
        {
            InitializeCollections();
            InitializeCommands();
            LoadEvents();
            InitializeAnnouncements();
        }

        private void InitializeCollections()
        {
            _eventTree = new EventAVLTree();
            Tags = new ObservableCollection<TagsModel>(TagsModel.Tags);
            _eventsByTag = new Dictionary<string, HashSet<EventModel>>();
            _uniqueCategories = new HashSet<string>();
            _searchHistory = new Stack<string>();
            _searchFrequency = new Dictionary<string, int>();
            _tagWeights = new Dictionary<int, double>();
            _selectedTags = new List<TagsModel>();
        }

        private void LoadEvents()
        {
            foreach (var eventModel in EventList.Instance.Events)
            {
                _eventTree.Insert(eventModel);

                // Index events by tags
                foreach (var tag in eventModel.eventTags)
                {
                    if (!_eventsByTag.ContainsKey(tag.TagName))
                    {
                        _eventsByTag[tag.TagName] = new HashSet<EventModel>();
                    }
                    _eventsByTag[tag.TagName].Add(eventModel);
                    _uniqueCategories.Add(tag.TagName);
                }
            }

            // Initialize filtered and recommended events
            FilteredEvents = new ObservableCollection<EventModel>(_eventTree.InOrderTraversal());
            UpdateRecommendedEvents();
        }

        private void UpdateRecommendedEvents()
        {
            var allEvents = _eventTree.InOrderTraversal();
            var recommendedList = allEvents
                .Where(e => e.eventDate >= DateTime.Now)
                .OrderBy(e => e.eventDate)
                .Take(5)
                .ToList();

            RecommendedEvents = new ObservableCollection<EventModel>(recommendedList);
        }

        private void ApplyFilters()
        {
            var allEvents = _eventTree.InOrderTraversal();
            IEnumerable<EventModel> filteredEvents = allEvents;

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filteredEvents = filteredEvents.Where(e =>
                    e.eventTitle.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventDescription.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventLocation.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Apply date range filter
            if (StartDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate.Date >= StartDate.Value.Date);
            }
            if (EndDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate.Date <= EndDate.Value.Date);
            }

            // Apply tag filter
            if (_selectedTags.Any())
            {
                filteredEvents = filteredEvents.Where(e =>
                    e.eventTags.Any(t => _selectedTags.Any(st => st.TagId == t.TagId)));
            }

            // Update filtered events
            FilteredEvents = new ObservableCollection<EventModel>(filteredEvents.OrderBy(e => e.eventDate));
        }

        public void FilterEventsByTag(TagsModel tag)
        {
            if (tag == null) return;

            if (tag.IsSelected)
            {
                if (!_selectedTags.Contains(tag))
                {
                    _selectedTags.Add(tag);
                }
            }
            else
            {
                _selectedTags.Remove(tag);
            }

            UpdateTagWeight(tag.TagId);
            ApplyFilters();
        }

        private void UpdateTagWeight(int tagId)
        {
            if (!_tagWeights.ContainsKey(tagId))
            {
                _tagWeights[tagId] = 1.0;
            }
            else
            {
                _tagWeights[tagId] += 0.5;
            }
        }

        public void SearchEvents(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                FilteredEvents = new ObservableCollection<EventModel>(_eventTree.InOrderTraversal());
                return;
            }

            _searchHistory.Push(searchTerm);
            UpdateSearchFrequency(searchTerm);

            var searchResults = _eventTree.InOrderTraversal()
                .Where(e =>
                    e.eventTitle.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventDescription.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventLocation.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(e => e.eventDate);

            FilteredEvents = new ObservableCollection<EventModel>(searchResults);
        }

        private void UpdateSearchFrequency(string searchTerm)
        {
            if (!_searchFrequency.ContainsKey(searchTerm))
            {
                _searchFrequency[searchTerm] = 1;
            }
            else
            {
                _searchFrequency[searchTerm]++;
            }
        }

        public void ClearFilters()
        {
            SearchQuery = null;
            StartDate = null;
            EndDate = null;
            foreach (var tag in Tags)
            {
                tag.IsSelected = false;
            }
            _selectedTags.Clear();
            ApplyFilters();
        }

        private void InitializeCommands()
        {
            _tagSelectedCommand = new RelayCommand<TagsModel>(FilterEventsByTag);
            _clearFiltersCommand = new RelayCommand(ClearFilters);
            _nextAnnouncementCommand = new RelayCommand(NextAnnouncement);
            _previousAnnouncementCommand = new RelayCommand(PreviousAnnouncement);
        }

        private void InitializeAnnouncements()
        {
            Announcements = new ObservableCollection<AnnouncementModel>(AnnouncementModel.Announcements);
            if (Announcements.Count > 0)
            {
                CurrentAnnouncement = Announcements[0];
            }

            _announcementTimer = new DispatcherTimer();
            _announcementTimer.Interval = TimeSpan.FromSeconds(3);
            _announcementTimer.Tick += AnnouncementTimer_Tick;
            _announcementTimer.Start();
        }

        private void AnnouncementTimer_Tick(object sender, EventArgs e)
        {
            NextAnnouncement();
        }

        private void NextAnnouncement()
        {
            if (Announcements.Count == 0) return;
            _currentAnnouncementIndex = (_currentAnnouncementIndex + 1) % Announcements.Count;
            CurrentAnnouncement = Announcements[_currentAnnouncementIndex];
        }

        private void PreviousAnnouncement()
        {
            if (Announcements.Count == 0) return;
            _currentAnnouncementIndex = (_currentAnnouncementIndex - 1 + Announcements.Count) % Announcements.Count;
            CurrentAnnouncement = Announcements[_currentAnnouncementIndex];
        }

        private string GetAnnouncementIcon(List<TagsModel> tags)
        {
            // Map tags to appropriate icons
            var primaryTag = tags.FirstOrDefault();
            string iconPath = "pack://application:,,,/Resources/Icons/default.png";

            if (primaryTag != null)
            {
                switch (primaryTag.TagId)
                {
                    case 1:
                        iconPath = "pack://application:,,,/Resources/Icons/cultural.png";
                        break;
                    case 2:
                        iconPath = "pack://application:,,,/Resources/Icons/sports.png";
                        break;
                    case 3:
                        iconPath = "pack://application:,,,/Resources/Icons/community.png";
                        break;
                    case 4:
                        iconPath = "pack://application:,,,/Resources/Icons/health.png";
                        break;
                    case 5:
                        iconPath = "pack://application:,,,/Resources/Icons/environment.png";
                        break;
                    case 6:
                        iconPath = "pack://application:,,,/Resources/Icons/safety.png";
                        break;
                    case 7:
                        iconPath = "pack://application:,,,/Resources/Icons/education.png";
                        break;
                    case 8:
                        iconPath = "pack://application:,,,/Resources/Icons/art.png";
                        break;
                    case 9:
                        iconPath = "pack://application:,,,/Resources/Icons/market.png";
                        break;
                    case 10:
                        iconPath = "pack://application:,,,/Resources/Icons/job.png";
                        break;
                    case 11:
                        iconPath = "pack://application:,,,/Resources/Icons/historical.png";
                        break;
                    case 12:
                        iconPath = "pack://application:,,,/Resources/Icons/family.png";
                        break;
                    case 13:
                        iconPath = "pack://application:,,,/Resources/Icons/social.png";
                        break;
                    case 14:
                        iconPath = "pack://application:,,,/Resources/Icons/disaster.png";
                        break;
                    case 15:
                        iconPath = "pack://application:,,,/Resources/Icons/consultation.png";
                        break;
                    case 16:
                        iconPath = "pack://application:,,,/Resources/Icons/traffic.png";
                        break;
                    case 17:
                        iconPath = "pack://application:,,,/Resources/Icons/youth.png";
                        break;
                    case 18:
                        iconPath = "pack://application:,,,/Resources/Icons/meeting.png";
                        break;
                    case 19:
                        iconPath = "pack://application:,,,/Resources/Icons/participation.png";
                        break;
                    case 20:
                        iconPath = "pack://application:,,,/Resources/Icons/volunteer.png";
                        break;
                    default:
                        iconPath = "pack://application:,,,/Resources/Icons/default.png";
                        break;
                }
            }

            return iconPath;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
