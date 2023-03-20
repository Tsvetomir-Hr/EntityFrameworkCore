namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper helper = new XmlHelper();

            ImportPlayDto[] playDtos = helper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            ICollection<Play> validPlays = new HashSet<Play>();
            foreach (ImportPlayDto playDto in playDtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool timespanIsValid = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, TimeSpanStyles.None, out TimeSpan duration);

                if (!timespanIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool genreIsValid = Enum.TryParse<Genre>(playDto.Genre, out Genre genre);
                if (!genreIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play play = new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = genre,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter,
                };
                validPlays.Add(play);
                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre} and a rating of {play.Rating}!");
            }
            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper helper = new XmlHelper();

            ImportCastDto[] castDtos = helper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            ICollection<Cast> validCasts = new HashSet<Cast>();

            foreach (ImportCastDto castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };
                validCasts.Add(cast);

                string role = string.Empty;
                if (cast.IsMainCharacter)
                {
                    role = "main";
                }
                else
                {
                    role = "lesser";
                }
                sb.AppendLine($"Successfully imported actor {cast.FullName} as a {role} character!");

            }
            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ImportTheatersTicketsDto[] theaterDtos = JsonConvert.DeserializeObject<ImportTheatersTicketsDto[]>(jsonString);
            ICollection<Theatre> validTheatres = new HashSet<Theatre>();
            foreach (var ttDto in theaterDtos)
            {
                if (!IsValid(ttDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
             
                Theatre theatre = new Theatre()
                {
                    Name = ttDto.Name,
                    NumberOfHalls = ttDto.NumberOfHalls,
                    Director = ttDto.Director
                };

                foreach (var ticketDto in ttDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Ticket ticket = new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                    };
                    theatre.Tickets.Add(ticket);
                }
                validTheatres.Add(theatre);
                sb.AppendLine($"Successfully imported theatre {theatre.Name} with #{theatre.Tickets.Count} tickets!");
            }
            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
