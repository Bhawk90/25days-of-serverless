using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Foodfest.Models;
using System.Linq;
using Microsoft.WindowsAzure.Storage;

namespace Foodfest
{
    public static class FoodfestFunctions
    {
        [FunctionName("AddItem")]
        public static async Task<IActionResult> AddItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] FoodfestItemPostBindingModel bindingModel,
            [Table("FoodfestItems", "Foods", Connection = "AzureWebJobsStorage")] IAsyncCollector<FoodfestItem> foodfestItemsTable,
            ILogger log)
        {
            try
            {
                var tableModel = FoodfestFactory.GetTableModelFor(bindingModel);
                await foodfestItemsTable.AddAsync(tableModel);
                log.LogInformation($"Added {tableModel.RowKey}: {tableModel.Food} by {tableModel.FullName}");

                return new OkObjectResult(FoodfestFactory.GetBindingModelFor(tableModel));
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("GetItems")]
        public static async Task<IEnumerable<FoodfestItemBindingModel>> GetItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table("FoodfestItems", "Foods", Connection = "AzureWebJobsStorage")] CloudTable foodfestItemsTable,
            ILogger log)
        {
            var query = new TableQuery<FoodfestItem>();
            var segment = await foodfestItemsTable.ExecuteQuerySegmentedAsync(query, null);

            return segment.Results.Select(tableModel => FoodfestFactory.GetBindingModelFor(tableModel));
        }

        [FunctionName("UpdateItem")]
        public static async Task<IActionResult> UpdateItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "UpdateItem/{id}")] FoodfestItemPatchBindingModel bindingModel,
            [Table("FoodfestItems", "Foods", Connection = "AzureWebJobsStorage")] CloudTable foodfestItemsTable,
            ILogger log,
            string id)
        {
            try
            {
                var findOperation = TableOperation.Retrieve<FoodfestItem>("Foods", id);
                var findResult = await foodfestItemsTable.ExecuteAsync(findOperation);
                if (findResult.Result == null)
                {
                    return new NotFoundResult();
                }

                var existingRow = (FoodfestItem)findResult.Result;
                existingRow.Food = bindingModel.Food ?? existingRow.Food;
                existingRow.Amount = bindingModel.Amount ?? existingRow.Amount;

                var replaceOperation = TableOperation.Replace(existingRow);
                await foodfestItemsTable.ExecuteAsync(replaceOperation);

                return new OkObjectResult(FoodfestFactory.GetBindingModelFor(existingRow));
            }
            catch
            {
                return new BadRequestResult();
            }
        }

        [FunctionName("DeleteItem")]
        public static async Task<IActionResult> DeleteItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteItem/{id}")] HttpRequest req,
            [Table("FoodfestItems", "Foods", Connection = "AzureWebJobsStorage")] CloudTable foodfestItemsTable,
            ILogger log,
            string id)
        {
            var deleteOperation = TableOperation.Delete(new TableEntity() { PartitionKey = "Foods", RowKey = id, ETag = "*" });

            try
            {
                var deleteResult = await foodfestItemsTable.ExecuteAsync(deleteOperation);
            }
            catch (StorageException e) when (e.RequestInformation.HttpStatusCode == 404)
            {
                return new NotFoundResult();
            }
            catch
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}
