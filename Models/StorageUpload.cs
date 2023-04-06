using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Models
{
    public class StorageUpload
    {


        private static readonly RegionEndpoint region = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;
        


        public static async Task UploadFileAsync(Stream FileStream, string bucketName, string keyName)
        {
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };

            var token = new BasicAWSCredentials("AKIA5SKGJHXSBC7RJX53", "BrjwUxycpdKNFFVDU1de9+oAOh3ZZupw2J6M7NlL");
            AmazonS3Client clientInfo = new AmazonS3Client(token, s3Config);
            s3Client = clientInfo;
            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(FileStream, bucketName, keyName);
        }
    }
}
