namespace Company.Function
{
    public static class InsertIntoSql
    {
        public static string WorkOrderInsert(WorkOrder workOrder)
        {
            string script = "";

            foreach (var server in workOrder.RequestDetails.Servers)
            {
                foreach (var user in workOrder.RequestDetails.Users)
                {
                    script += $"INSERT INTO [dbo].[Process] ([server],[user_name],[role],[action]) VALUES ('{server.Name}','{user.Name}','{workOrder.RequestDetails.Role}','ADD');\n";
                }
            }

            return script;
        }
    }
}
