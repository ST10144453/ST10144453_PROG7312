using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class AnnouncementNode
    {
        public AnnouncementModel Data { get; set; }
        public AnnouncementNode Left { get; set; }
        public AnnouncementNode Right { get; set; }
        public int Height { get; set; }

        public AnnouncementNode(AnnouncementModel data)
        {
            Data = data;
            Height = 1;
        }
    }
}
