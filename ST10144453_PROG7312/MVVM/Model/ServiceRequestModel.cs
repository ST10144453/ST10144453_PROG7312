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

        private static List<ServiceRequestModel> _allRequests = new List<ServiceRequestModel>
        {
            new ServiceRequestModel
            {
                RequestID = Guid.NewGuid(),
                Category = "Infrastructure",
                Description = "Street light not working on Main Road",
                PlottingPoint = "Main Road",
                AdditionalAddress = "London",
                RequestDate = DateTime.Now.AddDays(-5),
                SupportingEvidence = new List<MediaItem>(),
                LinkedReports = new List<ReportModel>(),
                FirstName = "John",
                Surname = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "07911123456",
                PreferredFeedbackMethod = "Email",
                Status = "Pending",
                CreatedBy = "user123"
            },
            new ServiceRequestModel
            {
                RequestID = Guid.NewGuid(),
                Category = "Roads",
                Description = "Large pothole on Church Street",
                PlottingPoint = "Church Street",
                AdditionalAddress = "London",
                RequestDate = DateTime.Now.AddDays(-3),
                SupportingEvidence = new List<MediaItem>(),
                LinkedReports = new List<ReportModel>(),
                FirstName = "Jane",
                Surname = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "07911123457",
                PreferredFeedbackMethod = "Phone",
                Status = "In Progress",
                CreatedBy = "user456"
            }
            // Add more sample data as needed
        };

        public static List<ServiceRequestModel> GetAllRequests()
        {
            return _allRequests;
        }

        public static void AddRequest(ServiceRequestModel request)
        {
            request.RequestID = Guid.NewGuid();
            request.RequestDate = DateTime.Now;
            request.SupportingEvidence = new List<MediaItem>();
            request.LinkedReports = new List<ReportModel>();
            request.Status = "Pending"; // Default status
            _allRequests.Add(request);
        }

        public static List<string> GetAllCategories()
        {
            return new List<string>
            {
                "Infrastructure",
                "Roads",
                "Water",
                "Electricity",
                "Waste Management",
                "Public Safety",
                "Parks and Recreation",
                "Other"
            };
        }

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
