using ADI.NET_Exercise;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace P03_MinionNames
{
    public class Program
    {
        public void Main(string[] args)
        {
            string[] minionsInfo = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string vilianName = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1];

         
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);

            sqlConnection.Open();



            sqlConnection.Close();
        }

        private static string AddNewMinon(SqlConnection connection,string [] minionInfo,string vilianName)
        {
           StringBuilder output = new StringBuilder();

            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string minionTown = minionInfo[2];

          SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                int townId = GetTownId(connection, transaction, minionTown, output);

            }
            catch (Exception e)
            {
                transaction.Rollback();  
              return e.ToString();
            }
        }
        private static int GetTownId(SqlConnection connection,SqlTransaction transaction,string minionTown,StringBuilder output)
        {
            string townNameQuery = @"SELECT [Id]
                                     FROM Towns
                                     WHERE [Name] = @minionTown";
            SqlCommand townIdCmd = new SqlCommand(townNameQuery, connection, transaction);
            townIdCmd.Parameters.AddWithValue("@townName", minionTown);

            object townIdObj = townIdCmd.ExecuteScalar();
            int townId;
            if (townIdObj == null)
            {
                string addTownQuery = "INSERT INTO Towns ([Name]) VALUES ('@minionTown')";

                SqlCommand addTownIdCmd = new SqlCommand(addTownQuery, connection, transaction);

                addTownIdCmd.Parameters.AddWithValue("@minionTown", minionTown);

                addTownIdCmd.ExecuteNonQuery();
                output.AppendLine($"Town {minionTown} was added to the database.");

                townIdObj = townIdCmd.ExecuteScalar();  
            }
            return (int)townIdObj;
       
        }
    }
}
