// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs" company="Exit Games GmbH">
//   Copyright (c) Exit Games GmbH.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.Models
{
    using System.Collections.Generic;
    using System.ComponentModel;

    #region Requests

    public class GameCloseRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorCount { get; set; }

        public string GameId { get; set; }

        /// <summary>
        /// the current game state, returned again with load game
        /// </summary>
        public dynamic State { get; set; }

        /// <summary>
        /// contains the actor list with user ids and actor numbers
        /// </summary>
        public dynamic State2 { get; set; }

        public string Type { get; set; }

        #endregion
    }

    public class GameCreateRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorNr { get; set; }

        public string GameId { get; set; }

        public string UserId { get; set; }

        public dynamic CreateOptions { get; set; }

        public bool CreateIfNotExists { get; set; }

        public string Type { get; set; }

        #endregion
    }

    public class GameLeaveRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorNr { get; set; }

        public string GameId { get; set; }

        /// <summary>
        /// true: the game is stored and the user can rejoin the game again (sent if the user disconnected)
        /// false: the game is deleted (sent if the user called leave game)
        /// </summary>
        [DefaultValue(false)]
        public bool IsInactive { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }
        
        #endregion
    }

    public class GameLoadRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorNr { get; set; }

        public string GameId { get; set; }

        public string UserId { get; set; }

        #endregion
    }

    public class GamePropertiesRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorNr { get; set; }

        public string GameId { get; set; }

        public dynamic State { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public string Type { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }
        #endregion
    }

    public class GetGameListRequest
    {
        #region Public Properties

        public string UserId { get; set; }

        #endregion
    }

    public class GameEventRequest
    {
        #region Public Properties

        [DefaultValue(-1)]
        public int ActorNr { get; set; }

        public string GameId { get; set; }

        public dynamic Data { get; set; }

        public dynamic State { get; set; }

        public string Type { get; set; }

        #endregion
    }

    #endregion

    #region Responses

    public class ErrorResponse
    {
        #region Public Properties

        public string Message { get; set; }

        public int ResultCode
        {
            get { return (int)Models.ResultCode.Failed; }
        }

        #endregion
    }

    public class GameLoadResponse
    {
        #region Public Properties

        public int ResultCode
        {
            get { return (int)Models.ResultCode.Ok; }
        }

        /// <summary>
        /// the game state as saved at game close 
        /// </summary>
        public dynamic State { get; set; }

        #endregion
    }

    public class GetGameListResponse
    {
        #region Public Properties

        /// <summary>
        /// the list of open games for this user, containing key/value pairs with game name/actor number
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        public int ResultCode
        {
            get { return (int)Models.ResultCode.Ok; }
        }

        #endregion
    }

    public class OkResponse
    {
        #region Public Properties

        public int ResultCode
        {
            get { return (int) Models.ResultCode.Ok; }
        }

        #endregion
        }

    #endregion

    public enum ResultCode
    {
        Ok = 0,
        Failed = 1, 
    }
}