
namespace Tipi.Tools.Services.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ImageTools
    {
        /// <summary>
        /// Clear base64 image string
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Clean base64</returns>
        public static string CleanBase64Image(string file)
        {
            var splited = file.Split(',');
            return splited[1];
        }

        /// <summary>
        /// Get base64 image extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Image extension</returns>
        public static string GetFileExtention(string file)
        {
            var splited = file.Split(',');
            var extention = '.' + splited[0].Split('/')[1].Split(';')[0];
            return extention;
        }
    }
}
