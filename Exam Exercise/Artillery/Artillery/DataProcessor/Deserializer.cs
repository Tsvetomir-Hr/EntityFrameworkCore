namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Footballers.Utilities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();

            ImportCountryDto[] countryDtos = helper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            StringBuilder output = new StringBuilder();
            ICollection<Country> validCountries = new HashSet<Country>();
            foreach (var cDto in countryDtos)
            {
                if (!IsValid(cDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Country country = new Country()
                {
                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize,
                };
                validCountries.Add(country);
                output.AppendLine($"Successfully import {country.CountryName} with {country.ArmySize} army personnel.");
            }
            context.AddRange(validCountries);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            ImportManufacturerDto[] manufacturerDtos = helper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");
            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();
            StringBuilder output = new StringBuilder();
            foreach (var mDto in manufacturerDtos)
            {
                if (!IsValid(mDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded
                };
                if (validManufacturers.Any(m => m.ManufacturerName == manufacturer.ManufacturerName))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                if (context.Manufacturers.Any(m => m.ManufacturerName == manufacturer.ManufacturerName))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                string[] address = mDto.Founded.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string townName = address[address.Length - 2];
                string countryName = address[address.Length - 1];
                validManufacturers.Add(manufacturer);
                output.AppendLine($"Successfully import manufacturer {manufacturer.ManufacturerName} founded in {townName}, {countryName}.");
            }
            context.AddRange(validManufacturers);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            ImportShellDto[] shellDtos = helper.Deserialize<ImportShellDto[]>(xmlString, "Shells");
            ICollection<Shell> validShells = new HashSet<Shell>();

            StringBuilder output = new StringBuilder();
            foreach (var slDto in shellDtos)
            {
                if (!IsValid(slDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                Shell shell = new Shell()
                {
                    ShellWeight = slDto.ShellWeight,
                    Caliber = slDto.Caliber
                };
                validShells.Add(shell);
                output.AppendLine($"Successfully import shell caliber #{shell.Caliber} weight {shell.ShellWeight} kg.");
            }
            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return output.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            ImportGunDto[] gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            ICollection<Gun> validGuns = new HashSet<Gun>();
            StringBuilder output = new StringBuilder();
            foreach (var gDto in gunDtos)
            {
                if (!IsValid(gDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }
                bool gunTypeIsValid = Enum.TryParse<GunType>(gDto.GunType, out GunType guntype);
                if (!gunTypeIsValid)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = gDto.ManufacturerId,
                    GunWeight = gDto.GunWeight,
                    BarrelLength = gDto.BarrelLength,
                    NumberBuild = gDto.NumberBuild,
                    Range = gDto.Range,
                    GunType = guntype,
                    ShellId = gDto.ShellId,
                };
                foreach (var countryDto in gDto.Countries)
                {
                    CountryGun country = new CountryGun()
                    {
                        CountryId = countryDto.Id
                    };
                    gun.CountriesGuns.Add(country);
                }
                validGuns.Add(gun);
                output.AppendLine($"Successfully import gun {gun.GunType} with a total weight of {gun.GunWeight} kg. and barrel length of {gun.BarrelLength} m.");
            }
            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return output.ToString().TrimEnd();
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