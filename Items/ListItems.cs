using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Avantia.Functions.Items
{
    public static class ListItems
    {
        [FunctionName("ListItems")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "items")] HttpRequest req,
            [CosmosDB("ToDoList", "Items", ConnectionStringSetting = "CosmosDB")]IEnumerable<ToDoItem> items,
            ILogger log)
        {
            log.LogInformation("ListItems request processed");
            return new OkObjectResult(items);
        }

        public const string _query = "SELECT * FROM Items OFFSET {skip} LIMIT {take}";
    }
}
