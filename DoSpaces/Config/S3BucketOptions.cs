using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tipi.Tools.Services.Config
{
    /// <summary>
    /// Class used to configure the <c>DoSpaces</c> class, 
    /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces/classes#bucket-options">See More</see>
    /// </summary>
    public class S3BucketOptions
    {
        /// <summary>
        /// Your access key provided
        /// </summary>
        public string AccessKey { get; }
        /// <summary>
        /// Your secret key provided
        /// </summary>
        public string SecretKey { get; }
        /// <summary>
        /// Spaces Bucket name 
        /// </summary>
        public string BucketName { get; }
        /// <summary>
        /// Folders path
        /// </summary>
        public string Root { get; }
        /// <summary>
        /// Spaces url path
        /// </summary>
        public string EndpointUrl { get; }
        /// <summary>
        /// Region
        /// </summary>
        public string Region { get; }
        /// <summary>
        /// Region
        /// </summary>
        public bool UseCdn { get; } = default!;

        /// <summary>
        /// Constructor to initialize the <c>S3BucketOptions</c> object.
        /// </summary>
        /// <remarks>
        /// Takes as parameters, the Port provided by your hosting.
        /// </remarks>
        /// <param name="values">Values to configure S3BucketOptions.</param>
        public S3BucketOptions(Dictionary<string, string> values)
        {
            AccessKey = values["AccessKey"];
            SecretKey = values["SecretKey"];
            BucketName = values["BucketName"];
            Root = values["Root"];
            EndpointUrl = values["EndpointUrl"];
            Region = values["Region"];
            UseCdn = bool.Parse(values["UseCdn"]);
        }
    }
}
