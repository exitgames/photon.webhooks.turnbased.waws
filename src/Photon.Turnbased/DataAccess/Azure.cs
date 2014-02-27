// ------------------------------------------------------------------------------------------------
//  <copyright file="Azure.cs" company="Exit Games GmbH">
//    Copyright (c) Exit Games GmbH.  All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------------

namespace Photon.Webhooks.Turnbased.DataAccess
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;

    public class Azure : IDataAccess
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                                                                    string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                                                                                        ConfigurationManager.AppSettings["AzureAccountName"], ConfigurationManager.AppSettings["AzureAccountKey"])
                                                                                );

        public bool StateExists(string appId, string key)
        {
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
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
            var blobClient = storageAccount.CreateCloudBlobClient();
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
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            return blockBlob.DownloadText();
        }

        public void StateDelete(string appId, string key)
        {
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            // Retrieve reference to container. Containers use same name rules as tables (see table name limitations).
            var container = blobClient.GetContainerReference(string.Format("states{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if container is already created manually in Azure 
            container.CreateIfNotExists();

            var blockBlob = container.GetBlockBlobReference(key);

            blockBlob.Delete();
        }

        public void GameInsert(string appId, string key, string gameId, int actorNr)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            //table names can't have "-" in the name and may not begin with a numeric character
            var table = tableClient.GetTableReference(string.Format("games{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if table is already created manually in Azure 
            table.CreateIfNotExists();

            table.Execute(TableOperation.InsertOrReplace(new GameEntity { PartitionKey = key, RowKey = gameId, ActorNr = actorNr }));
        }

        public void GameDelete(string appId, string key, string gameId)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.AuthenticationScheme = AuthenticationScheme.SharedKeyLite;

            //table names can't have "-" in the name and may not begin with a numeric character
            var table = tableClient.GetTableReference(string.Format("games{0}", appId.Replace("-", "")));

            //create for demo - this call is obsolete if table is already created manually in Azure 
            table.CreateIfNotExists();

            table.Execute(TableOperation.Delete(new TableEntity { PartitionKey = key, RowKey = gameId, ETag = "*" }));
        }

        public Dictionary<string, string> GameGetAll(string appId, string key)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
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