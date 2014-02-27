// ------------------------------------------------------------------------------------------------
//  <copyright file="IDataAccess.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System.Collections.Generic;

    public interface IDataAccess
    {
        bool StateExists(string appId, string key);

        void StateSet(string appId, string key, string state);

        string StateGet(string appId, string key);

        void StateDelete(string appId, string key);

        void GameInsert(string appId, string key, string gameId, int actorNr);

        void GameDelete(string appId, string key, string gameId);

        Dictionary<string, string> GameGetAll(string appId, string key);
    }
}