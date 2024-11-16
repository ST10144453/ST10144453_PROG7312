using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class EventNode
    {
        public EventModel Data { get; set; }
        public EventNode Left { get; set; }
        public EventNode Right { get; set; }
        public int Height { get; set; }

        public EventNode(EventModel data)
        {
            Data = data;
            Height = 1;
        }
    }
}
