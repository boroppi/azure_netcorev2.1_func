using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class HttpTriggerCSharp
    {
        [FunctionName("HttpTriggerCSharp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "parse-email")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            var workOrder = ParseTextIntoObject.TextToParse(requestBody);

            //var script = GenerateSqlScript.WorkOrderToSqlInsertScript(workOrder);

            var numberOfInsertedRows = InsertIntoSql.WorkOrderInsert(workOrder);
            //var json = JsonConvert.SerializeObject(workOrder);

            return numberOfInsertedRows != 0 ?
            (ActionResult)new OkObjectResult("Inserted " + numberOfInsertedRows + " rows.")
                    :
                new BadRequestObjectResult("Did not insert any rows");
            //return script != null
            //             ? (ActionResult)new OkObjectResult(script)
            //                : new BadRequestObjectResult("something went wrong");
        }
    }
}
