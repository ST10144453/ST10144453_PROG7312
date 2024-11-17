using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.Core
{
    public class ServiceRequestManager
    {
        private static ServiceRequestManager _instance;
        private static readonly object _lock = new object();

        public ObservableCollection<ServiceRequestModel> ServiceRequests { get; private set; }

        private ServiceRequestManager()
        {
            ServiceRequests = new ObservableCollection<ServiceRequestModel>();
            InitializeDummyData();
        }

        public static ServiceRequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceRequestManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeDummyData()
        {
            var dummyData = new List<ServiceRequestModel>
            {
                new ServiceRequestModel
                {
                    RequestID = Guid.NewGuid(),
                    Category = "Water & Sanitation",
                    Description = "Water leak on Main Street",
                    Status = "Pending",
                    CreatedBy = "john.doe",
                    RequestDate = DateTime.Now.AddDays(-5),
                    FirstName = "John",
                    Surname = "Doe",
                    Email = "john.doe@email.com",
                    PhoneNumber = "0123456789",
                    AdditionalAddress = "123 Main Street",
                    PreferredFeedbackMethod = "Email"
                },
                new ServiceRequestModel
                {
                    RequestID = Guid.NewGuid(),
                    Category = "Electricity",
                    Description = "Street light not working on Oak Avenue",
                    Status = "In Progress",
                    CreatedBy = "jane.smith",
                    RequestDate = DateTime.Now.AddDays(-3),
                    FirstName = "Jane",
                    Surname = "Smith",
                    Email = "jane.smith@email.com",
                    PhoneNumber = "0987654321",
                    AdditionalAddress = "456 Oak Avenue",
                    PreferredFeedbackMethod = "Phone"
                },
                new ServiceRequestModel
                {
                    RequestID = Guid.NewGuid(),
                    Category = "Roads & Transport",
                    Description = "Pothole on Maple Drive",
                    Status = "Completed",
                    CreatedBy = "bob.wilson",
                    RequestDate = DateTime.Now.AddDays(-7),
                    FirstName = "Bob",
                    Surname = "Wilson",
                    Email = "bob.wilson@email.com",
                    PhoneNumber = "0111222333",
                    AdditionalAddress = "789 Maple Drive",
                    PreferredFeedbackMethod = "Email"
                }
            };

            foreach (var request in dummyData)
            {
                ServiceRequests.Add(request);
            }
        }

        public void AddRequest(ServiceRequestModel request)
        {
            request.RequestID = Guid.NewGuid();
            request.RequestDate = DateTime.Now;
            ServiceRequests.Add(request);
        }

        public void UpdateRequest(ServiceRequestModel request)
        {
            var existingRequest = ServiceRequests.FirstOrDefault(r => r.RequestID == request.RequestID);
            if (existingRequest != null)
            {
                var index = ServiceRequests.IndexOf(existingRequest);
                ServiceRequests[index] = request;
            }
        }

        public List<string> GetAllCategories()
        {
            return new List<string>
            {
                "Water & Sanitation",
                "Electricity",
                "Roads & Transport",
                "Waste Management",
                "Parks & Recreation",
                "Public Safety",
                "Housing",
                "Other"
            };
        }

        public List<ServiceRequestModel> GetAllRequests()
        {
            return ServiceRequests.ToList();
        }
    }
}
