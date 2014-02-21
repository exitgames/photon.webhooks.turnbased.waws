// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased
{
    using System.Web.Http;

    public static class WebApiConfig
    {
        #region Public Methods and Operators

        public static void Register(HttpConfiguration config)
        {
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
}