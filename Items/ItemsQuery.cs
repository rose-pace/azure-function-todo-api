using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;

namespace Avantia.Functions.Items
{
    public static class ItemsQuery
    {
        [FunctionName("ItemsQuery")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "query/items")] HttpRequest req,
            [CosmosDB("ToDoList", "Items", ConnectionStringSetting = "CosmosDB")]DocumentClient items,
            ILogger log)
        {
            bool isComplete = false;
            bool.TryParse(req.Query["is-complete"], out isComplete);

            var query = items.CreateDocumentQuery<ToDoItem>(UriFactory.CreateDocumentCollectionUri("ToDoList", "Items"))
                .Where(i => i.IsComplete == isComplete)
                .OrderByDescending(i => i.Timestamp)
                .AsDocumentQuery();

            var filteredItems = new List<ToDoItem>();

            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<ToDoItem>();
                filteredItems.AddRange(response);
            }

            return new OkObjectResult(filteredItems);
        }
    }
}
