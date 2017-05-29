// ------------------------------------------------------------------------------------------------
//  <copyright file="Global.asax.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased
{
    using System;
    using System.Configuration;
    using System.Web.Http;

    using Amazon;
    using Amazon.DynamoDBv2;
    using Amazon.S3;

    using Microsoft.WindowsAzure.Storage;

    using Photon.Webhooks.Turnbased.DataAccess;

    using ServiceStack.Redis;

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IDataAccess DataAccess;

        public static CloudStorageAccount CloudStorageAccount;


        public static PooledRedisClientManager PooledRedisClientManager;

        public static AmazonS3Client AmazonS3Client;
        // ReSharper disable once InconsistentNaming
        public static AmazonDynamoDBClient AmazonDynamoDBClient;

        protected void Application_Start()
        {
            if (ConfigurationManager.AppSettings["DataAccess"].Equals("Azure", StringComparison.OrdinalIgnoreCase))
            {
                CloudStorageAccount =
                    CloudStorageAccount.Parse(
                        string.Format(
                            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                            ConfigurationManager.AppSettings["AzureAccountName"],
                            ConfigurationManager.AppSettings["AzureAccountKey"]));

                DataAccess = new Azure();
            }
            else if (ConfigurationManager.AppSettings["DataAccess"].Equals("Redis", StringComparison.OrdinalIgnoreCase))
            {
                PooledRedisClientManager = new PooledRedisClientManager(
                  string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisPassword"]) ?
                      string.Format("{0}:{1}", ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"]) :
                      string.Format("{0}@{1}:{2}", ConfigurationManager.AppSettings["RedisPassword"], ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"]));

                DataAccess = new Redis();
            }
            else if (ConfigurationManager.AppSettings["DataAccess"].Equals("Amazon", StringComparison.OrdinalIgnoreCase))
            {
                var regionEndpoint = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
                AmazonS3Client = new AmazonS3Client(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"], regionEndpoint);

                AmazonDynamoDBClient = new AmazonDynamoDBClient(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"]);

                DataAccess = new AmazonDataAccess();
            }
            else
            {
                DataAccess = new NotImplemented();
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}