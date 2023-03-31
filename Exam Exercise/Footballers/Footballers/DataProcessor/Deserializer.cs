namespace Footballers.DataProcessor
{

    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            ImportCoachDto[] coachDtos = helper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches")!;
            ICollection<Coach> validCoaches = new HashSet<Coach>();

            StringBuilder output = new StringBuilder();

            foreach (var cDto in coachDtos)
            {
                if (!IsValid(cDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Coach coach = new Coach()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality
                };
                foreach (var fDto in cDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isStartDateValid = DateTime.TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractStartDate);
                    if (!isStartDateValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isEndDateValid = DateTime.TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractEndDate);
                    if (!isEndDateValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (contractStartDate > contractEndDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = Enum.Parse<BestSkillType>(fDto.BestSkillType),
                        PositionType = Enum.Parse<PositionType>(fDto.PositionType)
                    };
                    coach.Footballers.Add(footballer);
                }
                validCoaches.Add(coach);
                output.AppendLine(String.Format(SuccessfullyImportedCoach,coach.Name,coach.Footballers.Count));

            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            ImportTeamDto[] teamDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString)!;
            ICollection<Team> validTeams = new HashSet<Team>();
            StringBuilder output = new StringBuilder();

            foreach (var tDto in teamDtos)
            {
                if (!IsValid(tDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (tDto.Trophies == 0)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Team team = new Team()
                {
                    Name = tDto.Name,
                    Nationality = tDto.Nationality,
                    Trophies = tDto.Trophies

                };
                foreach (var fDtoId in tDto.Footballers)
                {
                    Footballer footballer = context.Footballers.Find(fDtoId);
                    if (footballer == null)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    team.TeamsFootballers.Add(new TeamFootballer()
                    {
                        Footballer = footballer
                    });

                }
                validTeams.Add(team);
                output.AppendLine(string.Format(SuccessfullyImportedTeam,team.Name,team.TeamsFootballers.Count));

            }
            context.Teams.AddRange(validTeams);
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
}
