// ------------------------------------------------------------------------------------------------
//  <copyright file="Amazon.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using Amazon.S3.Model;
    
    public class AmazonDataAccess : IDataAccess
    {
        public bool StateExists(string appId, string key)
        {
            var listObjectsRequest = new ListObjectsRequest
                                        {
                                            BucketName = ConfigurationManager.AppSettings["BucketName"],
                                            Prefix = string.Format("{0}/{1}", appId, key),
                                        };

            var response =  WebApiApplication.AmazonS3Client.ListObjects(listObjectsRequest);

            return response.S3Objects.Count == 1;
        }

        public void StateSet(string appId, string key, string state)
        {
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = ConfigurationManager.AppSettings["BucketName"],
                Key = string.Format("{0}/{1}", appId, key),
                ContentBody = state,
            };

            WebApiApplication.AmazonS3Client.PutObject(putObjectRequest);
        }

        public string StateGet(string appId, string key)
        {
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = ConfigurationManager.AppSettings["BucketName"],
                Key = string.Format("{0}/{1}", appId, key),
            };

            using (var response = WebApiApplication.AmazonS3Client.GetObject(getObjectRequest))
            {
                using (var reader = new StreamReader(response.ResponseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void StateDelete(string appId, string key)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = ConfigurationManager.AppSettings["BucketName"],
                Key = string.Format("{0}/{1}", appId, key)
            };

            WebApiApplication.AmazonS3Client.DeleteObject(deleteObjectRequest);
        }

        public void GameInsert(string appId, string key, string gameId, int actorNr)
        {
            //Will be implemented with DynamoDB
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            //Will be implemented with DynamoDB        
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            //Will be implemented with DynamoDB
            
            return new Dictionary<string, string>();
        }
    }
}