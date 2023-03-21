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
    using System.Xml.Serialization;

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

            ImportCoachDto[] importCoachDtos = helper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            ICollection<Coach> validCoachesWithFb = new HashSet<Coach>();

            StringBuilder output = new StringBuilder();
            foreach (ImportCoachDto coachDto in importCoachDtos)
            {
                if (!IsValid(coachDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                };
                foreach (ImportFootballerDto fDto in coachDto.Footballers)
                {
                    if (!IsValid(fDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isContractStartDateIsValid = DateTime.TryParseExact(fDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractStartDate);
                    if (!isContractStartDateIsValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool isContractInfoEndDateIsValid = DateTime.TryParseExact(fDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractEndDate);
                    if (!isContractInfoEndDateIsValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractEndDate < contractStartDate)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isSkillValid = Enum.TryParse<BestSkillType>(fDto.BestSkillType, out BestSkillType bestSkill);
                    if (!isSkillValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isPositionValid = Enum.TryParse<PositionType>(fDto.PositionType, out PositionType position);
                    if (!isPositionValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Footballer footballer = new Footballer()
                    {
                        Name = fDto.Name,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate,
                        BestSkillType = bestSkill,
                        PositionType = position,
                    };
                    coach.Footballers.Add(footballer);

                }

                validCoachesWithFb.Add(coach);
                output.AppendLine($"Successfully imported coach - {coach.Name} with {coach.Footballers.Count} footballers.");
            }

            context.AddRange(validCoachesWithFb);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder output = new StringBuilder();
            ImportTeamWithFootballersDto[] teamDtos = JsonConvert.DeserializeObject<ImportTeamWithFootballersDto[]>(jsonString);

            ICollection<Team> validTeams = new HashSet<Team>();

            foreach (var teamDto in teamDtos)
            {
                if (!IsValid(teamDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (int.Parse(teamDto.Trophies) < 1)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = int.Parse(teamDto.Trophies),

                };
                foreach (var fDto in teamDto.FootballersIds)
                {
                    if (!IsValid(fDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!context.Footballers.Any(f => f.Id == fDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    TeamFootballer footballer = new TeamFootballer()
                    {
                        FootballerId = fDto
                    };

                    team.TeamsFootballers.Add(footballer);
                }
                validTeams.Add(team);
                output.AppendLine($"Successfully imported team - {team.Name} with {team.TeamsFootballers.Count} footballers.");

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
