// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEventController.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Controllers
{
    using System.Web.Http;

    using Models;

    public class GameEventController : ApiController
    {
        #region Public Methods and Operators

        public dynamic Post(string appId)
        {
            return new OkResponse();
        }

        #endregion
    }
}