//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.Core
{
    /// <summary>
    /// Represents a priority queue manager.
    /// </summary>
    public class PriorityQueueManager
    {
        private SortedSet<EventModel> priorityQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueueManager"/> class.
        /// </summary>
        /// <param name="events">The collection of events to initialize the priority queue with.</param>
        public PriorityQueueManager(IEnumerable<EventModel> events = null)
        {
            priorityQueue = new SortedSet<EventModel>(events ?? new List<EventModel>());
        }

        /// <summary>
        /// Adds an event to the priority queue.
        /// </summary>
        /// <param name="eventModel">The event to add.</param>
        public void AddEvent(EventModel eventModel)
        {
            priorityQueue.Add(eventModel);
        }

        /// <summary>
        /// Gets the next event from the priority queue.
        /// </summary>
        /// <returns>The next event, or null if the priority queue is empty.</returns>
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

        /// <summary>
        /// Gets all the events in the priority queue.
        /// </summary>
        /// <returns>An enumerable collection of events.</returns>
        public IEnumerable<EventModel> GetAllEvents()
        {
            return priorityQueue;
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
