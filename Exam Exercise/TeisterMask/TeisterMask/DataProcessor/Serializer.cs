﻿namespace TeisterMask.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using TeisterMask.Utilities;

    public class Serializer
    {
        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .ToArray()
                .Select(e => new
                {
                    e.Username,
                    Tasks = e.EmployeesTasks
                      .Where(et => et.Task.OpenDate >= date)
                      .ToArray()
                      .OrderByDescending(et => et.Task.DueDate)
                      .ThenBy(et => et.Task.Name)
                      .Select(et => new
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = et.Task.LabelType.ToString(),
                        ExecutionType = et.Task.ExecutionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(employees, Formatting.Indented);
        }
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            XmlHelper helper = new XmlHelper();
            ExportProjectDto[] exportProjectDto = context.Projects
                .Where(p => p.Tasks.Any())
                .ToArray()
                .Select(p => new ExportProjectDto()
                {
                    Name = p.Name,
                    TasksCount = p.Tasks.Count(),
                    DueDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
                    .Select(t => new ExportProjectTaskDto()
                    {
                        Name = t.Name,
                        LabelType = t.LabelType.ToString(),
                    })
                    .OrderBy(t => t.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.Tasks.Length)
                .ThenBy(p => p.Name)
                .ToArray();

            return helper.Serialize<ExportProjectDto[]>(exportProjectDto, "Projects");
        }
    }
}