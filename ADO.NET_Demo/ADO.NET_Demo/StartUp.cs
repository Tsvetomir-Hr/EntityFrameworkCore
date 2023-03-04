using System;
using System.Data.SqlClient;
namespace ADO.NET_Demo
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            // First of all install correct DataProvider for ADO.NET
            // DataProvider for SQL Server -> SqlClient

            //If Authentication Needed :
            //SqlCredential sqlCredential = new SqlCredential(username, password);

            //Created connenction with the database
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);

            sqlConnection.Open();

            //Here we have open connection 
            //Then we need to create command 

            // If we use transaction:
            //SqlTransaction transaction = sqlConnection.BeginTransaction();
            // and we add the transaction we add it like paramaeter in creating the command
            string employeeCount = "SELECT " +
                                   "COUNT(*) AS EmployeeCount " +
                                   "FROM Employees";
            SqlCommand cmd = new SqlCommand(employeeCount, sqlConnection);
            //cmd.ExecuteScalar(); - SELECT ONE ROW , ONE COLUMN
            //cmd.ExecuteReader(); - SELECT MANY ROWS AND MANY COLUMNS
            //cmd.ExecuteNonQuery(); - INSERT, UPDATE, DELETE, ALTER, CREATE - returns number of changed rows
            int emplyeeCount = (int)cmd.ExecuteScalar();

            Console.WriteLine($"Employees available : {emplyeeCount}");

            //if we are here ,the above query has ended and the connection is free
            string employeeInfoQuery = @"SELECT 
                                         FirstName,
                                         LastName,
                                         JobTitle
                                         FROM Employees";
            SqlCommand employeeInfoCmd = new SqlCommand(employeeInfoQuery, sqlConnection);

            using SqlDataReader employeeInfoReader = employeeInfoCmd.ExecuteReader();
            //Read -> true (if there another rows)
            //Read-> false if there is no another rows next

            int rowNum = 1;
            while (employeeInfoReader.Read
                ())
            {
                string firstName = (string)employeeInfoReader["FirstName"];
                string lastName = (string)employeeInfoReader["LastName"];
                string jobTitle = (string)employeeInfoReader["JobTitle"];

                Console.WriteLine($"##{rowNum++}. {firstName} {lastName} - {jobTitle}");

            }
            employeeInfoReader.Close();
            sqlConnection.Close();
            Console.WriteLine("---------------------------");


            Console.WriteLine("Connection completed");

        }
    }
}
