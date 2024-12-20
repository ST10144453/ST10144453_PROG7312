using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
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
        private TagsTree _tagsTree;
        private AnnouncementTree _announcementTree;

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
            // Initialize all collections and objects first
            _eventTree = new EventAVLTree();
            _eventsByTag = new Dictionary<string, HashSet<EventModel>>();
            _uniqueCategories = new HashSet<string>();
            _searchHistory = new Stack<string>();
            _searchFrequency = new Dictionary<string, int>();
            _tagWeights = new Dictionary<int, double>();
            _selectedTags = new List<TagsModel>();
            
            // Initialize Tags collection
            Tags = new ObservableCollection<TagsModel>(TagsModel.Tags);
            
            // Initialize trees
            _tagsTree = new TagsTree();
            _announcementTree = new AnnouncementTree();
            
            // Initialize tag hierarchy
            InitializeTagsHierarchy();
            
            // Load events
            LoadEvents();
            
            // Initialize announcements
            InitializeAnnouncements();
            
            // Initialize commands
            InitializeCommands();
        }

        private void InitializeTagsHierarchy()
        {
            // Define tag relationships
            var tagRelationships = new Dictionary<int, int?>
            {
                // Parent-child relationships for tags
                // TagId -> ParentTagId (null for root level tags)
                { 1, null },  // Cultural Festivals
                { 2, null },  // Sports Events
                { 3, null },  // Health and Wellness
                { 4, 3 },     // Environmental Initiatives under Health
                { 5, null },  // Public Safety
                { 6, null },  // Education and Workshops
                // Add more relationships as needed
            };

            foreach (var tag in Tags)
            {
                _tagsTree.AddTag(tag, tagRelationships.ContainsKey(tag.TagId) ? tagRelationships[tag.TagId] : null);
            }
        }

        private void LoadEvents()
        {
            if (_eventTree == null || _eventsByTag == null)
            {
                throw new InvalidOperationException("Event collections not properly initialized");
            }

            foreach (var eventModel in EventList.Instance.Events)
            {
                if (eventModel != null)
                {
                    _eventTree.Insert(eventModel);

                    if (eventModel.eventTags != null)
                    {
                        foreach (var tag in eventModel.eventTags)
                        {
                            if (tag != null && !string.IsNullOrEmpty(tag.TagName))
                            {
                                if (!_eventsByTag.ContainsKey(tag.TagName))
                                {
                                    _eventsByTag[tag.TagName] = new HashSet<EventModel>();
                                }
                                _eventsByTag[tag.TagName].Add(eventModel);
                                _uniqueCategories.Add(tag.TagName);
                            }
                        }
                    }
                }
            }

            FilteredEvents = new ObservableCollection<EventModel>(_eventTree.InOrderTraversal());
            UpdateRecommendedEvents();
        }

        private void UpdateRecommendedEvents()
        {
            var allEvents = _eventTree.InOrderTraversal();
            var userPreferences = CalculateUserPreferences();

            // First filter by date range
            var filteredEvents = allEvents.Where(e => e.eventDate >= DateTime.Now);
            if (StartDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate <= EndDate.Value);
            }

            // Apply search query filter if exists
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filteredEvents = filteredEvents.Where(e =>
                    e.eventTitle.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventDescription.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.eventLocation.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Calculate scores for filtered events
            var recommendedEvents = filteredEvents
                .Select(e => new
                {
                    Event = e,
                    Score = CalculateRecommendationScore(e, userPreferences)
                })
                .OrderByDescending(x => x.Score)
                .Take(4)
                .Select(x => x.Event)
                .ToList();

            RecommendedEvents = new ObservableCollection<EventModel>(recommendedEvents);
        }

        private Dictionary<int, double> CalculateUserPreferences()
        {
            var preferences = new Dictionary<int, double>();

            // Consider tag selection frequency
            foreach (var weight in _tagWeights)
            {
                preferences[weight.Key] = weight.Value;
            }

            // Consider search history
            foreach (var search in _searchFrequency)
            {
                var relatedTags = Tags.Where(t =>
                t.TagName.IndexOf(search.Key, StringComparison.OrdinalIgnoreCase) >= 0);

                foreach (var tag in relatedTags)
                {
                    if (!preferences.ContainsKey(tag.TagId))
                        preferences[tag.TagId] = 0;

                    preferences[tag.TagId] += search.Value * 0.5;
                }
            }

            // Consider related tags from tag tree
            foreach (var tagId in preferences.Keys.ToList())
            {
                var relatedTags = _tagsTree.GetRelatedTags(tagId);
                foreach (var relatedTag in relatedTags)
                {
                    if (!preferences.ContainsKey(relatedTag.TagId))
                        preferences[relatedTag.TagId] = 0;

                    preferences[relatedTag.TagId] += preferences[tagId] * 0.3;
                }
            }

            return preferences;
        }

        private double CalculateRecommendationScore(EventModel eventModel, Dictionary<int, double> preferences)
        {
            double score = 0;

            // Tag preference score
            foreach (var tag in eventModel.eventTags)
            {
                if (preferences.ContainsKey(tag.TagId))
                    score += preferences[tag.TagId];
            }

            // Date proximity score
            var daysUntilEvent = (eventModel.eventDate - DateTime.Now).TotalDays;
            score += Math.Max(0, 30 - daysUntilEvent) / 30 * 5;

            // Search relevance score
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchTerms = SearchQuery.ToLower().Split(' ');
                foreach (var term in searchTerms)
                {
                    if (eventModel.eventTitle.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 3;
                    if (eventModel.eventDescription.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 2;
                    if (eventModel.eventLocation.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 1;
                }
            }

            // Related events score
            var closestEvent = _eventTree.FindClosestEvent(eventModel.eventDate);
            if (closestEvent != null && closestEvent != eventModel)
            {
                var sharedTags = eventModel.eventTags.Intersect(closestEvent.eventTags).Count();
                score += sharedTags * 0.5;
            }

            return score;
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
            UpdateRecommendedEvents();
        }

        public void FilterEventsByTag(TagsModel tag)
        {
            if (tag == null) return;

            if (tag.IsSelected)
            {
                if (!_selectedTags.Contains(tag))
                {
                    _selectedTags.Add(tag);
                    // Add related tags with a depth of 1
                    var relatedTags = _tagsTree.GetRelatedTags(tag.TagId, 1);
                    foreach (var relatedTag in relatedTags)
                    {
                        if (!_selectedTags.Contains(relatedTag))
                        {
                            _selectedTags.Add(relatedTag);
                            relatedTag.IsSelected = true;
                        }
                    }
                }
            }
            else
            {
                _selectedTags.Remove(tag);
                // Remove related tags
                var relatedTags = _tagsTree.GetRelatedTags(tag.TagId, 1);
                foreach (var relatedTag in relatedTags)
                {
                    _selectedTags.Remove(relatedTag);
                    relatedTag.IsSelected = false;
                }
            }

            UpdateTagWeight(tag.TagId);
            ApplyFilters();
            UpdateRecommendedEvents();
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
            UpdateRecommendedEvents();
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
            UpdateRecommendedEvents();
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
            foreach (var announcement in AnnouncementModel.Announcements)
            {
                if (announcement.relatedEvent?.eventTags != null && announcement.relatedEvent.eventTags.Any())
                {
                    var iconPath = GetAnnouncementIcon(announcement.relatedEvent.eventTags);
                    Debug.WriteLine($"Setting icon path for announcement {announcement.announcementId}: {iconPath}");
                    announcement.announcementIcon = iconPath;
                }
                else
                {
                    Debug.WriteLine($"Setting default icon for announcement {announcement.announcementId}");
                    announcement.announcementIcon = "pack://application:,,,/Resources/Icons/default.png";
                }
                _announcementTree.Insert(announcement);
            }

            var orderedAnnouncements = _announcementTree.GetAnnouncementsInOrder();
            Announcements = new ObservableCollection<AnnouncementModel>(orderedAnnouncements);

            if (Announcements.Count > 0)
            {
                CurrentAnnouncement = Announcements[0];
                Debug.WriteLine($"Current announcement icon path: {CurrentAnnouncement.announcementIcon}");
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
            if (tags == null || !tags.Any())
            {
                return "pack://application:,,,/Resources/Icons/default.png";
            }

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