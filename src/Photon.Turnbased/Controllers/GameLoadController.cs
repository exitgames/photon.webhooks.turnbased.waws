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

    public class GameLoadController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(GameLoadRequest request, string appId)
        {
            string message;
            if (!IsValid(request, out message))
            {
                return new ErrorResponse { Message = message };
            }

            var stateJson = WebApiApplication.DataAccess.StateGet(appId, request.GameId);

            if (!string.IsNullOrEmpty(stateJson))
            {
                return new GameLoadResponse { State = JsonConvert.DeserializeObject(stateJson) };
            }

            if (request.ActorNr == 0)
            {
                return new OkResponse();
            }

            return new ErrorResponse { Message = "GameId not Found." };
        }

        private static bool IsValid(GameLoadRequest request, out string message)
        {
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