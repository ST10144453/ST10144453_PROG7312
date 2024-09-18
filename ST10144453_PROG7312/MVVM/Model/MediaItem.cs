//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
namespace ST10144453_PROG7312.MVVM.Model
{
    //============== Class: MediaItem ==============//
    /// <summary>
    /// This class makes the object representation of a media item.
    /// </summary>
    public class MediaItem
    {
        //++++++++++++++ Declarations ++++++++++++++//
        /// <summary>
        /// This property holds the base64 string of the media item.
        /// </summary>
        public string Base64String { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public bool IsText { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public bool IsDocument { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public bool IsPdf { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public bool IsWord { get; set; }

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public string ImageFormat { get; set; } 
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//