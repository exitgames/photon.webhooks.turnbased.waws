// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLeaveController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;

    using Models;

    public class GameLeaveController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(GameLeaveRequest request, string appId)
        {
            string message;
            if (!IsValid(request, out message))
            {
                return new ErrorResponse { Message = message };
            }

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

            return new OkResponse();
        }

        private static bool IsValid(GameLeaveRequest request, out string message)
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