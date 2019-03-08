using System;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class InsertIntoSql
    {
        public static int WorkOrderInsert(WorkOrder workOrder)
        {
            string script = "";
            int rows = 0;

            try
            {
                Console.WriteLine("\nConnecting to SQL database");
                // log.LogInformation($"Connecting to SQL database");

                var connStr = Environment.GetEnvironmentVariable("sqldb_connection");

                Console.WriteLine($"CONNECTION STRING: {connStr}");

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();



                    foreach (var server in workOrder.RequestDetails.Servers)
                    {
                        foreach (var user in workOrder.RequestDetails.Users)
                        {
                            script = $"INSERT INTO [dbo].[Process] ([server],[user_name],[role],[action]) VALUES ('{server.Name}','{user.Name}','{workOrder.RequestDetails.Role}','ADD')";
                            using (SqlCommand cmd = new SqlCommand(script, connection))
                            {
                                // Execute the command and log the # rows affected.
                                rows = cmd.ExecuteNonQuery();
                                Console.WriteLine($"{rows} rows were updated");
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("\nDone. Press enter.");

            return rows;
        }
    }
}
