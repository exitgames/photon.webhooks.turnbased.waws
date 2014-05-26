// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased
{
    using System.Web.Http;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    using Models;

    using log4net;

    using Newtonsoft.Json;

    public static class WebApiConfig
    {
        #region Public Methods and Operators

        public static void Register(HttpConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();

            config.Filters.Add(new UnhandledExceptionAttribute());

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(name: "ApiByAppId", routeTemplate: "{appId}/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.Indent = true;

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        #endregion
    }


    public class UnhandledExceptionAttribute : ExceptionFilterAttribute
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        public override void OnException(HttpActionExecutedContext context)
        {
            log.Error(context.Exception);

            var response = new ErrorResponse {Message = context.Exception.Message};
            if(log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));

            context.Response = context.Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}