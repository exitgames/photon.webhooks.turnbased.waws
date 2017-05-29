// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GamePropertiesController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web.Http;

    using Models;

    using log4net;

    using Newtonsoft.Json;

    using Photon.Webhooks.Turnbased.PushNotifications;

    using ServiceStack.Text;
    
    public class GamePropertiesController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        private PushWoosh pushWoosh = new PushWoosh();

        #region Public Methods and Operators

        public dynamic Post(GamePropertiesRequest request, string appId)
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

            if (request.State != null)
            {
                var state = (string)JsonConvert.SerializeObject(request.State);
                WebApiApplication.DataAccess.StateSet(appId, request.GameId, state);

                var properties = request.Properties;
                object actorNrNext = null;
                if (properties != null)
                {
                    properties.TryGetValue("turn", out actorNrNext);
                }
                var userNextInTurn = string.Empty;
                foreach (var actor in request.State.ActorList)
                {
                    if (actorNrNext != null)
                    {
                        if (actor.ActorNr == actorNrNext)
                        {
                            userNextInTurn = (string)actor.UserId;
                        }
                    }
                    WebApiApplication.DataAccess.GameInsert(appId, (string)actor.UserId, request.GameId, (int)actor.ActorNr);
                }

                if (!string.IsNullOrEmpty(userNextInTurn))
                {
                    var notificationContent = new Dictionary<string, string>
                                                  {
                                                      { "en", "{USERNAME} finished. It's your turn." },
                                                      { "de", "{USERNAME} hat seinen Zug gemacht. Du bist dran." },
                                                  };
                    pushWoosh.RequestPushNotification(notificationContent, request.Username, "UID2", userNextInTurn, appId);
                }
            }

            var response = new OkResponse();
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(response));
            return response;
        }


        private static bool IsValid(GamePropertiesRequest request, out string message)
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