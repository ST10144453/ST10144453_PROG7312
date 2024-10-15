using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.Core
{
    public class PriorityQueueManager
    {
        private SortedSet<EventModel> priorityQueue;

        public PriorityQueueManager(IEnumerable<EventModel> events = null)
        {
            priorityQueue = new SortedSet<EventModel>(events ?? new List<EventModel>());
        }

        public void AddEvent(EventModel eventModel)
        {
            priorityQueue.Add(eventModel);
        }

        public EventModel GetNextEvent()
        {
            if (priorityQueue.Count > 0)
            {
                var nextEvent = priorityQueue.Min;
                priorityQueue.Remove(nextEvent);
                return nextEvent;
            }
            return null;
        }

        public IEnumerable<EventModel> GetAllEvents()
        {
            return priorityQueue;
        }
    }
}
