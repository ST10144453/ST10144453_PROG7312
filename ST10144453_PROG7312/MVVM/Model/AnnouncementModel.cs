using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class AnnouncementModel
    {
        public int announcementId { get; set; }
        public string announcementTitle { get; set; }
        public string announcementDescription { get; set; }
        public DateTime announcementDate { get; set; }

        public AnnouncementModel()
        {
        }

        // Hardcoded announcements
        public static List<AnnouncementModel> Announcements = new List<AnnouncementModel>
        {
            new AnnouncementModel
            {
                announcementId = 1,
                announcementTitle = "Cleanup Supplies",
                announcementDescription = "Please bring your own gloves and trash bags.",
                announcementDate = DateTime.Now.AddDays(-16)
            },
            new AnnouncementModel
            {
                announcementId = 2,
                announcementTitle = "Meeting Point",
                announcementDescription = "We will meet at the Langa Community Center at 9 AM.",
                announcementDate = DateTime.Now.AddDays(-16)
            },
            new AnnouncementModel
            {
                announcementId = 3,
                announcementTitle = "Braai Competition",
                announcementDescription = "Enter our braai competition for a chance to win great prizes!",
                announcementDate = DateTime.Now.AddDays(-8)
            },
            new AnnouncementModel
            {
                announcementId = 4,
                announcementTitle = "Cultural Performances",
                announcementDescription = "Enjoy live performances from local artists throughout the day.",
                announcementDate = DateTime.Now.AddDays(-8)
            },
            new AnnouncementModel
            {
                announcementId = 5,
                announcementTitle = "Route Information",
                announcementDescription = "Check the official website for detailed route information and safety guidelines.",
                announcementDate = DateTime.Now.AddDays(-6)
            },
            new AnnouncementModel
            {
                announcementId = 6,
                announcementTitle = "Registration Deadline",
                announcementDescription = "Make sure to register by the end of the week to secure your spot.",
                announcementDate = DateTime.Now.AddDays(-6)
            }
        };
    }
}
