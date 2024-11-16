using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.Core
{ 
    public class PriorityQueue<T>
    {
        private List<(T item, int priority)> elements = new List<(T item, int priority)>();

        public int Count => elements.Count;

        public void Enqueue(T item, int priority)
        {
            elements.Add((item, priority));
        }

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

        public bool IsEmpty => elements.Count == 0;
    }
}
