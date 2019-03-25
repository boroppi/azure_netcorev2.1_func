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
            InsertIntoSql._Log = log;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (!requestBody.Contains("Work Order"))
            {
                InsertIntoSql.Log("Probably Empty request body or does not contain work order", "", InsertIntoSql.LogType.error);
                return new BadRequestObjectResult("Empty / Short request body or no Work Order");
            }
            else if (!requestBody.Contains("Administrative Account Management Service (AAMS)"))
            {
                InsertIntoSql.Log("generic email detected, not a AAMS request not processing.", RegexFind.FindString(requestBody, @"Work Order.([^\.]*)", "work order"), InsertIntoSql.LogType.error);
                return new BadRequestObjectResult("Wrong WorkOrder Summary");
            }
            var workOrder = ParseTextIntoObject.TextToParse(requestBody);

            var numberOfInsertedRows = InsertIntoSql.WorkOrderInsert(workOrder);

            if (numberOfInsertedRows > 0)
            {
                log.LogInformation($"C# HTTP trigger function processed a request. WO:{workOrder.WorkOrderId} inserted {numberOfInsertedRows} rows.");
                InsertIntoSql.Log($"Information: inserted {numberOfInsertedRows} rows.", workOrder.WorkOrderId, InsertIntoSql.LogType.information);
            }

            return numberOfInsertedRows != 0 ?
            (ActionResult)new OkObjectResult("Inserted " + numberOfInsertedRows + " rows.")
                    :
                new BadRequestObjectResult("Did not insert any rows");
        }
    }
}
