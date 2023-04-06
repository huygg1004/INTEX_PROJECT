using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intex_app.Services
{
    public class S3 : S3interface
    {
        private readonly AmazonS3Client s3Client;
        private const string BUCKET_NAME = "arn:aws:s3:us-east-1:932692180452:accesspoint/imageupload";
        private const string FOLDER_NAME = "Photos";
        private const double DURATION = 25;

        //public S3()
        //{
        //    s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
        //}
        public S3()
        {
            var s3Config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };

            var token = new BasicAWSCredentials("AKIA5SKGJHXSBC7RJX53", "BrjwUxycpdKNFFVDU1de9+oAOh3ZZupw2J6M7NlL");
            AmazonS3Client clientInfo = new AmazonS3Client(token, s3Config);
            s3Client = clientInfo;
        }


        public async Task<string> AddItem(IFormFile file, string readerName)
        {
            //Reading in the file when the function gets called
            string fileName = file.FileName;
            string objectKey = $"{FOLDER_NAME}/{fileName}"; //Knows where to upload the file

           

            using (Stream fileToUpload = file.OpenReadStream())
            {
                var putObjectRequest = new PutObjectRequest();
                putObjectRequest.BucketName = BUCKET_NAME;
                putObjectRequest.Key = objectKey;
                putObjectRequest.InputStream = fileToUpload;
                putObjectRequest.ContentType = file.ContentType;

                var response = await s3Client.PutObjectAsync(putObjectRequest);
                

                return GeneratePreSignedURL(objectKey);

            }
        }

        private string GeneratePreSignedURL(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BUCKET_NAME,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(DURATION)
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }
    }
}
