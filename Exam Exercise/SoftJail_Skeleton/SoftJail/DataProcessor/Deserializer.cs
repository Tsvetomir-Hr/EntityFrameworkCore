namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportDepartmentDto[] departmentDto =
                  JsonConvert.DeserializeObject<ImportDepartmentDto[]>(jsonString);
            ICollection<Department> validDepartments = new HashSet<Department>();


            foreach (var depDto in departmentDto)
            {
                if (!IsValid(depDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (!depDto.Cells.Any())
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                if (depDto.Cells.Any(c => !IsValid(c)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = depDto.Name,

                };
                foreach (var cellDto in depDto.Cells)
                {
                    Cell cell = Mapper.Map<Cell>(cellDto);
                    department.Cells.Add(cell);
                }
                validDepartments.Add(department);
                sb.AppendLine($"Imported {depDto.Name} with {depDto.Cells.Length} cells");

            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonerDto[] prisonersDtos =
                  JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            ICollection<Prisoner> validPrisoners = new HashSet<Prisoner>();

            foreach (var prDto in prisonersDtos)
            {
                if (!IsValid(prDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (prDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                bool isIncarcerationDateIsValid = DateTime.TryParseExact(prDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime incarcerationDate);

                if (!isIncarcerationDateIsValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!String.IsNullOrEmpty(prDto.ReleaseDate))
                {
                    bool isReleaseDateIsValid = DateTime.TryParseExact(prDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDateValue);

                    if (!isReleaseDateIsValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }
                    releaseDate = releaseDateValue;
                }
               
                Prisoner prisoner = new Prisoner()
                {
                    FullName = prDto.FullName,
                    Nickname = prDto.Nickname,
                    Age = prDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prDto.Bail,
                    CellId = prDto.CellId
                };

                foreach (var mailDto in prDto.Mails)
                {
                    Mail mail = Mapper.Map<Mail>(mailDto);
                    prisoner.Mails.Add(mail);
                }
                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prDto.FullName} {prDto.Age} years old");

            }
            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}