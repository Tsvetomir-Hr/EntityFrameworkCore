namespace Trucks.DataProcessor
{
    using Data;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper helper = new XmlHelper();
            var despatchers = context.Despatchers
                .ToArray()
                .Where(d => d.Trucks.Count >= 1)
                .Select(d => new ExportDespatcherDto()
                {
                    Name = d.Name,
                    TrucksCount = d.Trucks.Count,
                    Trucks = d.Trucks
                    .Select(t => new ExportTruckDto()
                    {
                        RegistrationNumber = t.RegistrationNumber,
                        MakeType = t.MakeType
                    })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.Name)
                .ToArray();

            return helper.Serialize<ExportDespatcherDto[]>(despatchers, "Despatchers");


        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var client = context.Clients
                 .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                 .Select(c => new
                 {
                     Name = c.Name,
                     Trucks = c.ClientsTrucks
                     .Select(cl => cl.Truck)
                     .Where(t => t.TankCapacity >= capacity)
                     .OrderBy(t => t.MakeType)
                     .ThenByDescending(t => t.CargoCapacity)
                     .Select(t => new
                     {
                         TruckRegistrationNumber = t.RegistrationNumber,
                         VinNumber = t.VinNumber,
                         TankCapacity = t.TankCapacity,
                         CargoCapacity = t.CargoCapacity,
                         CategoryType = t.CategoryType.ToString(),
                         MakeType = t.MakeType.ToString()
                     })
                     .ToArray()
                 })
                 .OrderByDescending(c => c.Trucks.Count())
                 .ThenBy(c => c.Name)
                 .Take(10)
                 .ToArray();

            return JsonConvert.SerializeObject(client, Formatting.Indented);




        }
    }
}
