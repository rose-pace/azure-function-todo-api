using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Avantia.Functions.Items
{
    public static class UpdateItem
    {
        [FunctionName("UpdateItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "items/{id}")] HttpRequest req,
            string id,
            [CosmosDB("ToDoList", "Items", Id = "{id}", ConnectionStringSetting = "CosmosDB")]ToDoItem inputItem,
            ILogger log)
        {
            if (inputItem == null)
            {
                log.LogWarning($"Item not found for id: {id}");
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ToDoItem item = JsonConvert.DeserializeObject<ToDoItem>(requestBody);

            inputItem.Description = item.Description;
            inputItem.IsComplete = item.IsComplete;

            return new OkObjectResult(new
            {
                message = $"Item with id {id} successfully updated.",
                data = inputItem
            });
        }
    }
}
