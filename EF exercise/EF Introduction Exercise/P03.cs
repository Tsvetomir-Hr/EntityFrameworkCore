using SoftUni.Data;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbcontext = new SoftUniContext();

        string result = GetEmployeesFullInformation(dbcontext);
        Console.WriteLine(result);
    }
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder result = new StringBuilder();

        var allEmployees = context.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            });


        foreach (var e in allEmployees)
        {
            result.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }
        return result.ToString().TrimEnd();
    }
}