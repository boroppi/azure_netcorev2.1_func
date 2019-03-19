
using System.Collections.Generic;

namespace Company.Function
{
    public static class GenerateSqlScript
    {
        public static List<string> WorkOrderToSqlInsertScript(WorkOrder workOrder)
        {
            var scriptLines = new List<string>();

            foreach (var server in workOrder.RequestDetails.Servers)
            {
                foreach (var user in workOrder.RequestDetails.Users)
                {
                    scriptLines.Add($"INSERT INTO [dbo].[Process] ([server],[user_name],[role],[action], [work_order_id]) VALUES ('{server.Name}','{user.Name}','{workOrder.RequestDetails.Role}','ADD', '{workOrder.WorkOrderId}');");
                }
            }

            return scriptLines;
        }
    }
}
