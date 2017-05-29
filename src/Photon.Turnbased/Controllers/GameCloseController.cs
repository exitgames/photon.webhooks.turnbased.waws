// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCloseController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;
    using Newtonsoft.Json;
    using Models;
    using log4net;

    public class GameListItem
    {
        public int ActorNr;
        public dynamic Properties;
    }

    public class GameCloseController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        #region Public Methods and Operators

        public dynamic Post(GameCloseRequest request, string appId)
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

            if (request.State == null)
            {
                if (request.ActorCount > 0)
                {
                    var errorResponse = new ErrorResponse { Message = "Missing State." };
                    if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(errorResponse));
                    return errorResponse;
                }

                WebApiApplication.DataAccess.StateDelete(appId, request.GameId);

                var okResponse = new OkResponse();
                if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(okResponse));
                return okResponse;
            }

            foreach (var actor in request.State.ActorList)
            {
                //var listProperties = new ListProperties() { ActorNr = (int)actor.ActorNr, Properties = request.State.CustomProperties };
                //WebApiApplication.DataAccess.GameInsert(appId, (string)actor.UserId, request.GameId, (string)JsonConvert.SerializeObject(listProperties));
                WebApiApplication.DataAccess.GameInsert(appId, (string)actor.UserId, request.GameId, (int)actor.ActorNr);
            }                

            //deprecated
            if (request.State2 != null)
            {
                foreach (var actor in request.State2.ActorList)
                {
                    WebApiApplication.DataAccess.GameInsert(appId, (string)actor.UserId, request.GameId, (int)actor.ActorNr);
                }
            }

            var state = (string)JsonConvert.SerializeObject(request.State);
            WebApiApplication.DataAccess.StateSet(appId, request.GameId, state);

            var response = new OkResponse();
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }

        private static bool IsValid(GameCloseRequest request, out string message)
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

            message = "";
            return true;
        }

        #endregion
    }
}