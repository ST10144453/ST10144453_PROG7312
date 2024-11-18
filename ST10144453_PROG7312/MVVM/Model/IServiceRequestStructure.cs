using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public interface IServiceRequestStructure
    {
        void Insert(ServiceRequestModel request);
        IEnumerable<ServiceRequestModel> GetAllRequests();
        string GetStructureType();
        string GetStructureDescription();
        void UpdateDisplay();
    }
}
