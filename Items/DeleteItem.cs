using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;

namespace Avantia.Functions.Items
{
    public static class DeleteItem
    {
        [FunctionName("DeleteItem")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "items/{id}")] HttpRequest req,
            string id,
            [CosmosDB("ToDoList", "Items", Id = "{id}", ConnectionStringSetting = "CosmosDB")]ToDoItem inputItem,
            [CosmosDB("ToDoList", "Items", ConnectionStringSetting = "CosmosDB")]DocumentClient items,
            ILogger log)
        {
            if (inputItem == null)
            {
                log.LogWarning($"Item not found for id: {id}");
                return new NotFoundResult();
            }

            await items.DeleteDocumentAsync(inputItem.SelfLink);

            return new OkObjectResult(new
            {
                message = $"Item with id {id} successfully deleted"
            });
        }
    }
}
