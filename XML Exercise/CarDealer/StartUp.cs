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

        string xml = File.ReadAllText("../../../Datasets/parts.xml");

        string result = ImportParts(dbContext, xml);
        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        Console.WriteLine(result);
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