namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlHelper helper = new XmlHelper();
            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(c => new ExportCoachDto()
                {
                    FootballersCount = c.Footballers.Count(),
                    Name = c.Name,
                    Footballers = c.Footballers
                    .Select(f => new ExportFootballerDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString(),
                    })
                    .OrderBy(f => f.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Count())
                .ThenBy(c => c.Name)
                .ToArray();

            return helper.Serialize(coaches, "Coaches");

        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .ToArray()
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()
                    })
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
