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
    using System.Linq;

    using Amazon.S3;
    using Amazon.S3.Model;
    using Amazon.DynamoDBv2.Model;
    
    public class AmazonDataAccess : IDataAccess
    {
        private const string HashKey = "UserId";
        private const string RangeKey = "GameId";
        private const string ActorNr = "ActorNr";

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

            try
            {
                using (var response = WebApiApplication.AmazonS3Client.GetObject(getObjectRequest))
                {
                    using (var reader = new StreamReader(response.ResponseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            //Amazon throws exception if file not found
            catch (AmazonS3Exception)
            {
                return null;
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
            WebApiApplication.AmazonDynamoDBClient.PutItem(
                new PutItemRequest
                {
                    TableName = ConfigurationManager.AppSettings["TableName"],
                    Item = new Dictionary<string, AttributeValue>
                    {
                        {HashKey, new AttributeValue { S = string.Format("{0}_{1}", appId, key) } },
                        {RangeKey, new AttributeValue { S = gameId } },
                        {ActorNr, new AttributeValue { N = actorNr.ToString() } },
                    },
                }
            );
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            WebApiApplication.AmazonDynamoDBClient.DeleteItem(
                new DeleteItemRequest
                {
                    TableName = ConfigurationManager.AppSettings["TableName"],
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {HashKey, new AttributeValue { S = string.Format("{0}_{1}", appId, key) } },
                        {RangeKey, new AttributeValue { S = gameId } },
                    },
                }
            );
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            var request = new QueryRequest
            {
                TableName = ConfigurationManager.AppSettings["TableName"],
                KeyConditions = new Dictionary<string, Condition>
                {
                    {
                        HashKey,
                        new Condition
                        {
                            ComparisonOperator = "EQ",
                            AttributeValueList = new List<AttributeValue>()
                            {
                                new AttributeValue {S = string.Format("{0}_{1}", appId, key)}
                            }
                        }
                    }
                }
            };

            var response = WebApiApplication.AmazonDynamoDBClient.Query(request);

            return response.Items.ToDictionary(item => item[RangeKey].S, item => item[ActorNr].N);
        }
    }
}