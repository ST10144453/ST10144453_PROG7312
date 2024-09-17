using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class MediaItem
    {
        public string Base64String { get; set; }
        public bool IsImage { get; set; }
        public bool IsText { get; set; } // Add this for text files
        public bool IsDocument { get; set; } // Add this for documents
        public bool IsPdf { get; set; } // Add this property
        public bool IsWord { get; set; }

        public string ImageFormat { get; set; } // e.g., "jpg", "jpeg", "png"



    }


}
