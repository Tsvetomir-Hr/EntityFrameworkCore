using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using System.IO;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {

            CarDealerContext context = new CarDealerContext();
            string inputXml = File.ReadAllText("../../../Datasets/parts.xml");

            string result = ImportParts(context, inputXml);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = InitializeMapper();
            XmlHelper helper = new XmlHelper();

            ImportSupplierDto[] supplierDtos = helper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

            foreach (var suppDto in supplierDtos)
            {
                if (String.IsNullOrEmpty(suppDto.Name))
                {
                    continue;
                }
                //Manual mapping for simple models

                //Supplier supplier = new Supplier()
                //{
                //    Name = suppDto.Name,
                //    IsImporter = suppDto.IsImporter
                //};

                //With AutoMapper
                Supplier supplier = mapper.Map<Supplier>(suppDto);

                validSuppliers.Add(supplier);
            }
            context.Suppliers.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlHelper helper = new XmlHelper();

            ImportPartDto[] partsDtos = helper.Deserialize<ImportPartDto[]>(inputXml, "Parts");
            ICollection<Part> validParts = new HashSet<Part>();
            foreach (var partDto in partsDtos)
            {
                if (!partDto.SupplierId.HasValue ||
                    !context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    continue;
                }
                Part part = new Part()
                {
                    Name = partDto.Name,
                    Price = partDto.Price,
                    Quantity = partDto.Quantity,
                    SupplierId = (int)partDto.SupplierId
                };
                validParts.Add(part);
            }
            context.Parts.AddRange(validParts);
            context.SaveChanges();  
            return $"Successfully imported {validParts.Count}";

        }
        private static IMapper InitializeMapper()
       => new Mapper(new MapperConfiguration(cfg =>
       {
           cfg.AddProfile<CarDealerProfile>();
       }));
    }
}