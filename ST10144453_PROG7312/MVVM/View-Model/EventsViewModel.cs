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
            Console.WriteLine("Filtering events...");

            if (selectedTagIds.Count > 0)
            {
                Console.WriteLine($"Selected tags: {string.Join(", ", selectedTagIds)}");
            }
            else
            {
                Console.WriteLine("No tags selected.");
            }

            List<EventModel> filtered;

            if (selectedTagIds.Count == 0)
            {
                filtered = GetAllEvents().Where(e =>
                    (string.IsNullOrEmpty(SearchQuery) || e.eventTitle.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                ).ToList();

                Console.WriteLine("No tags selected. Showing all events.");
            }
            else
            {
                Console.WriteLine("Filtering events by selected tags...");

                filtered = GetAllEvents()
                    .Where(e =>
                    {
                        Console.WriteLine($"Event: {e.eventTitle}, Tags: {string.Join(", ", e.eventTags.Select(t => t.TagId))}");
                        return e.eventTags.Any(tag => selectedTagIds.Contains(tag.TagId)) &&
                               (string.IsNullOrEmpty(SearchQuery) || e.eventTitle.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
                    })
                    .ToList();

                Console.WriteLine($"Matching events count: {filtered.Count}");
            }

            foreach (var evt in filtered)
            {
                Console.WriteLine($"Filtered Event: {evt.eventTitle}, Tags: {string.Join(", ", evt.eventTags.Select(t => t.TagName))}");
            }

            FilteredEvents = new ObservableCollection<EventModel>(filtered);
            UpdateRecommendedEvents();

            Console.WriteLine("Filter application complete.");
        }
    

        private void UpdateRecommendedEvents()
        {
            var recommended = GetAllEvents()
                .OrderByDescending(e => _previouslySelectedTagIds.Count(tagId => e.eventTags.Any(tag => tag.TagId == tagId)))
                .ThenByDescending(e => _previousSearches.Count(s => e.eventTitle.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0))
                .Take(4)
                .ToList();

            RecommendedEvents = new ObservableCollection<EventModel>(recommended);
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
    }

}
