//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ST10144453_PROG7312.MVVM.Model.ServiceRequestMinHeap;

namespace ST10144453_PROG7312.Core
{
    /// <summary>
    /// Manages service requests and their prioritization.
    /// </summary>
    public class ServiceRequestManager
    {
        private static ServiceRequestManager _instance;
        private static readonly object _lock = new object();
        private readonly ServiceRequestMinHeap priorityHeap;
        private DateTime lastOptimizationCheck;

        public ObservableCollection<ServiceRequestModel> ServiceRequests { get; private set; }

        private ServiceRequestManager()
        {
            ServiceRequests = new ObservableCollection<ServiceRequestModel>();
            priorityHeap = new ServiceRequestMinHeap();
            InitializeDummyData();
        }

        /// <summary>
        /// Gets the singleton instance of the ServiceRequestManager class.
        /// </summary>
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

        /// <summary>
        /// Adds a new service request.
        /// </summary>
        /// <param name="request">The service request to add.</param>
        public void AddRequest(ServiceRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Service request cannot be null.");
            }

            request.RequestID = Guid.NewGuid();
            request.RequestDate = DateTime.Now;
            ServiceRequests.Add(request);
            priorityHeap.Insert(request);
        }

        /// <summary>
        /// Updates an existing service request.
        /// </summary>
        /// <param name="request">The updated service request.</param>
        public void UpdateRequest(ServiceRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Service request cannot be null.");
            }

            var existingRequest = ServiceRequests.FirstOrDefault(r => r.RequestID == request.RequestID);
            if (existingRequest != null)
            {
                var index = ServiceRequests.IndexOf(existingRequest);
                ServiceRequests[index] = request;
                priorityHeap.UpdatePriority(request.RequestID, GetPriorityScore(request));
            }
        }

        /// <summary>
        /// Gets the prioritized service requests.
        /// </summary>
        /// <returns>An enumerable collection of prioritized service requests.</returns>
        public IEnumerable<ServiceRequestModel> GetPrioritizedRequests()
        {
            if ((DateTime.Now - lastOptimizationCheck).TotalMinutes >= 30)
            {
                OptimizeRequests();
                lastOptimizationCheck = DateTime.Now;
            }
            return priorityHeap.GetPrioritizedRequests();
        }

        private int GetPriorityScore(ServiceRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Service request cannot be null.");
            }

            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;

            // Age factor
            score += age > 14 ? 50 :
                    age > 7 ? 40 :
                    age > 3 ? 30 : 20;

            // Status factor
            switch (request.Status)
            {
                case "Critical":
                    score += 100;
                    break;
                case "High":
                    score += 75;
                    break;
                case "Medium":
                    score += 50;
                    break;
                case "Low":
                    score += 25;
                    break;
                default:
                    score += 0;
                    break;
            }

            // Category priority
            switch (request.Category)
            {
                case "Water & Sanitation":
                case "Electricity":
                    score += 50;
                    break;
                case "Public Safety":
                    score += 45;
                    break;
                case "Roads & Transport":
                    score += 40;
                    break;
                case "Waste Management":
                    score += 35;
                    break;
                case "Housing":
                    score += 30;
                    break;
                case "Parks & Recreation":
                    score += 25;
                    break;
                default:
                    score += 20;
                    break;
            }

            return score;
        }

        /// <summary>
        /// Gets a list of all available categories.
        /// </summary>
        /// <returns>A list of category names.</returns>
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

        /// <summary>
        /// Gets a list of all service requests.
        /// </summary>
        /// <returns>A list of service requests.</returns>
        public List<ServiceRequestModel> GetAllRequests()
        {
            return ServiceRequests.ToList();
        }

        /// <summary>
        /// Optimizes the service requests and provides a report.
        /// </summary>
        public void OptimizeRequests()
        {
            var stats = priorityHeap.GetStatistics();

            System.Diagnostics.Debug.WriteLine($"Service Request Optimization Report:");
            System.Diagnostics.Debug.WriteLine($"Total Requests: {stats.TotalRequests}");
            System.Diagnostics.Debug.WriteLine($"Pending Requests: {stats.TotalPendingRequests}");
            System.Diagnostics.Debug.WriteLine($"Average Priority Score: {stats.AveragePriorityScore:F2}");

            foreach (var category in stats.RequestsByCategory)
            {
                System.Diagnostics.Debug.WriteLine($"{category.Key}: {category.Value} requests");
            }
        }

        /// <summary>
        /// Gets the optimization statistics of the service requests.
        /// </summary>
        /// <returns>The optimization statistics.</returns>
        public ServiceRequestStatistics GetOptimizationStatistics()
        {
            return priorityHeap.GetStatistics();
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
