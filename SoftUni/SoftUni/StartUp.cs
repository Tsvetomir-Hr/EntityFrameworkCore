using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SoftUni;

public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbcontext = new SoftUniContext();

        string result = GetEmployeesInPeriod(dbcontext);
        Console.WriteLine(result);
    }
    //public static string GetEmployeesFullInformation(SoftUniContext context)
    //{
    //    StringBuilder result = new StringBuilder();

    //    var allEmployees = context.Employees
    //        .OrderBy(e => e.EmployeeId)
    //        .Select(e => new
    //        {
    //            e.FirstName,
    //            e.LastName,
    //            e.MiddleName,
    //            e.JobTitle,
    //            e.Salary
    //        });


    //    foreach (var e in allEmployees)
    //    {
    //        result.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
    //    }
    //    return result.ToString().TrimEnd();
    //}

    //public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    //{
    //    StringBuilder result = new StringBuilder();

    //    var employees = context.Employees
    //        .OrderBy(e => e.FirstName)
    //        .Select (e => new
    //        {
    //            e.FirstName,
    //            e.Salary
    //        })
    //        .Where(e=>e.Salary>50000)
    //        .ToArray();

    //    foreach (var e in employees)
    //    {
    //        result.AppendLine($"{e.FirstName} - {e.Salary:f2}");
    //    }
    //    return result.ToString().TrimEnd();
    //}
    //public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    //{
    //    StringBuilder sb = new StringBuilder();

    //    var employees = context.Employees
    //        .Where(e => e.Department.Name == "Research and Development")
    //        .OrderBy(e=>e.Salary)
    //        .ThenByDescending(e=>e.FirstName)
    //        .Select(e=> new 
    //        {
    //            e.FirstName,
    //            e.LastName,
    //            e.Department,
    //            e.Salary
    //        })
    //        .ToArray();

    //    foreach ( var e in employees )
    //    {
    //        sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
    //    }
    //    return sb.ToString().TrimEnd();
    //}

    //public static string AddNewAddressToEmployee(SoftUniContext context)
    //{
    //    Address newAddress = new Address()
    //    {
    //        AddressText = "Vitoshka 15",
    //        TownId = 4
    //    };

    //    context.Addresses.Add(newAddress); // this is the way for adding into database
    //    Employee? employee = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
    //    if (employee != null)
    //    {
    //        employee.Address = newAddress;
    //    }
    //    context.SaveChanges();

    //    var employeeAddresses = context.Employees
    //        .OrderByDescending(e => e.AddressId)
    //        .Take(10)
    //        .Select(e => e.Address!.AddressText)
    //        .ToArray();

    //    return string.Join(Environment.NewLine, employeeAddresses);
    //}


    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder result = new StringBuilder();
        var employees = context.Employees
             //.Where(e => e.EmployeesProjects
             //.Any(ep => ep.Project.StartDate.Year >= 2001 &&
             //ep.Project.StartDate.Year <= 2003))
             .Take(10)
             .Select(e => new
             {
                 e.FirstName,
                 e.LastName,
                 ManagerFirstName = e.Manager!.FirstName,
                 ManagerLastName = e.Manager!.LastName,
                 Projects = e.EmployeesProjects
                 .Where(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)
                     .Select(ep => new
                     {
                         ProjectName = ep.Project.Name,
                         StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                         EndDate = ep.Project.EndDate.HasValue ?
                         ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"

                     })
                     .ToArray()
             })
             .ToArray();

        foreach (var e in employees)
        {
            result.AppendLine($"{e.FirstName} {e.LastName} - Manager {e.ManagerFirstName} {e.ManagerLastName}");
            foreach (var p in e.Projects)
            {
                result.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }
        return result.ToString().TrimEnd();
    }
}