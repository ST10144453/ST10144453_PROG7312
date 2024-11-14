//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// This property holds the file name of the media item.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// This property holds the file path of the media item.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// This property holds the file size of the media item.
        /// </summary>
        public long FileSize { get; set; }

        public string Id;

        /// <summary>
        /// This method creates a MediaItem object from a file path.
        /// </summary>
        public static async Task<MediaItem> FromFileAsync(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var mediaItem = new MediaItem
                {
                    FileName = fileInfo.Name,
                    FilePath = filePath,
                    FileSize = fileInfo.Length
                };

                // Use System.IO.File to read the bytes asynchronously
                // Use System.IO.FileStream to read the bytes asynchronously
                byte[] fileBytes;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    fileBytes = new byte[stream.Length];
                    await stream.ReadAsync(fileBytes, 0, (int)stream.Length);
                }
                mediaItem.Base64String = Convert.ToBase64String(fileBytes);

                // Set media type based on extension
                string extension = fileInfo.Extension.ToLower();
                mediaItem.IsImage = new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(extension);
                mediaItem.IsPdf = extension == ".pdf";
                mediaItem.IsWord = new[] { ".doc", ".docx" }.Contains(extension);
                mediaItem.IsText = extension == ".txt";

                if (mediaItem.IsImage)
                {
                    mediaItem.ImageFormat = extension.TrimStart('.');
                }

                return mediaItem;
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//