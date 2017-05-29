// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameJoinController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;
    using Models;
    using log4net;
    using Newtonsoft.Json;


    public class GameJoinController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        #region Public Methods and Operators

        public dynamic Post(GameLeaveRequest request, string appId)
        {
            if (log.IsDebugEnabled) log.DebugFormat("{0} - {1}", Request.RequestUri, request == null ? "null" : JsonConvert.SerializeObject(request));

            var response = new OkResponse();
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        #endregion
    }
}