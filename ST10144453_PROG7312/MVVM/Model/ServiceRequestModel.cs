using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestModel
    {
        public Guid RequestID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string PlottingPoint { get; set; }
        public string AdditionalAddress { get; set; }
        public DateTime RequestDate { get; set; }
        public List<MediaItem> SupportingEvidence { get; set; }
        public List<ReportModel> LinkedReports { get; set; }

        // Contact Details
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PreferredFeedbackMethod { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }

        public ServiceRequestModel()
        {
            RequestID = Guid.NewGuid();
            RequestDate = DateTime.Now;
            SupportingEvidence = new List<MediaItem>();
            LinkedReports = new List<ReportModel>();
            Status = "Pending"; // Default status
        }
    }
}
