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
            int rows = 0;

            try
            {
                Console.WriteLine("\nConnecting to SQL database");
                // log.LogInformation($"Connecting to SQL database");

                var connStr = Environment.GetEnvironmentVariable("sqldb_connection");

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    var scriptLines = GenerateSqlScript.WorkOrderToSqlInsertScript(workOrder);

                    foreach (var scriptLine in scriptLines)
                    {
                        var command = new SqlCommand(scriptLine, connection); // script to be run

                        var transaction = connection.BeginTransaction(); // add a transaction object to connection
                        command.Transaction = transaction;

                        rows = command.ExecuteNonQuery(); // run the query set the number of rows inserted to rows variable

                        try { transaction.Commit(); } // Here the execution is committed to the DB
                        catch (Exception e)
                        {
                            transaction.Rollback(); // Rollback changes if any error occurs
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine($"Inserted {rows} rows to the DB");
            }
            return rows; // if everyting goes well it should response back the number of rows inserted
        }
    }
}
