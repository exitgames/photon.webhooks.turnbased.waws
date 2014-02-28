﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCloseController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;

    using Newtonsoft.Json;

    using Models;

    public class GameCloseController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(GameCloseRequest request, string appId)
        {
            string message;
            if (!IsValid(request, out message))
            {
                return new ErrorResponse { Message = message };
            }

            if (request.State == null)
            {
                if (request.ActorCount > 0)
                {
                    return new ErrorResponse { Message = "Missing State." };
                }

                WebApiApplication.DataAccess.StateDelete(appId, request.GameId);

                return new OkResponse();
            }

            if (request.State2 != null)
            {
                foreach (var actor in request.State2.ActorList)
                {
                    WebApiApplication.DataAccess.GameInsert(appId, (string)actor.UserId, request.GameId, (int)actor.ActorNr);
                }
            }

            WebApiApplication.DataAccess.StateSet(appId, request.GameId, (string)JsonConvert.SerializeObject(request.State));

            return new OkResponse();
        }

        private static bool IsValid(GameCloseRequest request, out string message)
        {
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