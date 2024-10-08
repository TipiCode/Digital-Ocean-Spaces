﻿using Tipi.Tools.Services.Models;

namespace Tipi.Tools.Services.Interfaces
{
    /// <summary>
    /// Interface <c>IDoSpaces</c> serves as an interace to implement the Class <c>DoSpaces</c>,
    /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces">See More</see>
    /// </summary>
    /// <remarks>
    /// Exposes methods to interact with the S3 service.
    /// </remarks>
    public interface IDoSpaces
    {
        /// <summary>
        /// This method creates upload a new file to spaces
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/methods#upload-file-async">See More</see>.
        /// </summary>
        /// <remarks>
        /// Upload new file and return file url
        /// </remarks>
        /// <param name="file">Base64 file</param>
        /// <param name="folderName">Folder where to upload the image</param>
        /// <returns>
        /// Returns an <c>UploadResult</c> containing the file url.
        /// </returns>
        Task<UploadResult> UploadFileAsync(string file, string folderName);

        /// <summary>
        /// This method delete an existing file
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/methods#delete-file-async">See More</see>.
        /// </summary>
        /// <remarks>
        /// Delete an existing file
        /// </remarks>
        /// <param name="fileUrl">Url of the file or file name</param>
        /// <returns>
        /// Returns an <c>UploadResult</c> containing the operation result.
        /// </returns>
        Task DeleteFileAsync(string fileUrl);

        /// <summary>
        /// This method Updates an existing file
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/methods#update-file-async">See More</see>.
        /// </summary>
        /// <remarks>
        /// Update an existing file
        /// </remarks>
        /// <param name="file">Base64 file</param>
        /// <param name="fileUrl">Url of the image to update</param>
        /// <returns>
        /// Returns an <c>UploadResult</c> containing the operation result.
        /// </returns>
        Task UpdateFileAsync(string file, string fileUrl);
    }
}
