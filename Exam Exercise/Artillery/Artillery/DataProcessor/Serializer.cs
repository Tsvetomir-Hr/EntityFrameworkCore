
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                 .Where(s => s.ShellWeight > shellWeight)
                 .Select(s => new
                 {
                     ShellWeight = s.ShellWeight,
                     Caliber = s.Caliber,
                     Guns = s.Guns
                     .Where(g => g.GunType.Equals(GunType.AntiAircraftGun))
                     .Select(g => new
                     {
                         GunType = g.GunType.ToString(),
                         GunWeight = g.GunWeight,
                         BarrelLength = g.BarrelLength,
                         Range = g.Range > 3000 ? "Long-range" : "Regular range"
                     })
                     .OrderByDescending(g => g.GunWeight)
                     .ToList()
                 })
                 .OrderBy(s => s.ShellWeight)
                 .ToList();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            ExportGunDto[] guns = context.Guns
                 .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                 .Select(g => new ExportGunDto()
                 {
                     ManufacturerName = g.Manufacturer.ManufacturerName,
                     GunType = g.GunType.ToString(),
                     GunWeight = g.GunWeight,
                     BarrelLength = g.BarrelLength,
                     Range = g.Range,
                     CountriesGuns = g.CountriesGuns
                     .Where(c => c.GunId == g.Id && c.Country.ArmySize > 4500000)
                     .Select(c => new ExpoertContriesDto
                     {
                         CountryName = c.Country.CountryName,
                         ArmySize = c.Country.ArmySize,

                     })
                     .OrderBy(c => c.ArmySize)
                     .ToArray()
                 })
                 .OrderBy(g => g.BarrelLength)
                 .ToArray();

            XmlHelper helper = new XmlHelper();
            string xml = helper.Serialize<ExportGunDto[]>(guns, "Guns");
            return xml;
        }
    }
}
