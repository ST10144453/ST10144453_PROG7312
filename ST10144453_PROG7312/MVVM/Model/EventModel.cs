using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class EventModel
    {
        public Guid eventId { get; set; }
        public DateTime eventDate { get; set; }
        public DateTime eventTime { get; set; }
        public string eventTitle { get; set; }
        public string eventLocation { get; set; }
        public string eventDescription { get; set; }
        public DateTime eventStartTime { get; set; }
        public DateTime? eventEndTime { get; set; }
        public List<String> eventPhotos { get; set; }
        public List<String> eventTags { get; set; }

        public EventModel()
        {
            eventId = Guid.NewGuid();
        }
    }
}
