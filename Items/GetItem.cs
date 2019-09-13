using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Avantia.Functions.Items
{
    public static class GetItem
    {
        [FunctionName("GetItem")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "items/{id}")] HttpRequest req,
            string id,
            [CosmosDB("ToDoList", "Items", Id = "{id}", ConnectionStringSetting = "CosmosDB")]ToDoItem inputItem,
            ILogger log)
        {
            if (inputItem == null)
            {
                log.LogWarning($"Item not found for id: {id}");
                return new NotFoundResult();
            }

            log.LogInformation($"Item returned successfully for id: {id}");
            return new OkObjectResult(inputItem);
        }
    }
}
