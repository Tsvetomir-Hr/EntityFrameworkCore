// ReSharper disable InconsistentNaming

namespace TeisterMask.DataProcessor;

using Castle.Components.DictionaryAdapter;
using Data;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;
using TeisterMask.Utilities;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedProject
        = "Successfully imported project - {0} with {1} tasks.";

    private const string SuccessfullyImportedEmployee
        = "Successfully imported employee - {0} with {1} tasks.";

    public static string ImportProjects(TeisterMaskContext context, string xmlString)
    {
        XmlHelper helper = new XmlHelper();
        ImportProjectDto[] projectDtos = helper.Deserialize<ImportProjectDto[]>(xmlString, "Projects");

        ICollection<Project> validProjects = new HashSet<Project>();
        StringBuilder output = new StringBuilder();

        foreach (var pDto in projectDtos)
        {
            if (!IsValid(pDto))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            bool isOpenDateValid = DateTime.TryParseExact(pDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate);
            if (!isOpenDateValid)
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            DateTime? dueDate = null;
            if (!String.IsNullOrEmpty(pDto.DueDate))
            {
                bool isDueDateValid = DateTime.TryParseExact(pDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateValue);
                if (!isDueDateValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                dueDate = dueDateValue;
            }
            Project project = new Project()
            {
                Name = pDto.Name,
                OpenDate = openDate,
                DueDate = dueDate
            };
            foreach (var tDto in pDto.Tasks)
            {
                if (!IsValid(tDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isOpenDateTaskValid = DateTime.TryParseExact(tDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDateTask);
                if (!isOpenDateTaskValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isDueDateTaskValid = DateTime.TryParseExact(tDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateTask);

                if (!isDueDateTaskValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                if (openDateTask < openDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (dueDateTask > dueDate)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }


                bool isExecutionTypeValid = Enum.TryParse<ExecutionType>(tDto.ExecutionType, out ExecutionType execType);
                if (!isExecutionTypeValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool isLabelTypeValid = Enum.TryParse<LabelType>(tDto.LabelType, out LabelType labelType);
                if (!isLabelTypeValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Task task = new Task()
                {
                    Name = tDto.Name,
                    OpenDate = openDateTask,
                    DueDate = dueDateTask,
                    ExecutionType = execType,
                    LabelType = labelType
                };
                project.Tasks.Add(task);
            }

            validProjects.Add(project);
            output.AppendLine(String.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
        }
        context.Projects.AddRange(validProjects);
        context.SaveChanges();

        return output.ToString().TrimEnd();

    }

    public static string ImportEmployees(TeisterMaskContext context, string jsonString)
    {
        ImportEmployeeDto[] employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

        ICollection<Employee> validEmployees = new HashSet<Employee>();
        StringBuilder output = new StringBuilder();

        foreach (var eDto in employeeDtos)
        {
            if (!IsValid(eDto))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            Employee employee = new Employee()
            {
                Username = eDto.Username,
                Email = eDto.Email,
                Phone = eDto.Phone,
            };
            foreach (var tDtoId in eDto.Tasks)
            {
                if (!context.Tasks.Any(t => t.Id == tDtoId))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Task task = context.Tasks.Find(tDtoId);

                if (task == null)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                employee.EmployeesTasks.Add(new EmployeeTask()
                {
                    Task = task
                });
            }
            validEmployees.Add(employee);
            output.AppendLine(String.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
        }
        context.Employees.AddRange(validEmployees);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}
