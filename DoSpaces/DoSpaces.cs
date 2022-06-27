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
        /// <summary>
        /// 
        /// </summary>
        public S3BucketOptions Options { get; }
        /// <summary>
        /// 
        /// </summary>
        private static IAmazonS3 s3Client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsAccessor"></param>
        public DoSpaces(S3BucketOptions optionsAccessor)
        {
            Options = optionsAccessor;
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

            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = Options.EndpointUrl,
            };

            s3Client = new AmazonS3Client(Options.AccessKey, Options.SecretKey, s3ClientConfig);

            try
            {
                // Name for the file will be a random Guid
                var name = Guid.NewGuid().ToString();
                // Converting the file to a byte array to save it with the memory stream
                byte[] bytes = Convert.FromBase64String(file);
                using (s3Client)
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = Options.BucketName + @"/" + Options.Root + @"/" + folderName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = name + format
                    };
                    //request.StreamTransferProgress += RequestProgress;
                    using (var ms = new MemoryStream(bytes))
                    {
                        request.InputStream = ms;
                        await s3Client.PutObjectAsync(request);
                    }
                    //request.StreamTransferProgress -= RequestProgress;
                }
                return new UploadResult(true, Path: $"https://{Options.BucketName}.{Options.Endpoint}/{Options.Root}/{folderName}/{name + format}");
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
        /// <param name="folderName">Folder where the uploaded image is saved</param>
        /// <param name="fileUrl">Url of the image to update</param>
        /// <returns>
        /// Returns an <c>UploadResult</c> containing the operation result.
        /// </returns>
        public async Task<UploadResult> UpdateFileAsync(string fileToUpdate, string folderName, string fileUrl)
        {
            fileToUpdate = ImageTools.CleanBase64Image(fileToUpdate.Split("|")[0]);

            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = Options.EndpointUrl,
            };

            // Split the Url to know the file name later on, file name is on the last index of the array.
            var fileName = fileUrl.Split('/');

            s3Client = new AmazonS3Client(Options.AccessKey, Options.SecretKey, s3ClientConfig);

            try
            {

                // Converting the file to a byte array to save it with the memory stream
                byte[] bytes = Convert.FromBase64String(fileToUpdate);

                using (s3Client)
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = Options.BucketName + @"/" + Options.Root + @"/" + folderName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = fileName[fileName.Length - 1]
                    };
                    //request.StreamTransferProgress += RequestProgress;
                    using (var ms = new MemoryStream(bytes))
                    {
                        request.InputStream = ms;
                        await s3Client.PutObjectAsync(request);
                    }
                    //request.StreamTransferProgress -= RequestProgress;
                }
                return new UploadResult(true);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                return new UploadResult(false, Error: "S3 Error --- " + e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                if (e.Message.Contains("disposed")) { };
                return new UploadResult(false, Error: "Server Error --- " + e);
            }
        }

        

        /// <summary>
        /// This method delete an existing file
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/methods#delete-file-async">See More</see>.
        /// </summary>
        /// <remarks>
        /// Delete an existing file
        /// </remarks>
        /// <param name="url">Url of the file or file name</param>
        /// <param name="folder">Folder where the uploaded image is saved</param>
        /// <returns>
        /// Returns an <c>UploadResult</c> containing the operation result.
        /// </returns>
        public async Task<UploadResult> DeleteFileAsync(string url, string folder)
        {
            var fileName = url.Split('/');
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = Options.EndpointUrl,
            };
            s3Client = new AmazonS3Client(Options.AccessKey, Options.SecretKey, s3ClientConfig);

            try
            {
                using (s3Client)
                {
                    var request = new DeleteObjectRequest
                    {
                        BucketName = Options.BucketName + @"/" + Options.Root + @"/" + folder,
                        Key = fileName[fileName.Length - 1]
                    };
                    await s3Client.DeleteObjectAsync(request);
                }
                return new UploadResult(true);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                return new UploadResult(false, Error: "S3 Error --- " + e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                if (e.Message.Contains("disposed")) { };
                return new UploadResult(false, Error: "Server Error --- " + e);
            }
        }
        #endregion
    }
}
