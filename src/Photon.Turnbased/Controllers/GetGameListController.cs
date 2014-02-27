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

    public class GetGameListController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(GetGameListRequest request, string appId)
        {
            string message;
            if (!IsValid(request, out message))
            {
                return new ErrorResponse { Message = message };
            }

            var list = new Dictionary<string, string>();

            foreach (var pair in WebApiApplication.DataAccess.GameGetAll(appId, request.UserId))
            {
                // exists - save result in list
                if (WebApiApplication.DataAccess.StateExists(appId, pair.Key))
                {
                    list.Add(pair.Key, pair.Value);
                }
                // not exists - delete
                else
                {
                    WebApiApplication.DataAccess.GameDelete(appId, request.UserId, pair.Key);
                }
            }
           

            return new GetGameListResponse { Data = list };
        }

        private static bool IsValid(GetGameListRequest request, out string message)
        {
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