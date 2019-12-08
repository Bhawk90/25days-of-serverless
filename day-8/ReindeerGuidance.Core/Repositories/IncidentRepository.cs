using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using ReindeerGuidance.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReindeerGuidance.Core.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        #region Data Members

        private readonly TableStorageConfiguration _configuration;

        private const string _TABLE_NAME = "ReindeerIncidents";
        private const string _TABLE_PARTITION_KEY = "RDIC";

        #endregion Data Members

        #region Constructor

        public IncidentRepository(IOptions<TableStorageConfiguration> configuration)
        {
            _configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion Constructor

        #region Public Methods

        public async Task<IncidentEntity> CreateAsync(IncidentPostBindingModel model)
        {
            var storageClient = this.GetStorageClient();
            var incidentTable = storageClient.GetTableReference(_TABLE_NAME);
            await incidentTable.CreateIfNotExistsAsync();

            TableOperation insertOperation = TableOperation.Insert(new IncidentEntity()
            {
                RowKey = $"{_TABLE_PARTITION_KEY}_{new Random().Next(1000, 3000)}",
                PartitionKey = _TABLE_PARTITION_KEY,
                Status = IncidentEntity.IncidentStatus.Open,
                Title = model.Title,
                Notes = model.Notes,
                ModifiedAt = DateTime.UtcNow
            });

            var result = await incidentTable.ExecuteAsync(insertOperation);
            return result.Result as IncidentEntity;
        }

        public async Task DeleteAsync(string id)
        {
            var storageClient = this.GetStorageClient();
            var incidentTable = storageClient.GetTableReference(_TABLE_NAME);
            await incidentTable.CreateIfNotExistsAsync();

            var incident = await GetByIdAsync(id);
            if (incident != null)
            {
                await incidentTable.ExecuteAsync(TableOperation.Delete(incident));
            }
            else
            {
                throw new NullReferenceException("Object with id not found");
            }
        }

        public async Task<IList<IncidentEntity>> GetAsync()
        {
            var storageClient = this.GetStorageClient();
            var incidentTable = storageClient.GetTableReference(_TABLE_NAME);
            await incidentTable.CreateIfNotExistsAsync();

            // Retrieve all elements with the specified PartitionKey
            var query = new TableQuery<IncidentEntity>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _TABLE_PARTITION_KEY)
            );

            var segment = await incidentTable.ExecuteQuerySegmentedAsync(query, null);
            return segment.Results;
        }

        public async Task<IncidentEntity> GetByIdAsync(string id)
        {
            var storageClient = this.GetStorageClient();
            var incidentTable = storageClient.GetTableReference(_TABLE_NAME);
            await incidentTable.CreateIfNotExistsAsync();

            var retrieveOperation = TableOperation.Retrieve<IncidentEntity>(_TABLE_PARTITION_KEY, id);
            var result = await incidentTable.ExecuteAsync(retrieveOperation);

            return result.Result as IncidentEntity;
        }

        public async Task UpdateAsync(string id, IncidentPutBindingModel model)
        {
            var storageClient = this.GetStorageClient();
            var incidentTable = storageClient.GetTableReference(_TABLE_NAME);
            await incidentTable.CreateIfNotExistsAsync();

            var incident = await GetByIdAsync(id);
            if (incident != null)
            {
                incident.ModifiedAt = DateTime.UtcNow;
                incident.Title = model.Title ?? incident.Title;
                incident.Notes = model.Notes ?? incident.Notes;

                if (Enum.TryParse(typeof(IncidentEntity.IncidentStatus), model.Status, out object incidentStatus))
                {
                    incident.Status = (IncidentEntity.IncidentStatus)incidentStatus;
                }

                await incidentTable.ExecuteAsync(TableOperation.Replace(incident));
            }
            else
            {
                throw new NullReferenceException("Object with id not found");
            }
        }

        #endregion Public Methods

        #region Private Helper Methods

        /// <summary>
        /// Returns a pre-configured instance of CloudTableClient.
        /// </summary>
        /// <returns></returns>
        private CloudTableClient GetStorageClient()
        {
            var storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    _configuration.StorageName, _configuration.AccessKey)
                , useHttps: true);

            return storageAccount.CreateCloudTableClient();
        }

        #endregion Private Helper Methods
    }
}
