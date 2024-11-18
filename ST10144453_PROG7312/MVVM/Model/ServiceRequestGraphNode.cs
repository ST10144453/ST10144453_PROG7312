using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestGraphNode
    {
        public ServiceRequestModel Data { get; set; }
        public Dictionary<string, ServiceRequestGraphNode> Adjacent { get; set; }

        public ServiceRequestGraphNode(ServiceRequestModel data)
        {
            Data = data;
            Adjacent = new Dictionary<string, ServiceRequestGraphNode>();
        }
    }
}
