using System;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class InsertIntoSql
    {
        public static ILogger _Log { get; set; }
        public enum LogType
        {
            error,
            information
        }
        public static int WorkOrderInsert(WorkOrder workOrder)
        {
            int rows = 0;

            try
            {
                // Grab connection string from local.settings.json
                // Azure functionn actually grabs it from the applciation settings variables inside azure portal
                var connStr = Environment.GetEnvironmentVariable("sqldb_connection");

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    // Generate scripts to run
                    var scriptLines = GenerateSqlScript.WorkOrderToSqlInsertScript(workOrder);

                    // Loop through each line of script
                    foreach (var scriptLine in scriptLines)
                    {
                        var command = new SqlCommand(scriptLine, connection); // command to be run

                        var transaction = connection.BeginTransaction(); // add a transaction object to connection
                        command.Transaction = transaction;

                        rows += command.ExecuteNonQuery(); // run the query set the number of rows inserted to rows variable

                        try { transaction.Commit(); } // Here the execution is committed to the DB
                        catch (Exception e)
                        {
                            string message = e.Message.Replace("'", "''");
                            transaction.Rollback(); // Rollback changes if any error occurs
                            _Log.LogError($"Error: had to Rollback inserting into the DB due to: {message}. WO:{workOrder.WorkOrderId}");
                            Log($"Error: had to Rollback inserting into the DB due to: {message}.", string.Join("\n", scriptLines), LogType.error);
                            Console.WriteLine(message);
                            throw e;
                        }
                    }
                }
            }
            // TODO We need to insert errros to the DB
            catch (Exception e)
            {
                string message = e.Message.Replace("'", "''");
                Console.WriteLine(message);
                Log($"Error: {message}.", workOrder.WorkOrderId, LogType.error);
            }
            finally
            {
                Console.WriteLine($"Inserted {rows} rows to the DB");
            }
            return rows; // if everyting goes well it should response back the number of rows inserted
        }

        public static int Log(string error, string emailBody, LogType logType)
        {
            int rowInserted = 0;
            var connStr = Environment.GetEnvironmentVariable("sqldb_connection");
            string _logType = "";
            switch (logType)
            {
                case LogType.error:
                    _logType = "Error";
                    break;
                case LogType.information:
                    _logType = "Information";
                    break;
            }
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                var query = $"INSERT INTO dbo.Process_Log (log_type, error_msg, email_body) VALUES ('{_logType}', '{error}', '{emailBody}')";

                var command = new SqlCommand(query, connection); // script to be run

                var transaction = connection.BeginTransaction(); // add a transaction object to connection
                command.Transaction = transaction;

                try
                {
                    rowInserted = command.ExecuteNonQuery(); // run the query set the number of rows inserted to rows variable
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                try { transaction.Commit(); } // Here the execution is committed to the DB
                catch (Exception e)
                {
                    transaction.Rollback(); // Rollback changes if any error occurs
                    Console.WriteLine(e.Message);
                    _Log.LogError($"Error: had to Rollback error logging into the DB due to: {e.Message}. emailBody:{emailBody}");
                }

                return rowInserted;
            }
        }
    }
}
