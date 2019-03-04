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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "parse-email")] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            var workOrder = ParseTextIntoObject.TextToParse(requestBody);

            var script = GenerateSqlScript.WorkOrderToSqlInsertScript(workOrder);

            var script2 = InsertIntoSql.WorkOrderInsert(workOrder);

            //var json = JsonConvert.SerializeObject(workOrder);

            return script != null
                         ? (ActionResult)new OkObjectResult(script)
                            : new BadRequestObjectResult("something went wrong");
            //  return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //   : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
   
        }
    }
}
