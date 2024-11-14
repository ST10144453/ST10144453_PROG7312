using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class MediaNode
    {
        public MediaItem Data { get; set; }
        public List<MediaNode> Children { get; set; }
        public MediaNode Parent { get; set; }

        public MediaNode(MediaItem data)
        {
            Data = data;
            Children = new List<MediaNode>();
        }
    }
}
