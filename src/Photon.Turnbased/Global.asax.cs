// ------------------------------------------------------------------------------------------------
//  <copyright file="Global.asax.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased
{
    using System;
    using System.Web.Http;
    using System.Configuration;

    using DataAccess;

    using ServiceStack.Redis;

    using Amazon;
    using Amazon.S3;

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IDataAccess DataAccess;

        public static PooledRedisClientManager PooledRedisClientManager;

        public static AmazonS3Client AmazonS3Client;

        protected void Application_Start()
        {
            if (ConfigurationManager.AppSettings["DataAccess"].Equals("Azure", StringComparison.OrdinalIgnoreCase))
            {
                DataAccess = new Azure();
            }
            else if (ConfigurationManager.AppSettings["DataAccess"].Equals("Redis", StringComparison.OrdinalIgnoreCase))
            {
                PooledRedisClientManager = new PooledRedisClientManager(
                  string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisPassword"]) ?
                      string.Format("{0}:{1}", ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"]) :
                      string.Format("{0}@{1}:{2}", ConfigurationManager.AppSettings["RedisPassword"], ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"])
                  );

                DataAccess = new Redis();
            }
            if (ConfigurationManager.AppSettings["DataAccess"].Equals("Amazon", StringComparison.OrdinalIgnoreCase))
            {
                var regionEndpoint = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWSRegion"]);
                AmazonS3Client = new AmazonS3Client(ConfigurationManager.AppSettings["AWSAccessKey"], ConfigurationManager.AppSettings["AWSSecretKey"], regionEndpoint);

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
