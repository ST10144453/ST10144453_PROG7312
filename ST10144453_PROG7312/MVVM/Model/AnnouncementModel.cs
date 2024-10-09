using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class AnnouncementModel
    {
        public Guid announcementId { get; set; }
        public string announcementTitle { get; set; }
        public string announcementDescription { get; set; }
        public DateTime announcementDate { get; set; }
        public List<String> announcementPhotos { get; set; }

        public AnnouncementModel() 
        { 
           announcementId = Guid.NewGuid();
        }
    }
}
