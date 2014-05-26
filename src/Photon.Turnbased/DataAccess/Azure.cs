// ------------------------------------------------------------------------------------------------
//  <copyright file="Azure.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    using log4net;

    public class Azure : IDataAccess
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("MyLogger");

        public bool StateExists(string appId, string key)
        {
            // Create the blob client.
            var blobClient = WebApiApplication.CloudStorageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            //workaround for Exist()
            try
            {
                blockBlob.FetchAttributes();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void StateSet(string appId, string key, string state)
        {
            // Create the blob client.
            var blobClient = WebApiApplication.CloudStorageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            blockBlob.UploadText(state);
        }

        public string StateGet(string appId, string key)
        {
            // Create the blob client.
            var blobClient = WebApiApplication.CloudStorageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            try
            {
                return blockBlob.DownloadText();
            }
            //Azure throws exception if file not found
            catch (StorageException)
            {
                if (log.IsDebugEnabled) log.DebugFormat("StateGet, state {0}/{1} not found", appId, key);
                return null;
            }
        }

        public void StateDelete(string appId, string key)
        {
            // Create the blob client.
            var blobClient = WebApiApplication.CloudStorageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            try
            {
                blockBlob.Delete();
            }
            //Azure throws exception if file not found
            catch (StorageException)
            {
                if (log.IsDebugEnabled) log.DebugFormat("StateDelete, state {0}/{1} not found", appId, key);
            }
        }

        public void GameInsert(string appId, string key, string gameId, int actorNr)
        {
            var tableClient = WebApiApplication.CloudStorageAccount.CreateCloudTableClient();
            tableClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            //table names can't have "-" in the name and may not begin with a numeric character
            var table = tableClient.GetTableReference(string.Format("games{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if table is already created manually in Azure 
            table.CreateIfNotExists();

            table.Execute(TableOperation.InsertOrReplace(new GameEntity { PartitionKey = key, RowKey = gameId, ActorNr = actorNr }));
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            var tableClient = WebApiApplication.CloudStorageAccount.CreateCloudTableClient();
            tableClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            //table names can't have "-" in the name and may not begin with a numeric character
            var table = tableClient.GetTableReference(string.Format("games{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if table is already created manually in Azure 
            table.CreateIfNotExists();

            try
            {
                table.Execute(TableOperation.Delete(new TableEntity { PartitionKey = key, RowKey = gameId, ETag = "*" }));
            }
            catch (StorageException)
            {
                if (log.IsDebugEnabled) log.DebugFormat("GameDelete, game {0}/{1}/{2} not found", appId, key, gameId);
            }
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            var tableClient = WebApiApplication.CloudStorageAccount.CreateCloudTableClient();
            tableClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            //table names can't have "-" in the name and may not begin with a numeric character
            var table = tableClient.GetTableReference(string.Format("games{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if table is already created manually in Azure 
            table.CreateIfNotExists();

            var result = table.ExecuteQuery(new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, key)));

            return result.ToDictionary(dynamicTableEntity => dynamicTableEntity.RowKey, dynamicTableEntity => dynamicTableEntity["ActorNr"].Int32Value.ToString());
        }
    }

    public class GameEntity : TableEntity
    {
        public int ActorNr { get; set; }
    }
}