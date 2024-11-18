using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
