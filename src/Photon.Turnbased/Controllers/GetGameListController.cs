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
    using DataAccess;

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

            foreach (var pair in Redis.HashGetAll(string.Format("{0}_{1}", appId, request.UserId)))
            {
                // exists - save result in list
                if (Redis.Exists(string.Format("{0}_{1}", appId, pair.Key)))
                {
                    list.Add(pair.Key, pair.Value);
                }
                // not exists - delete
                else
                {
                    Redis.HashDelete(string.Format("{0}_{1}", appId, request.UserId), pair.Key);
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