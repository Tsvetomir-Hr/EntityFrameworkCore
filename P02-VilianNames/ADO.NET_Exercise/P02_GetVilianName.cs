using System;
using System.Data.SqlClient;
using System.Text;

namespace ADI.NET_Exercise
{
    public class P02_GetVilianName
    {
        static void Main(string[] args)
        {
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);

            sqlConnection.Open();


            string result = GetViliansNameWithMinionsCount(sqlConnection);

            Console.WriteLine(result);
  
            sqlConnection.Close();
        }

        private static string  GetViliansNameWithMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder output = new StringBuilder();
            string query = @"SELECT 
                          v.[Name],
                          COUNT(mv.MinionId) AS NumberOfMinions
                          FROM Villains AS v
                          JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                          GROUP BY v.[Name]
                          HAVING COUNT(*) > 3
                          ORDER BY NumberOfMinions DESC";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                output.AppendLine($"{reader["Name"]} - {reader["NumberOfMinions"]}");
            }

            return output.ToString().TrimEnd();
        }
       
    }
}
