using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestNode
    {
        public ServiceRequestModel Data { get; set; }
        public ServiceRequestNode Left { get; set; }
        public ServiceRequestNode Right { get; set; }
        public int Height { get; set; }

        public ServiceRequestNode(ServiceRequestModel data)
        {
            Data = data;
            Height = 1;
        }
    }
}
