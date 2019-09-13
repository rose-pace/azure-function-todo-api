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
    public static class AddItem
    {
        [FunctionName("AddItem")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "items")] HttpRequest req,
            [CosmosDB("ToDoList", "Items", ConnectionStringSetting = "CosmosDB")]out ToDoItem outputItem,
            ILogger log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            ToDoItem newItem = JsonConvert.DeserializeObject<ToDoItem>(requestBody);

            if (string.IsNullOrEmpty(newItem.Name))
            {
                outputItem = null;
                return new BadRequestObjectResult("A name is required for new tasks.");
            }

            newItem.Id = Guid.NewGuid().ToString();
            newItem.IsComplete = false;

            outputItem = newItem;

            log.LogInformation($"Item added successfully with id: {outputItem.Id}");
            return new OkObjectResult(new
            {
                message = "Item added successfully",
                data = outputItem
            });
        }
    }
}
