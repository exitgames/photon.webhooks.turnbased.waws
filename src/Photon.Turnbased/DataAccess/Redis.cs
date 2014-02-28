// ------------------------------------------------------------------------------------------------
//  <copyright file="Redis.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System.Collections.Generic;
    using ServiceStack.Redis;
    using ServiceStack.Text;

    public class Redis : IDataAccess
    {
        public static bool Exists(string key)
        {   
            using(var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                return redisClient.Exists(key) == 1;
            }
        }

        public static void Set(string key, string value)
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                redisClient.Set(key, value.ToUtf8Bytes());
            }
        }

        public static string Get(string key)
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                return redisClient.Get(key).FromUtf8Bytes();
            }
        }

        public static void Delete(string key)
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                redisClient.Del(key);
            }
        }

        public static void HashSet(string hashId, string key, string value)
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                redisClient.HSet(hashId, key.ToUtf8Bytes(), value.ToUtf8Bytes());
            }
        }

        public static Dictionary<string, string> HashGetAll(string hashId)
        {
            var result = new Dictionary<string, string>();

            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                var multiDataList = redisClient.HGetAll(hashId);

                for (var i = 0; i < multiDataList.Length; i += 2)
                {
                    result.Add(multiDataList[i].FromUtf8Bytes(), multiDataList[i + 1].FromUtf8Bytes());
                }

                return result;
            }
        }

        public static void HashDelete(string hashId, string key)
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                redisClient.HDel(hashId, key.ToUtf8Bytes());
            }
        }

        public bool StateExists(string appId, string key)
        {
            return Exists(string.Format("{0}_{1}", appId, key));
        }

        public void StateSet(string appId, string key, string state)
        {
            Set(string.Format("{0}_{1}", appId, key), state);
        }

        public string StateGet(string appId, string key)
        {
            return Get(string.Format("{0}_{1}", appId, key));
        }

        public void StateDelete(string appId, string key)
        {
            Delete(string.Format("{0}_{1}", appId, key));
        }

        public void GameInsert(string appId, string key, string gameId, int actorNr)
        {
            HashSet(string.Format("{0}_{1}", appId, key), gameId, actorNr.ToString());
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            HashDelete(string.Format("{0}_{1}", appId, key), gameId);
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            return HashGetAll(string.Format("{0}_{1}", appId, key));
        }
    }
}