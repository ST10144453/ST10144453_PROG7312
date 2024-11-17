using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class EventModel : IComparable<EventModel>
    {
        public Guid eventId { get; set; }
        public DateTime eventDate { get; set; }
        public string eventTitle { get; set; }
        public string eventLocation { get; set; }
        public string eventDescription { get; set; }
        public List<string> eventPhotos { get; set; }
        public List<TagsModel> eventTags { get; set; }

        // Add static Events list
        public static List<EventModel> Events = new List<EventModel>
        {
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(7),
                eventTitle = "Community Cleanup Day",
                eventLocation = "Langa Community Center",
                eventDescription = "Join us for a community-wide cleanup initiative to make our neighborhood cleaner and safer.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Cleanup/cleanup1.jpg", "pack://application:,,,/Resources/Hardcoded/Cleanup/cleanup2.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[0], TagsModel.Tags[3] } // Community Engagement, Environmental
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(14),
                eventTitle = "Heritage Day Festival",
                eventLocation = "V&A Waterfront, Cape Town",
                eventDescription = "Celebrate South Africa's rich cultural heritage with traditional food, music, and performances.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Heritage/heritage1.jpg", "pack://application:,,,/Resources/Hardcoded/Heritage/heritage2.jpg", "pack://application:,,,/Resources/Hardcoded/Heritage/heritage3.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[1], TagsModel.Tags[10] } // Cultural Festivals, Music and Entertainment
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(12),
                eventTitle = "Two Oceans Marathon",
                eventLocation = "Newlands, Cape Town",
                eventDescription = "An ultra-marathon held annually, offering participants the chance to run one of the most scenic routes in the world.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Marathon/marathon1.jpg", "pack://application:,,,/Resources/Hardcoded/Marathon/marathon2.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[2], TagsModel.Tags[15] } // Sports Events, Health and Wellness
            }
        };

        public string Title => eventTitle;
        public DateTime Date => eventDate;
        public string Location => eventLocation;
        public string Description => eventDescription;

        public EventModel()
        {
            eventId = Guid.NewGuid();
            eventPhotos = new List<string>();

        }

        public int CompareTo(EventModel other)
        {
            if (other == null) return 1;
            return this.eventDate.CompareTo(other.eventDate);
        }
    }

    public class EventList
    {
        private static readonly Lazy<EventList> lazy = new Lazy<EventList>(() => new EventList());

        public static EventList Instance => lazy.Value;

        public ObservableCollection<EventModel> Events { get; private set; }
        private EventList()

        {
            Events = new ObservableCollection<EventModel>
            {



            new EventModel
            {
                eventDate = DateTime.Now.AddDays(-15),
                eventTitle = "Mandela Day Cleanup",
                eventLocation = "Langa Township, Cape Town",
                eventDescription = "Celebrate Nelson Mandela Day by joining the community in cleaning up the streets and public spaces in Langa.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/MandelaDay/mandela1.jpg", "pack://application:,,,/Resources/Hardcoded/MandelaDay/mandela2.jpg", "pack://application:,,,/Resources/Hardcoded/MandelaDay/mandela3.jpg", "pack://application:,,,/Resources/Hardcoded/MandelaDay/mandela4.jpg", "pack://application:,,,/Resources/Hardcoded/MandelaDay/mandela5.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[0], TagsModel.Tags[4] }, // Community Engagement, Environmental Initiatives
               
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(-7),
                eventTitle = "Heritage Day Braai",
                eventLocation = "Company's Garden, Cape Town",
                eventDescription = "Join us in celebrating Heritage Day with a traditional South African braai in the heart of Cape Town.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/HeratigeDay/heratige1.jpg", "pack://application:,,,/Resources/Hardcoded/HeratigeDay/heratige2.jpg", "pack://application:,,,/Resources/Hardcoded/HeratigeDay/heratige3.jpg", "pack://application:,,,/Resources/Hardcoded/HeratigeDay/heratige4.jpeg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[1], TagsModel.Tags[12] }, // Cultural Festivals, Social Development
               
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(-5),
                eventTitle = "Cape Town Cycle Tour",
                eventLocation = "Cape Town Stadium",
                eventDescription = "The world's largest timed cycling event, bringing together thousands of cyclists for a scenic ride around the Cape Peninsula.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle1.jpg", "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle2.jpg", "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle3.jpg", "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle4.jpg", "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle5.jpeg", "pack://application:,,,/Resources/Hardcoded/CycleTour/cycle6.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[2], TagsModel.Tags[3] }, // Sports Events, Health and Wellness
               
            },
new EventModel
{
    eventDate = DateTime.Now.AddDays(-2),
    eventTitle = "Kirstenbosch Craft Market",
    eventLocation = "Kirstenbosch National Botanical Garden, Cape Town",
    eventDescription = "A vibrant craft market showcasing local artisans, with handmade crafts, food, and live entertainment.",
    eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Kirstenbosch/kirstenbosch1.jpg", "pack://application:,,,/Resources/Hardcoded/Kirstenbosch/kirstenbosch2.jpg", "pack://application:,,,/Resources/Hardcoded/Kirstenbosch/kirstenbosch3.jpg", "pack://application:,,,/Resources/Hardcoded/Kirstenbosch/kirstenbosch4.jpg" },
    eventTags = new List<TagsModel> { TagsModel.Tags[7], TagsModel.Tags[8] }, // Art and Craft Fairs, Local Markets
    
},
new EventModel
{
    eventDate = DateTime.Now.AddDays(-1),
    eventTitle = "Western Cape Job Fair",
    eventLocation = "CTICC, Cape Town",
    eventDescription = "A major job fair offering opportunities in various industries, with workshops and networking for job seekers.",
    eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/JobFair/job1.jpg", "pack://application:,,,/Resources/Hardcoded/JobFair/job2.jpg", "pack://application:,,,/Resources/Hardcoded/JobFair/job3.jpg" },
    eventTags = new List<TagsModel> { TagsModel.Tags[9], TagsModel.Tags[6] }, // Job Fairs, Education and Workshops
    
},
new EventModel
{
    eventDate = DateTime.Now.AddDays(1),
    eventTitle = "Table Mountain Hiking for Health",
    eventLocation = "Table Mountain, Cape Town",
    eventDescription = "A guided hike up Table Mountain to promote health and wellness. All fitness levels are welcome.",
    eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/TblHike/hike1.jpg", "pack://application:,,,/Resources/Hardcoded/TblHike/hike2.jpg", "pack://application:,,,/Resources/Hardcoded/TblHike/hike3.jpg", "pack://application:,,,/Resources/Hardcoded/TblHike/hike4.jpg", "pack://application:,,,/Resources/Hardcoded/TblHike/hike5.jpg" },
    eventTags = new List<TagsModel> { TagsModel.Tags[3], TagsModel.Tags[0] }, // Health and Wellness, Community Engagement
    
    
},
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(3),
                eventTitle = "Disaster Preparedness Workshop",
                eventLocation = "Bellville Civic Centre, Cape Town",
                eventDescription = "A workshop focused on preparing communities for natural disasters, with tips and strategies for personal and public safety.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Disaster/disaster1.jpg", "pack://application:,,,/Resources/Hardcoded/Disaster/disaster2.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[13], TagsModel.Tags[5] } // Disaster Preparedness, Public Safety
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(5),
                eventTitle = "Youth Empowerment Summit",
                eventLocation = "Soweto Theatre, Johannesburg",
                eventDescription = "An interactive summit designed to empower young South Africans with skills development, mentorship, and leadership training.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Youth/youth1.jpg", "pack://application:,,,/Resources/Hardcoded/Youth/youth2.jpg", "pack://application:,,,/Resources/Hardcoded/Youth/youth3.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[16], TagsModel.Tags[0] } // Youth Programs, Community Engagement
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(8),
                eventTitle = "Durban July",
                eventLocation = "Greyville Racecourse, Durban",
                eventDescription = "South Africa's premier horse racing event, combining high fashion, sport, and entertainment in Durban.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Race/Race1.jpg", "pack://application:,,,/Resources/Hardcoded/Race/race2.jpg", "pack://application:,,,/Resources/Hardcoded/Race/race3.jpg", "pack://application:,,,/Resources/Hardcoded/Race/race4.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[2], TagsModel.Tags[1] } // Sports Events, Cultural Festivals
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(10),
                eventTitle = "Oppikoppi Music Festival",
                eventLocation = "Northam, Limpopo",
                eventDescription = "One of South Africa's largest multi-genre music festivals, featuring local and international artists in a rustic outdoor setting.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Oppikoppi/oppikoppi1.jpg", "pack://application:,,,/Resources/Hardcoded/Oppikoppi/oppikoppi2.jpg", "pack://application:,,,/Resources/Hardcoded/Oppikoppi/oppikoppi3.jpg", "pack://application:,,,/Resources/Hardcoded/Oppikoppi/oppikoppi4.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[1], TagsModel.Tags[10] } // Cultural Festivals, Music and Entertainment
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(12),
                eventTitle = "Two Oceans Marathon",
                eventLocation = "Newlands, Cape Town",
                eventDescription = "An ultra-marathon held annually, offering participants the chance to run one of the most scenic routes in the world.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Marathon/marathon1.JPG", "pack://application:,,,/Resources/Hardcoded/Marathon/marathon2.jpg", "pack://application:,,,/Resources/Hardcoded/Marathon/marathon3.jpg", "pack://application:,,,/Resources/Hardcoded/Marathon/marathon4.png", "pack://application:,,,/Resources/Hardcoded/Marathon/marathon5.jpeg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[2], TagsModel.Tags[3] } // Sports Events, Health and Wellness
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(14),
                eventTitle = "Franschhoek Bastille Festival",
                eventLocation = "Franschhoek, Western Cape",
                eventDescription = "A celebration of French heritage in South Africa with food, wine, and festivities in the picturesque town of Franschhoek.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Franschoek/franschoek1.jpg", "pack://application:,,,/Resources/Hardcoded/Franschoek/franschoek2.jpg", "pack://application:,,,/Resources/Hardcoded/Franschoek/franschoek3.jpg", "pack://application:,,,/Resources/Hardcoded/Franschoek/franschoek4.jpg", "pack://application:,,,/Resources/Hardcoded/Franschoek/franschoek5.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[1], TagsModel.Tags[8] } // Cultural Festivals, Local Markets
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(16),
                eventTitle = "National Arts Festival",
                eventLocation = "Grahamstown, Eastern Cape",
                eventDescription = "A celebration of the arts, featuring live performances, exhibitions, and workshops by artists from around the world.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/National/national1.jpg", "pack://application:,,,/Resources/Hardcoded/National/national2.jpg", "pack://application:,,,/Resources/Hardcoded/National/national3.jpg", "pack://application:,,,/Resources/Hardcoded/National/national4.jpg", "pack://application:,,,/Resources/Hardcoded/National/national5.jpg" },
                eventTags = new List<TagsModel> { TagsModel.Tags[7], TagsModel.Tags[10] } // Art and Craft Fairs, Music and Entertainment
            },
            new EventModel
            {
                eventDate = DateTime.Now.AddDays(18),
                eventTitle = "Cape Winelands Harvest Festival",
                eventLocation = "Stellenbosch, Western Cape",
                eventDescription = "Celebrate the wine harvest season with wine tastings, live music, and food markets in the heart of the Cape Winelands.",
                eventPhotos = new List<string> { "pack://application:,,,/Resources/Hardcoded/Harvest/harvest1.jpg", "pack://application:,,,/Resources/Hardcoded/Harvest/harvest2.jpg", "pack://application:,,,/Resources/Hardcoded/Harvest/harvest3.jpg"},
                eventTags = new List<TagsModel> { TagsModel.Tags[1], TagsModel.Tags[8] } // Cultural Festivals, Local Markets
            }
            };
        }

        public ObservableCollection<EventModel> GetEventsForDate(DateTime date)
        {
            return new ObservableCollection<EventModel>(Events.Where(e => e.eventDate.Date == date.Date));
        }
    }
}
