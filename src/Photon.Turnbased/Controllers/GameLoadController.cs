// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLoadController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;
    using Newtonsoft.Json;
    using Models;
    using log4net;

    public class GameLoadController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        #region Public Methods and Operators

        public dynamic Post(GameCreateRequest request, string appId)
        {
            appId = appId.ToLowerInvariant();

            string message;
            if (!IsValid(request, out message))
            {
                var errorResponse = new ErrorResponse { Message = message };
                if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(errorResponse));
                return errorResponse;
            }

            if (log.IsDebugEnabled) log.DebugFormat("{0} - {1}", Request.RequestUri, JsonConvert.SerializeObject(request));

            dynamic response = GameCreateController.GameLoad(request, appId);
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        private static bool IsValid(GameCreateRequest request, out string message)
        {
            if (request == null)
            {
                message = "Request data is missing.";
                return false;
            }

            if (string.IsNullOrEmpty(request.GameId))
            {
                message = "Missing GameId.";
                return false;
            }

            if (string.IsNullOrEmpty(request.UserId))
            {
                message = "Missing UserId.";
                return false;
            }

            message = "";
            return true;
        }

        #endregion
    }
}