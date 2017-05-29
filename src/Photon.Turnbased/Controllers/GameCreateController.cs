// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCreateController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;
    using Models;
    using log4net;
    using Newtonsoft.Json;

    public class GameCreateController : ApiController
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

            dynamic response;
            if (!string.IsNullOrEmpty(request.Type) && request.Type == "Load")
            {
                response = GameLoad(request, appId);
            }
            else
            {
                response = GameCreate(request, appId);
            }

            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        private static dynamic GameCreate(GameCreateRequest request, string appId)
        {
            dynamic response;
            if (WebApiApplication.DataAccess.StateExists(appId, request.GameId))
            {
                response = new ErrorResponse { Message = "Game already exists." };
                return response;
            }

            if (request.CreateOptions == null)
            {
                WebApiApplication.DataAccess.StateSet(appId, request.GameId, string.Empty);
            }
            else
            {
                WebApiApplication.DataAccess.StateSet(appId, request.GameId, (string)JsonConvert.SerializeObject(request.CreateOptions));
            }

            response = new OkResponse();
            return response;
        }

        public static dynamic GameLoad(GameCreateRequest request, string appId)
        {
            dynamic response;
            string stateJson = string.Empty;
            stateJson = WebApiApplication.DataAccess.StateGet(appId, request.GameId);

            if (!string.IsNullOrEmpty(stateJson))
            {
                response = new GameLoadResponse { State = JsonConvert.DeserializeObject(stateJson) };
                return response;
            }

            //TBD - check how deleteIfEmpty works with createifnot exists
            if (stateJson == string.Empty)
            {
                WebApiApplication.DataAccess.StateDelete(appId, request.GameId);

                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Deleted empty state, app id {0}, gameId {1}", appId, request.GameId);
                }
            }

            if (request.CreateIfNotExists)
            {
                response = new OkResponse();
                return response;
            }

            response = new ErrorResponse { Message = "GameId not Found." };
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