using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{public interface IServiceRequestTree
    {
        void Insert(ServiceRequestModel request);
        void Delete(ServiceRequestModel request);
        void SetSortingStrategy(SortingStrategy strategy);
        IEnumerable<ServiceRequestModel> GetAllRequests();
        IEnumerable<ServiceRequestModel> GetRequestsByCategory(string category);
        IEnumerable<ServiceRequestModel> GetRequestsByDateRange(DateTime startDate, DateTime endDate);
        IEnumerable<ServiceRequestModel> GetRequestsByUser(string username);
        IEnumerable<ServiceRequestModel> GetRequestsByPriority();
        string GetTreeType();
        string GetTreeDescription();
    }
}
