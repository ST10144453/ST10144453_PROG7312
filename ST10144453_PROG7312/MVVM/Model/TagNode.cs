using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    internal class TagNode
    {
        public TagsModel Data { get; set; }
        public List<TagNode> Children { get; set; }
        public TagNode Parent { get; set; }

        public TagNode(TagsModel data)
        {
            Data = data;
            Children = new List<TagNode>();
        }
    }
}
