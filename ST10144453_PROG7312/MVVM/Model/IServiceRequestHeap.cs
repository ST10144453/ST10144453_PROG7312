using System;
using System.Collections.Generic;

namespace ST10144453_PROG7312.MVVM.Model
{
    public interface IServiceRequestHeap
    {
        void Insert(ServiceRequestModel request);
        ServiceRequestModel ExtractHighestPriority();
        void UpdatePriority(Guid requestId, int newPriority);
        IEnumerable<ServiceRequestModel> GetPrioritizedRequests();
    }
}