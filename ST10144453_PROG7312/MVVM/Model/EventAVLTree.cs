using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class EventAVLTree
    {
        private EventNode root;

        private int GetHeight(EventNode node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(EventNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private EventNode RightRotate(EventNode y)
        {
            EventNode x = y.Left;
            EventNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private EventNode LeftRotate(EventNode x)
        {
            EventNode y = x.Right;
            EventNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public void Insert(EventModel eventModel)
        {
            root = InsertRec(root, eventModel);
        }

        private EventNode InsertRec(EventNode node, EventModel eventModel)
        {
            if (node == null)
                return new EventNode(eventModel);

            if (eventModel.eventDate.CompareTo(node.Data.eventDate) < 0)
                node.Left = InsertRec(node.Left, eventModel);
            else if (eventModel.eventDate.CompareTo(node.Data.eventDate) > 0)
                node.Right = InsertRec(node.Right, eventModel);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && eventModel.eventDate.CompareTo(node.Left.Data.eventDate) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && eventModel.eventDate.CompareTo(node.Right.Data.eventDate) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && eventModel.eventDate.CompareTo(node.Left.Data.eventDate) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && eventModel.eventDate.CompareTo(node.Right.Data.eventDate) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        public IEnumerable<EventModel> InOrderTraversal()
        {
            var events = new List<EventModel>();
            InOrderTraversalRec(root, events);
            return events;
        }

        private void InOrderTraversalRec(EventNode node, List<EventModel> events)
        {
            if (node != null)
            {
                InOrderTraversalRec(node.Left, events);
                events.Add(node.Data);
                InOrderTraversalRec(node.Right, events);
            }
        }
        public EventModel FindClosestEvent(DateTime date)
        {
            return FindClosestEventRec(root, date);
        }

        private EventModel FindClosestEventRec(EventNode node, DateTime date)
        {
            if (node == null) return null;

            if (node.Data.eventDate == date)
                return node.Data;

            if (date < node.Data.eventDate)
            {
                var leftResult = FindClosestEventRec(node.Left, date);
                if (leftResult == null) return node.Data;
                return (date - leftResult.eventDate).Duration() < (date - node.Data.eventDate).Duration()
                    ? leftResult : node.Data;
            }

            var rightResult = FindClosestEventRec(node.Right, date);
            if (rightResult == null) return node.Data;
            return (date - rightResult.eventDate).Duration() < (date - node.Data.eventDate).Duration()
                ? rightResult : node.Data;
        }

        public IEnumerable<EventModel> GetEventsInDateRange(DateTime startDate, DateTime endDate)
        {
            var events = new List<EventModel>();
            GetEventsInDateRangeRec(root, startDate, endDate, events);
            return events;
        }

        private void GetEventsInDateRangeRec(EventNode node, DateTime startDate, DateTime endDate, List<EventModel> events)
        {
            if (node == null) return;

            if (node.Data.eventDate >= startDate)
                GetEventsInDateRangeRec(node.Left, startDate, endDate, events);

            if (node.Data.eventDate >= startDate && node.Data.eventDate <= endDate)
                events.Add(node.Data);

            if (node.Data.eventDate <= endDate)
                GetEventsInDateRangeRec(node.Right, startDate, endDate, events);
        }

        public void Delete(EventModel eventModel)
        {
            root = DeleteRec(root, eventModel);
        }

        private EventNode DeleteRec(EventNode node, EventModel eventModel)
        {
            if (node == null) return null;

            if (eventModel.eventDate.CompareTo(node.Data.eventDate) < 0)
                node.Left = DeleteRec(node.Left, eventModel);
            else if (eventModel.eventDate.CompareTo(node.Data.eventDate) > 0)
                node.Right = DeleteRec(node.Right, eventModel);
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    EventNode temp = null;
                    if (temp == node.Left)
                        temp = node.Right;
                    else
                        temp = node.Left;

                    if (temp == null)
                        return null;
                    else
                        node = temp;
                }
                else
                {
                    EventNode temp = MinValueNode(node.Right);
                    node.Data = temp.Data;
                    node.Right = DeleteRec(node.Right, temp.Data);
                }
            }

            if (node == null) return null;

            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
            int balance = GetBalance(node);

            // Rebalancing cases
            if (balance > 1 && GetBalance(node.Left) >= 0)
                return RightRotate(node);

            if (balance > 1 && GetBalance(node.Left) < 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            if (balance < -1 && GetBalance(node.Right) <= 0)
                return LeftRotate(node);

            if (balance < -1 && GetBalance(node.Right) > 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private EventNode MinValueNode(EventNode node)
        {
            EventNode current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

    }
}
