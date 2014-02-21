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
    
    public class Redis
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

        public static void FlushAll()
        {
            using (var redisClient = (RedisNativeClient)WebApiApplication.PooledRedisClientManager.GetClient())
            {
                redisClient.FlushAll();
            }
        }
    }
}