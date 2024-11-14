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

        public void AddServiceRequest(ServiceRequestModel request)
        {
            ServiceRequests.Add(request);
        }

        public void UpdateServiceRequest(ServiceRequestModel request)
        {
            var index = ServiceRequests.IndexOf(
                ServiceRequests.FirstOrDefault(r => r.RequestID == request.RequestID));
            if (index != -1)
            {
                ServiceRequests[index] = request;
            }
        }
    }
}
