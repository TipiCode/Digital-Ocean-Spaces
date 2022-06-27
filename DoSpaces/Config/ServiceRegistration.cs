using Microsoft.Extensions.DependencyInjection;
using Tipi.Tools.Services.Interfaces;

namespace Tipi.Tools.Services.Config
{
    /// <summary>
    /// Static class to provide access to service registrations,
    /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces">See More</see>
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// This method configures the <c>DoSpaces</c> class for Dependency Injection inside .Net Core, 
        /// <see href="https://docs.codingtipi.com/docs/toolkit/dospaces">See More</see>.
        /// </summary>
        /// <remarks>
        /// Configure Dependency Ijection.
        /// </remarks>
        /// <param name="services">App Service collection.</param>
        /// <param name="accessKey">Your access key provided.</param>
        /// <param name="secretKey">Your secret key provided.</param>
        /// <param name="bucketName">Spaces Bucket name .</param>
        /// <param name="root">Folders path.</param>
        /// <param name="endpointUrl">Spaces url path.</param>
        /// <param name="endpoint">Spaces url.</param>
        public static void ConfigureDoSpaces(this IServiceCollection services, string accessKey, string secretKey, string bucketName, string root, string endpointUrl, string endpoint)
        {
            //S3 Storage Config
            services.AddTransient<IDoSpaces, DoSpaces>();
            var optionDictionary = new Dictionary<string, string>
            {
                { "AccessKey", accessKey },
                { "SecretKey", secretKey },
                { "BucketName", bucketName },
                { "Root", root },
                { "EndpointUrl", endpointUrl },
                { "Endpoint", endpoint }
            };

            services.AddSingleton(new S3BucketOptions(optionDictionary));
        }

    }
}
