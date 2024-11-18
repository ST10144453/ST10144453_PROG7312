//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.Core
{ 
  /// <summary>
    /// Represents a priority queue data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the priority queue.</typeparam>
    public class PriorityQueue<T>
    {
        private List<(T item, int priority)> elements = new List<(T item, int priority)>();

        /// <summary>
        /// Gets the number of elements in the priority queue.
        /// </summary>
        public int Count => elements.Count;

        /// <summary>
        /// Adds an element to the priority queue with the specified priority.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <param name="priority">The priority of the element.</param>
        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
        }

        /// <summary>
        /// Removes and returns the element with the highest priority from the priority queue.
        /// </summary>
        /// <returns>The element with the highest priority.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the priority queue is empty.</exception>
        public T Dequeue()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            int highestPriorityIndex = 0;
            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].priority > elements[highestPriorityIndex].priority)
                {
                    highestPriorityIndex = i;
                }
            }

            T highestPriorityItem = elements[highestPriorityIndex].item;
            elements.RemoveAt(highestPriorityIndex);
            return highestPriorityItem;
        }

        /// <summary>
        /// Gets a value indicating whether the priority queue is empty.
        /// </summary>
        public bool IsEmpty => elements.Count == 0;
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
