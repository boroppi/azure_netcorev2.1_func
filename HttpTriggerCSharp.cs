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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (!requestBody.Contains("Administrative Account Management Service (AAMS)"))
            {
                return new BadRequestObjectResult("Wrong WorkOrder Summary");
            }
            var workOrder = ParseTextIntoObject.TextToParse(requestBody);

            var numberOfInsertedRows = InsertIntoSql.WorkOrderInsert(workOrder);

            return numberOfInsertedRows != 0 ?
            (ActionResult)new OkObjectResult("Inserted " + numberOfInsertedRows + " rows.")
                    :
                new BadRequestObjectResult("Did not insert any rows");

        }
    }
}
