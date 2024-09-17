using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ReportModel
    {
        /// <summary>
        /// Unique ID for report... will help when users functionality is added. 
        /// </summary>
        public Guid reportID { get; set; } 
        /// <summary>
        /// Name of the submitted report
        /// </summary>
       public string reportName {  get; set; }
        /// <summary>
        /// Date of report submitted (Automatically generated)
        /// </summary>
        public DateTime reportDate { get; set; }
        /// <summary>
        /// Location of the issue reported. 
        /// </summary>
        public string reportLocation { get; set; }
        /// <summary>
        /// Description of issue. 
        /// </summary>
        public string reportDescription { get; set; }
        /// <summary>
        /// Selected category of report
        /// </summary>
        public string reportCategory { get; set; }
        /// <summary>
        /// List of categories for report (Will be used for dropdown list)
        /// </summary>
        public List<string> Categories { get; set; }
        /// <summary>
        /// List of media files attached to report. The media files will be stored in a folder and the path will be stored here.
        /// </summary>
        public List<MediaItem> Media { get; set; }

        public bool HasPdf
        {
            get { return Media != null && Media.Any(item => item.IsPdf); }
        }

        public bool HasWord
        {
            get { return Media != null && Media.Any(item => item.IsWord); }
        }
        public ReportModel()
        {
            reportID = Guid.NewGuid();
            reportDate = DateTime.Now;
            Categories = new List<string> { "Category 1", "Category 2", "Category 3" };
            Media = new List<MediaItem>();
        }

    }
}

