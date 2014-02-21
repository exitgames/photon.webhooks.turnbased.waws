// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCreateController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;

    using Models;
    using DataAccess;


    public class GameCreateController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(GameCreateRequest request, string appId)
        {
            string message;
            if (!IsValid(request, out message))
            {
                return new ErrorResponse { Message = message };
            }

            if (Redis.Exists(string.Format("{0}_{1}", appId, request.GameId)))
            {
                return new ErrorResponse { Message = "Game already exists." };
            }

            Redis.Set(string.Format("{0}_{1}", appId, request.GameId), string.Empty);

            return new OkResponse();
        }

        private static bool IsValid(GameCreateRequest request, out string message)
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