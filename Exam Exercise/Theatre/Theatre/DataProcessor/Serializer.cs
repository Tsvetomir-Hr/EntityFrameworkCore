namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theaters = context.Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                    .Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                    .Select(p => p.Price)
                    .Sum(),
                    Tickets = t.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5)
                    .Select(t => new
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber,
                    })
                    .OrderByDescending(t => t.Price)
                    .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string json = JsonConvert.SerializeObject(theaters, Formatting.Indented);
            return json;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            ExportPlayDto[] plays = context.Plays
                .ToArray()
                .Where(p => p.Rating <= raiting)  
                .Select(p => new ExportPlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Casts = p.Casts
                    .Where(c => c.IsMainCharacter)
                    .Select(c => new ExportCastDto()
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            XmlHelper helper = new XmlHelper();

            return helper.Serialize<ExportPlayDto[]>(plays, "Plays");



            //StringBuilder stringBuilder = new StringBuilder();

            //using StringWriter stringWriter = new StringWriter(stringBuilder);


            //XmlRootAttribute root = new XmlRootAttribute("Plays");

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportPlayDto[]), root);

            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            //namespaces.Add(string.Empty, string.Empty);

            //xmlSerializer.Serialize(stringWriter, plays,namespaces);

            //return stringBuilder.ToString().TrimEnd();
        }
    }
}
