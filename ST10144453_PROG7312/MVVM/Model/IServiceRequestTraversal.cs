using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public interface IServiceRequestTraversal
{
    IEnumerable<ServiceRequestModel> GetRelatedRequests(
        ServiceRequestModel selectedRequest, 
        bool isStaffUser,
        int maxDepth = 2);
    void AddRequest(ServiceRequestModel request);
}
}
