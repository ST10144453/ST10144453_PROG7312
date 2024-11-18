using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestGraph
    {
        private Dictionary<Guid, List<(Guid TargetId, double Weight)>> adjacencyList;
        private Dictionary<Guid, ServiceRequestModel> requests;

        public void AddEdge(Guid source, Guid target, double weight)
        {
            if (!adjacencyList.ContainsKey(source))
                adjacencyList[source] = new List<(Guid, double)>();

            adjacencyList[source].Add((target, weight));
        }

        public double CalculateRelationshipStrength(ServiceRequestModel req1, ServiceRequestModel req2)
        {
            double strength = 0;

            // Location proximity
            if (req1.AdditionalAddress == req2.AdditionalAddress) strength += 0.4;

            // Category relationship
            if (req1.Category == req2.Category) strength += 0.3;

            // Time proximity
            var timeDiff = Math.Abs((req1.RequestDate - req2.RequestDate).Days);
            strength += timeDiff < 7 ? 0.3 : 0;

            return strength;
        }
    }
}
