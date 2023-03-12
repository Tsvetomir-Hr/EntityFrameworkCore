using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext dbContext = new CarDealerContext();

            string xml = File.ReadAllText("../../../Datasets/suppliers.xml");

            string result = ImportSuppliers(dbContext, xml);
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
}