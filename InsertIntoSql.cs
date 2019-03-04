using System;
using System.Data.SqlClient;
using System.Text;

namespace Company.Function
{
    public static class InsertIntoSql
    {

        public static string WorkOrderInsert(WorkOrder workOrder)
        {

            try 
            { 
                Console.WriteLine("\nConnecting to SQL database");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            
               // Just documenting this here
               //CREATE LOGIN sql_azure_function WITH PASSWORD = '24Password24#' 
               //CREATE USER sql_azure_function_user FROM LOGIN sql_azure_function;
               //ALTER ROLE db_datawriter ADD MEMBER sql_azure_function_user

                builder.DataSource = "dco-tools.database.windows.net"; 
                builder.UserID = "sql_azure_function_user";            
                builder.Password = "24Password24#";     
                builder.InitialCatalog = "repo";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                 {

                connection.Open(); 

                string script = "";
            

                foreach (var server in workOrder.RequestDetails.Servers)
                {
                    foreach (var user in workOrder.RequestDetails.Users)
                    {
                    
                    script = $"INSERT INTO [dbo].[Process] ([server],[user_name],[role],[action]) VALUES ('{server.Name}','{user.Name}','{workOrder.RequestDetails.Role}','ADD')";
        

                     SqlCommand command = new SqlCommand(script, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    Console.WriteLine(script.ToString());
                    
                    command.Connection.Close();

                    //using (SqlCommand command = new SqlCommand(script, connection))
                    //{
                        
                        //using (SqlDataReader reader = command.ExecuteReader())
                        //{
                        //  while (reader.Read())
                        //                            {
                        //                              Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        //                        }
                        //                  }
                   // }       

                
                    }
                }






                 }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            
            }
            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine(); 
        
      

            return "true";
        }
    }
}
