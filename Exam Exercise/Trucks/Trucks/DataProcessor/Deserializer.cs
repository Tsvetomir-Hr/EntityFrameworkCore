namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            ImportDespatcherDto[] importDespatcherDtos = helper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");
            ICollection<Despatcher> validDespatchers = new HashSet<Despatcher>();

            StringBuilder output = new StringBuilder();
            foreach (var dDto in importDespatcherDtos)
            {
                if (!IsValid(dDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (String.IsNullOrEmpty(dDto.Position))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Despatcher despatcher = new Despatcher()
                {
                    Name = dDto.Name,
                    Position = dDto.Position,
                };

                foreach (var truckDto in dDto.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    bool categoryTypeIsValid = Enum.TryParse<CategoryType>(truckDto.CategoryType, out CategoryType categoryType);
                    if (!categoryTypeIsValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool makeTypeIsValid = Enum.TryParse<MakeType>(truckDto.MakeType, out MakeType makeType);
                    if (!makeTypeIsValid)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (String.IsNullOrEmpty(truckDto.VinNumber))
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (truckDto.VinNumber.Length != 17)
                    {
                        output.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck truck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = categoryType,
                        MakeType = makeType,
                    };
                    despatcher.Trucks.Add(truck);

                }
                validDespatchers.Add(despatcher);
                output.AppendLine(String.Format(SuccessfullyImportedDespatcher, despatcher.Name, despatcher.Trucks.Count));

            }
            context.Despatchers.AddRange(validDespatchers);
            context.SaveChanges();

            return output.ToString().TrimEnd();

        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            ImportClientDto[] clientDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            ICollection<Client> validClients = new HashSet<Client>();
            StringBuilder sb = new StringBuilder();

            foreach (var cDto in clientDtos)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (cDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = cDto.Name,
                    Nationality = cDto.Nationality,
                    Type = cDto.Type,

                };


                foreach (var truckId in cDto.Trucks)
                {
                    if (!context.Trucks.Any(t => t.Id == truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck truck = context.Trucks.Find(truckId);

                    client.ClientsTrucks.Add(new ClientTruck()
                    {
                        Truck = truck
                    });
                }


                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));

            }
            context.Clients.AddRange(validClients);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}