// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetGameListController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;

    using Models;

    using log4net;

    using Newtonsoft.Json;

    using ServiceStack.Text;

    public class GetGameListController : ApiController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        #region Public Methods and Operators

        public dynamic Post(GetGameListRequest request, string appId)
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

            var list = new Dictionary<string, object>();

            foreach (var pair in WebApiApplication.DataAccess.GameGetAll(appId, request.UserId))
            {
                // exists - save result in list
                //if (WebApiApplication.DataAccess.StateExists(appId, pair.Key))
                var stateJson = WebApiApplication.DataAccess.StateGet(appId, pair.Key);
                if (stateJson != null)
                {
                    dynamic customProperties = null;
                    if (stateJson != string.Empty)
                    {
                        var state = JsonConvert.DeserializeObject<dynamic>(stateJson);
                        customProperties = state.CustomProperties;
                    }

                    var gameListItem = new GameListItem(){ ActorNr = int.Parse(pair.Value), Properties = customProperties };

                    list.Add(pair.Key, gameListItem);
                }
                // not exists - delete
                else
                {
                    WebApiApplication.DataAccess.GameDelete(appId, request.UserId, pair.Key);
                }
            }

            var getGameListResponse = new GetGameListResponse { Data = list };
            if (log.IsDebugEnabled) log.Debug(JsonConvert.SerializeObject(getGameListResponse));
            return getGameListResponse;
        }

        private static bool IsValid(GetGameListRequest request, out string message)
        {
            if (request == null)
            {
                message = "Request data is missing.";
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