// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLeaveController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;
    using Models;
    using log4net;
    using Newtonsoft.Json;

    public class GameLeaveController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        #region Public Methods and Operators

        public dynamic Post(GameLeaveRequest request, string appId)
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

            if (request.IsInactive)
            {
                if (request.ActorNr > 0)
                {
                    WebApiApplication.DataAccess.GameInsert(appId, request.UserId, request.GameId, request.ActorNr);
                }
            }
            else
            {
                WebApiApplication.DataAccess.GameDelete(appId, request.UserId, request.GameId);
            }

            var okResponse = new OkResponse();
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(okResponse));
            return okResponse;
        }

        private static bool IsValid(GameLeaveRequest request, out string message)
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