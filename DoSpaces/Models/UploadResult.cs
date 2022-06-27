
namespace Tipi.Tools.Services.Models
{
    /// <summary>
    ///     Class representing the UploadResult object
    ///     <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/classes#uploadresult">See More</see>
    /// </summary>
    /// <param name="Success">Upload image status</param>
    /// <param name="Path">Image Url</param>
    /// <param name="Error">Error description if upload fail</param>
    public record UploadResult(bool Success, string Path = "", string Error = "");
}
