using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Xml.Serialization;

namespace CarDealer;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext dbContext = new CarDealerContext();

        //string xmlSupp = File.ReadAllText("../../../Datasets/suppliers.xml");
        //string result = ImportSuppliers(dbContext, xmlSupp);

        //string xmlParts = File.ReadAllText("../../../Datasets/parts.xml");
        //string result = ImportParts(dbContext, xmlParts);

        string xmlCars = File.ReadAllText("../../../Datasets/cars.xml");
        string result = ImportCars(dbContext, xmlCars);

        Console.WriteLine(result);

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

    }



    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers"); // root

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

        using StringReader xmlReader = new StringReader(inputXml);

        ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer
            .Deserialize(xmlReader);

        Supplier[] suppliers = dtos.Select(s => new Supplier
        {
            Name = s.Name,
            IsImporter = s.IsisImporter
        })
            .ToArray();

        context.Suppliers.AddRange(suppliers);
        context.SaveChanges();

        return $"Successfully imported {suppliers.Length}";
    }
    public static string ImportParts(CarDealerContext context, string inputXml)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), xmlRoot);

        using StringReader xmlReader = new StringReader(inputXml);

        ImportPartDto[] partDtos = (ImportPartDto[])xmlSerializer.Deserialize(xmlReader);

        ICollection<Part> parts = new List<Part>();

        foreach (ImportPartDto partDto in partDtos)
        {

            if (!context.Suppliers.Any(s => s.Id == partDto.SupplierId))
            {
                continue;
            }

            Part part = new Part()
            {
                Name = partDto.Name,
                Price = partDto.Price,
                Quantity = partDto.Quantity,
                SupplierId = partDto.SupplierId
            };

            parts.Add(part);
        }

        context.Parts.AddRange(parts);
        context.SaveChanges();

        return $"Successfully imported {parts.Count}";
    }

    public static string ImportCars(CarDealerContext context, string inputXml)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute("Cars");
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDto[]), xmlRoot);

        StringReader xmlReader = new StringReader(inputXml);

        ImportCarDto[] carDtos = (ImportCarDto[])xmlSerializer.Deserialize(xmlReader);

        ICollection<Car> validCars = new List<Car>();

        foreach (ImportCarDto cDto in carDtos)
        {
            Car car = new Car()
            {
                Make = cDto.Make,
                Model = cDto.Model,
                TravelledDistance = cDto.TraveledDistance
            };
            ICollection<PartCar> currentCarParts = new List<PartCar>();

            foreach (int partId in cDto.Parts.Select(p => p.Id).Distinct())
            {
                if (!context.Parts.Any(p => p.Id == partId))
                {
                    continue;
                }
                currentCarParts.Add(new PartCar()
                {
                    Car = car,
                    PartId = partId
                });
            }

            car.PartsCars = currentCarParts;

            validCars.Add(car);
        }
        context.Cars.AddRange(validCars);
        context.SaveChanges();

        return $"Successfully imported {validCars.Count}";

    }

    private static T Desirialize<T>(string inputXml, string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName); // root
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

        using StringReader xmlReader = new StringReader(inputXml);
        T dtos = (T)xmlSerializer
            .Deserialize(xmlReader);

        return dtos;
    }
}