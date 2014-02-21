namespace Photon.Webhooks.Turnbased
{
    using System.Web.Http;
    using System.Configuration;

    using ServiceStack.Redis;
    
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static PooledRedisClientManager PooledRedisClientManager;

        protected void Application_Start()
        {
            PooledRedisClientManager = new PooledRedisClientManager(
                   string.IsNullOrEmpty(ConfigurationManager.AppSettings["RedisPassword"]) ?
                       string.Format("{0}:{1}", ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"]) :
                       string.Format("{0}@{1}:{2}", ConfigurationManager.AppSettings["RedisPassword"], ConfigurationManager.AppSettings["RedisUrl"], ConfigurationManager.AppSettings["RedisPort"])
                   );

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
