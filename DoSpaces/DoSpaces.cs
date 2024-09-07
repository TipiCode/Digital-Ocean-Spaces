using Amazon.S3;
using Amazon.S3.Model;
using Tipi.Tools.Services.Config;
using Tipi.Tools.Services.Helpers;
using Tipi.Tools.Services.Interfaces;
using Tipi.Tools.Services.Models;

namespace Tipi.Tools.Services
{
    /// <summary>
    /// Class <c>DoSpaces</c>,
    /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces">See More</see>
    /// </summary>
    /// <remarks>
    /// Exposes methods to interact with the S3 service.
    /// </remarks>
    public class DoSpaces : IDoSpaces
    {

        private readonly S3BucketOptions _options;
        private readonly AmazonS3Client _client;

        /// <summary>
        /// Contructor with supported dependency injection
        /// </summary>
        /// <param name="options">Options to support the Aws S3 client.</param>
        /// <param name="client">Amazon S3 client</param>
        public DoSpaces(S3BucketOptions options, 
            AmazonS3Client client)
        {
            _options = options;
            _client = client;
        }

        #region Public Methods
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
        public async Task<UploadResult> UploadFileAsync(string file, string folderName)
        {
            string format = ImageTools.GetFileExtention(file.Split("|")[0]);
            file = ImageTools.CleanBase64Image(file.Split("|")[0]);

            try
            {
                // Name for the file will be a random Guid
                var name = Guid.NewGuid().ToString().Split('-')[0];
                // Converting the file to a byte array to save it with the memory stream
                byte[] bytes = Convert.FromBase64String(file);

                var rootPath = "";
                if (!String.IsNullOrEmpty(_options.Root))
                    rootPath = $"{_options.Root}";

                
                var filePath = rootPath + @"/" + folderName;
                var key = name + format;
                var cleanedBody = _options.EndpointUrl.Replace("https://", "")
                    .Replace(_options.Region, "")
                    .Replace("/", "")
                    .Substring(1);

                using var stream = new MemoryStream(bytes);

                // Upload the stream
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = $"{filePath}/{key}",
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead
                };

                var response = await _client.PutObjectAsync(uploadRequest);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return new UploadResult(true, Path: $"https://{_options.BucketName}.{_options.Region}.{(_options.UseCdn ? "cdn." : String.Empty)}{cleanedBody}/{filePath}/{key}");
                else
                    return new UploadResult(false, Error: $"An error ocurred during the upload process, HTTP CODE: {response.HttpStatusCode}");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                return new UploadResult(false, Error: "S3 Error --- " + e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return new UploadResult(false, Error: "Server Error --- " + e);
            }
        }

        /// <summary>
        /// This method Updates an existing file
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/methods#update-file-async">See More</see>.
        /// </summary>
        /// <remarks>
        /// Update an existing file
        /// </remarks>
        /// <param name="fileToUpdate">Base64 file</param>
        /// <param name="fileUrl">Url of the image to update</param>
        public async Task UpdateFileAsync(string fileToUpdate, string fileUrl)
        {
            fileToUpdate = ImageTools.CleanBase64Image(fileToUpdate.Split("|")[0]);

            try
            {
                // Converting the file to a byte array to save it with the memory stream
                byte[] bytes = Convert.FromBase64String(fileToUpdate);

                var key = fileUrl.Split("/").Last();
                var filePath = fileUrl.Replace(_options.BucketName, "")
                    .Replace(_options.Region, "")
                    .Replace(key, "")
                    .Replace("cdn", "")
                    .Replace("https://", "")
                    .Replace("digitaloceanspaces.com", "")
                    .Replace(".", "")
                    .Substring(1);

                using var stream = new MemoryStream(bytes);

                // Upload the stream
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = $"{filePath}{key}",
                    InputStream = stream,
                    CannedACL = S3CannedACL.PublicRead
                };

                await _client.PutObjectAsync(uploadRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }



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
        public async Task DeleteFileAsync(string fileUrl)
        {

            try
            {

                var key = fileUrl.Split("/").Last();
                var filePath = fileUrl.Replace(_options.BucketName, "")
                    .Replace(_options.Region, "")
                    .Replace(key, "")
                    .Replace("cdn", "")
                    .Replace("https://", "")
                    .Replace("digitaloceanspaces.com", "")
                    .Replace(".", "")
                    .Substring(1);

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = $"{filePath}{key}"
                };

                await _client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
        #endregion
    }
}
