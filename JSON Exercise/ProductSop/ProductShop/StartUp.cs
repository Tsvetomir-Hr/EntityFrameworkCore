
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Common;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    public class StartUp
    {


        public static void Main()
        {

            ProductShopContext dbContext = new ProductShopContext();

            string inputJson = File.ReadAllText(ProjectImportPaths.CategoriesImportPath);

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            string output = ImportCategories(dbContext, inputJson);
            Console.WriteLine(output);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<ProductShopProfile>();
            //}));

            var mapper = InitializeMapper();

            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> users = new List<User>();

            foreach (ImportUserDto userDto in userDtos)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }
                User user = mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";

        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var mapper = InitializeMapper();

            ImportProductDto[]? productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }
                Product product = mapper.Map<Product>(productDto);
                products.Add(product);
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";

        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var mapper = InitializeMapper();

            ImportCategoryDto[]? categoriesDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);
            ICollection<Category> categories = new List<Category>();

            foreach (var categoryDto in categoriesDtos)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }
                Category category = mapper.Map<Category>(categoryDto);
                categories.Add(category);

            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }

        private static Mapper InitializeMapper()
        {
            Mapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductShopProfile());

            }));
            return mapper;
        }
    }
}