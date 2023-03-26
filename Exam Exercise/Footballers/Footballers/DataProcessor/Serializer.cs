﻿namespace Footballers.DataProcessor;

using Data;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ExportDto;
using Footballers.Utilities;
using Newtonsoft.Json;
using System.Globalization;

public class Serializer
{
    public static string ExportCoachesWithTheirFootballers(FootballersContext context)
    {
      
        var coaches = context.Coaches
             .Where(c => c.Footballers.Any())
             .ToArray()
             .Select(c => new ExportCoachDto()
             {
                 Name = c.Name,
                 FootballersCount = c.Footballers.Count(),
                 Footballers = c.Footballers
                 .Select(f => new ExportCoachFootballerDto()
                 {
                     Name = f.Name,
                     PositionType = f.PositionType.ToString()
                 })
                 .OrderBy(f => f.Name)
                 .ToArray()
             })
             .OrderByDescending(c=>c.Footballers.Count())
             .OrderBy(c => c.Name)
             .ToArray();

        XmlHelper helper = new XmlHelper();
        return helper.Serialize<ExportCoachDto[]>(coaches, "Coaches");
    }

    public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
    {
        var teams = context.Teams
            .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
            .ToArray()
            .Select(t => new
            {
                Name = t.Name,
                Footballers = t.TeamsFootballers
                .Where(tf => tf.Footballer.ContractStartDate >= date)
                .ToArray()
                .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                .ThenBy(tf => tf.Footballer.Name)
                .Select(tf => new
                {
                    FootballerName = tf.Footballer.Name,
                    ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                    ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                    BestSkillType = tf.Footballer.BestSkillType.ToString(),
                    PositionType = tf.Footballer.PositionType.ToString(),
                })
                .ToArray()
            })
            .OrderByDescending(t => t.Footballers.Length)
            .ThenBy(t => t.Name)
            .Take(5)
            .ToArray();

        return JsonConvert.SerializeObject(teams, Formatting.Indented);
    }
}