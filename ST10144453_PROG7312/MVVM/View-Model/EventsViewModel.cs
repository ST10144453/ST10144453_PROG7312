using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class EventsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TagsModel> _tags;
        public ObservableCollection<TagsModel> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }

        private SortedDictionary<DateTime, List<EventModel>> _events;
        public SortedDictionary<DateTime, List<EventModel>> Events
        {
            get { return _events; }
            set
            {
                _events = value;
                OnPropertyChanged(nameof(Events));
            }
        }

        private ObservableCollection<EventModel> _filteredEvents;
        public ObservableCollection<EventModel> FilteredEvents
        {
            get { return _filteredEvents; }
            set
            {
                _filteredEvents = value;
                OnPropertyChanged(nameof(FilteredEvents));
            }
        }

        private ObservableCollection<EventModel> _recommendedEvents;
        public ObservableCollection<EventModel> RecommendedEvents
        {
            get { return _recommendedEvents; }
            set
            {
                _recommendedEvents = value;
                OnPropertyChanged(nameof(RecommendedEvents));
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                var selectedTagIds = Tags.Where(t => t.IsSelected).Select(t => t.TagId).ToList();
                FilterEvents(selectedTagIds);
            }
        }

        public ICommand TagSelectedCommand { get; }

        private PriorityQueueManager priorityQueueManager;
        private HashSet<DateTime> uniqueDates;
        private HashSet<string> uniqueCategories;
        private List<int> _previouslySelectedTagIds;

        private List<string> _previousSearches;
        private List<TagsModel> _previouslySelectedTags;

        private Dictionary<int, List<EventModel>> eventsByTag;

        private ObservableCollection<AnnouncementModel> _announcements;
        public ObservableCollection<AnnouncementModel> Announcements
        {
            get { return _announcements; }
            set
            {
                _announcements = value;
                OnPropertyChanged(nameof(Announcements));
            }
        }

        private AnnouncementModel _currentAnnouncement;
        public AnnouncementModel CurrentAnnouncement
        {
            get { return _currentAnnouncement; }
            set
            {
                _currentAnnouncement = value;
                OnPropertyChanged(nameof(CurrentAnnouncement));
            }
        }

        private DispatcherTimer _announcementTimer;
        private int _currentAnnouncementIndex;

        private DateTime? _startDate;
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

        private DateTime? _endDate;
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

        public ICommand ClearFiltersCommand { get; }

        private readonly Stack<string> _searchHistory;
        private readonly Queue<FilterOperation> _filterOperations;
        private readonly Dictionary<string, int> _searchFrequency;
        private readonly Dictionary<int, int> _tagSelectionFrequency;
        private readonly Dictionary<string, HashSet<EventModel>> _eventsByKeyword = new Dictionary<string, HashSet<EventModel>>();
        private readonly Dictionary<int, double> _tagWeights;

        private class FilterOperation
        {
            public string SearchQuery { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public List<int> SelectedTagIds { get; set; }
        }

        public ICommand NextAnnouncementCommand { get; }
        public ICommand PreviousAnnouncementCommand { get; }

        public EventsViewModel()
        {
            Tags = new ObservableCollection<TagsModel>(TagsModel.Tags);
            Events = new SortedDictionary<DateTime, List<EventModel>>();
            uniqueDates = new HashSet<DateTime>();
            uniqueCategories = new HashSet<string>();

            foreach (var eventModel in EventList.Instance.Events)
            {
                if (!Events.ContainsKey(eventModel.eventDate))
                {
                    Events[eventModel.eventDate] = new List<EventModel>();
                }
                Events[eventModel.eventDate].Add(eventModel);
                uniqueDates.Add(eventModel.eventDate);

                foreach (var tag in eventModel.eventTags)
                {
                    uniqueCategories.Add(tag.TagName);
                }
            }
            _previouslySelectedTagIds = new List<int>();

            InitializeEventsByTag();
            FilteredEvents = new ObservableCollection<EventModel>(GetAllEvents());
            RecommendedEvents = new ObservableCollection<EventModel>(GetRecommendedEvents());

            TagSelectedCommand = new RelayCommand<TagsModel>(OnTagSelected);
            priorityQueueManager = new PriorityQueueManager(EventList.Instance.Events);

            _previousSearches = new List<string>();
            _previouslySelectedTags = new List<TagsModel>();

            Announcements = new ObservableCollection<AnnouncementModel>(AnnouncementModel.Announcements);
            if (Announcements.Count > 0)
            {
                CurrentAnnouncement = Announcements[0];
            }

            _announcementTimer = new DispatcherTimer();
            _announcementTimer.Interval = TimeSpan.FromSeconds(3);
            _announcementTimer.Tick += AnnouncementTimer_Tick;
            _announcementTimer.Start();

            ClearFiltersCommand = new RelayCommand(ClearFilters);
            _searchHistory = new Stack<string>();
            _filterOperations = new Queue<FilterOperation>();
            _searchFrequency = new Dictionary<string, int>();
            _tagSelectionFrequency = new Dictionary<int, int>();
            _eventsByKeyword = new Dictionary<string, HashSet<EventModel>>();
            _tagWeights = new Dictionary<int, double>();

            InitializeEventIndexes();

            NextAnnouncementCommand = new RelayCommand(NextAnnouncement);
            PreviousAnnouncementCommand = new RelayCommand(PreviousAnnouncement);

            InitializeAnnouncements();
        }

        private void AnnouncementTimer_Tick(object sender, EventArgs e)
        {
            if (Announcements.Count == 0) return;

            _currentAnnouncementIndex = (_currentAnnouncementIndex + 1) % Announcements.Count;
            CurrentAnnouncement = Announcements[_currentAnnouncementIndex];
        }

        private void InitializeEventsByTag()
        {
            eventsByTag = new Dictionary<int, List<EventModel>>();

            foreach (var eventModel in GetAllEvents())
            {
                foreach (var tag in eventModel.eventTags)
                {
                    if (!eventsByTag.ContainsKey(tag.TagId))
                    {
                        eventsByTag[tag.TagId] = new List<EventModel>();
                    }
                    eventsByTag[tag.TagId].Add(eventModel);
                }
            }
        }

        private void OnTagSelected(TagsModel tag)
        {
            Console.WriteLine($"Tag selected: {tag.TagName}, IsSelected: {tag.IsSelected}");

            // Update the list of previously selected tag IDs
            if (tag.IsSelected)
            {
                if (!_previouslySelectedTagIds.Contains(tag.TagId))
                {
                    _previouslySelectedTagIds.Add(tag.TagId);
                }
            }
            else
            {
                _previouslySelectedTagIds.Remove(tag.TagId);
            }

            FilterEvents(Tags.Where(t => t.IsSelected).Select(t => t.TagId).ToList());
        }

        private void FilterEvents(List<int> selectedTagIds)
        {
            var filteredEvents = GetAllEvents();
            var resultSet = new HashSet<EventModel>();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchWords = SearchQuery.ToLower().Split(' ');
                foreach (var word in searchWords)
                {
                    UpdateSearchFrequency(word);
                    if (_eventsByKeyword.ContainsKey(word))
                    {
                        if (resultSet.Count == 0)
                        {
                            resultSet.UnionWith(_eventsByKeyword[word]);
                        }
                        else
                        {
                            resultSet.IntersectWith(_eventsByKeyword[word]);
                        }
                    }
                }
                filteredEvents = resultSet.Count > 0 ? resultSet : filteredEvents;
            }

            // Apply tag filters
            if (selectedTagIds.Any())
            {
                var taggedEvents = new HashSet<EventModel>();
                foreach (var tagId in selectedTagIds)
                {
                    UpdateTagSelectionFrequency(tagId);
                    var eventsWithTag = filteredEvents.Where(e =>
                        e.eventTags.Any(t => t.TagId == tagId));
                    taggedEvents.UnionWith(eventsWithTag);
                }
                filteredEvents = taggedEvents;
            }

            // Apply date range filter
            if (StartDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                filteredEvents = filteredEvents.Where(e => e.eventDate <= EndDate.Value);
            }

            FilteredEvents = new ObservableCollection<EventModel>(filteredEvents);
            UpdateRecommendedEvents();

            // Store filter operation
            _filterOperations.Enqueue(new FilterOperation
            {
                SearchQuery = SearchQuery,
                StartDate = StartDate,
                EndDate = EndDate,
                SelectedTagIds = selectedTagIds
            });
        }


        private void UpdateRecommendedEvents()
        {
            var allEvents = GetAllEvents().ToList();
            var eventScores = new Dictionary<EventModel, double>();

            foreach (var evt in allEvents)
            {
                double score = 0;

                // Tag preference scoring (weighted by frequency)
                foreach (var tag in evt.eventTags)
                {
                    if (_tagSelectionFrequency.ContainsKey(tag.TagId))
                    {
                        var tagWeight = (double)_tagSelectionFrequency[tag.TagId] /
                            _tagSelectionFrequency.Values.Sum();
                        score += tagWeight * 2.0; // Increased weight for tag preferences
                    }
                }

                // Search history relevance
                if (!string.IsNullOrEmpty(SearchQuery))
                {
                    var searchTerms = SearchQuery.ToLower().Split(' ');
                    foreach (var term in searchTerms)
                    {
                        if (evt.eventTitle.ToLower().Contains(term) ||
                            evt.eventDescription.ToLower().Contains(term))
                        {
                            score += 1.5; // Boost score for matching current search
                        }
                    }
                }

                // Previous search history
                foreach (var search in _previousSearches)
                {
                    if (evt.eventTitle.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        evt.eventDescription.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        score += 0.5; // Lower weight for historical searches
                    }
                }

                // Date relevance scoring
                var daysUntilEvent = (evt.eventDate - DateTime.Now).TotalDays;
                if (daysUntilEvent > 0)
                {
                    // Events within next 30 days get higher scores
                    score += Math.Max(0, 1 - (daysUntilEvent / 30)) * 1.5;
                }

                // Date range preference
                if (StartDate.HasValue && EndDate.HasValue)
                {
                    if (evt.eventDate >= StartDate.Value && evt.eventDate <= EndDate.Value)
                    {
                        score += 1.0; // Boost score for events within selected date range
                    }
                }

                // Previously selected tags
                foreach (var prevTag in _previouslySelectedTags)
                {
                    if (evt.eventTags.Any(t => t.TagId == prevTag.TagId))
                    {
                        score += 0.3; // Small boost for matching historical tag preferences
                    }
                }

                eventScores[evt] = score;
            }

            // Get top recommended events
            var recommended = eventScores
                .OrderByDescending(kvp => kvp.Value)
                .Take(4)
                .Select(kvp => kvp.Key)
                .ToList();

            RecommendedEvents = new ObservableCollection<EventModel>(recommended);
        }

        private void UpdateSearchFrequency(string searchTerm)
        {
            if (!_searchFrequency.ContainsKey(searchTerm))
            {
                _searchFrequency[searchTerm] = 0;
            }
            _searchFrequency[searchTerm]++;
        }

        private void UpdateTagSelectionFrequency(int tagId)
        {
            if (!_tagSelectionFrequency.ContainsKey(tagId))
            {
                _tagSelectionFrequency[tagId] = 0;
            }
            _tagSelectionFrequency[tagId]++;
            UpdateTagWeights();
        }

        private void UpdateTagWeights()
        {
            var totalSelections = _tagSelectionFrequency.Values.Sum();
            foreach (var tagId in _tagSelectionFrequency.Keys)
            {
                _tagWeights[tagId] = (double)_tagSelectionFrequency[tagId] / totalSelections;
            }
        }

        private double GetSearchWeight(string searchTerm)
        {
            if (!_searchFrequency.ContainsKey(searchTerm))
                return 0;

            var totalSearches = _searchFrequency.Values.Sum();
            return (double)_searchFrequency[searchTerm] / totalSearches;
        }

        private IEnumerable<EventModel> GetAllEvents()
        {
            foreach (var eventList in Events.Values)
            {
                foreach (var eventModel in eventList)
                {
                    yield return eventModel;
                }
            }
        }

        private IEnumerable<EventModel> GetRecommendedEvents()
        {
            return GetAllEvents().Take(4);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeEventIndexes()
        {
            // Hash table for quick keyword search
            // _eventsByKeyword = new Dictionary<string, HashSet<EventModel>>(); // Remove this line

            foreach (var evt in GetAllEvents())
            {
                // Index words from title and description
                var words = (evt.eventTitle + " " + evt.eventDescription)
                    .ToLower()
                    .Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    if (!_eventsByKeyword.ContainsKey(word))
                    {
                        _eventsByKeyword[word] = new HashSet<EventModel>();
                    }
                    _eventsByKeyword[word].Add(evt);
                }
            }
        }

        private void ApplyFilters()
        {
            var filteredResults = GetAllEvents();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var searchWords = SearchQuery.ToLower().Split(' ');
                var searchResults = new HashSet<EventModel>();

                foreach (var word in searchWords)
                {
                    if (_eventsByKeyword.ContainsKey(word))
                    {
                        if (searchResults.Count == 0)
                        {
                            searchResults.UnionWith(_eventsByKeyword[word]);
                        }
                        else
                        {
                            searchResults.IntersectWith(_eventsByKeyword[word]);
                        }
                    }
                }

                filteredResults = filteredResults.Intersect(searchResults);
                _searchHistory.Push(SearchQuery);
            }

            // Apply date range filter
            if (StartDate.HasValue)
            {
                filteredResults = filteredResults.Where(e => e.eventDate >= StartDate.Value);
            }
            if (EndDate.HasValue)
            {
                filteredResults = filteredResults.Where(e => e.eventDate <= EndDate.Value);
            }

            // Apply tag filters
            var selectedTags = Tags.Where(t => t.IsSelected).ToList();
            if (selectedTags.Any())
            {
                filteredResults = filteredResults.Where(e =>
                    e.eventTags.Any(t => selectedTags.Any(st => st.TagId == t.TagId)));
            }

            // Update filtered events
            FilteredEvents = new ObservableCollection<EventModel>(filteredResults);
            UpdateRecommendedEvents();
        }

        private void ClearFilters()
        {
            SearchQuery = string.Empty;
            StartDate = null;
            EndDate = null;
            foreach (var tag in Tags)
            {
                tag.IsSelected = false;
            }
            ApplyFilters();
        }

        private void InitializeAnnouncements()
        {
            Announcements.Clear();
            var allEvents = GetAllEvents();

            // Add upcoming events announcements
            var upcomingEvents = allEvents
                .Where(e => e.eventDate > DateTime.Now)
                .OrderBy(e => e.eventDate)
                .Take(5);

            foreach (var evt in upcomingEvents)
            {
                var daysUntil = (evt.eventDate - DateTime.Now).Days;
                AddEventAnnouncement(evt, daysUntil);
            }

            // Add recent past events announcements
            var recentEvents = allEvents
                .Where(e => e.eventDate <= DateTime.Now && e.eventDate >= DateTime.Now.AddDays(-7))
                .OrderByDescending(e => e.eventDate)
                .Take(3);

            foreach (var evt in recentEvents)
            {
                AddEventAnnouncement(evt, 0);
            }

            if (Announcements.Count > 0)
            {
                CurrentAnnouncement = Announcements[0];
            }
        }

        private void AddEventAnnouncement(EventModel evt, int daysUntil)
        {
            var announcement = new AnnouncementModel
            {
                announcementId = Announcements.Count + 1,
                announcementTitle = daysUntil > 0
                    ? $"Upcoming: {evt.eventTitle}"
                    : $"Recent: {evt.eventTitle}",
                announcementDescription = evt.eventDescription,
                announcementDate = evt.eventDate,
                announcementImage = evt.eventPhotos.FirstOrDefault(),
                announcementIcon = GetAnnouncementIcon(evt.eventTags),
                relatedEvent = evt,
                announcementType = AnnouncementModel.AnnouncementType.Event,
                isHighPriority = daysUntil <= 3 && daysUntil >= 0
            };

            Announcements.Add(announcement);
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

        private void UpdateAnnouncementPriorities()
        {
            foreach (var announcement in Announcements)
            {
                if (announcement.relatedEvent != null)
                {
                    var daysUntil = (announcement.relatedEvent.eventDate - DateTime.Now).Days;
                    announcement.isHighPriority = daysUntil <= 3;
                }
            }
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
    }

}
