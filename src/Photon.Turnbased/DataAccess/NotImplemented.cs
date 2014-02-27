// ------------------------------------------------------------------------------------------------
//  <copyright file="NotImplemented.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System.Collections.Generic;

    public class NotImplemented : IDataAccess
    {
        public bool StateExists(string appId, string key)
        {
            throw new System.NotImplementedException();
        }

        public void StateSet(string appId, string key, string state)
        {
            throw new System.NotImplementedException();
        }

        public string StateGet(string appId, string key)
        {
            throw new System.NotImplementedException();
        }

        public void StateDelete(string appId, string key)
        {
            throw new System.NotImplementedException();
        }

        public void GameInsert(string appId, string key, string gameId, int actorNr)
        {
            throw new System.NotImplementedException();
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            throw new System.NotImplementedException();
        }
    }
}