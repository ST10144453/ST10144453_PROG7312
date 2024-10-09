using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class TagsModel
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        public TagsModel() 
        {
            TagId = Guid.NewGuid();
        }
    }
}
