namespace ProductShop;

using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

public class StartUp
{

    public static void Main()
    {

        ProductShopContext context = new ProductShopContext();
        string inputJson = File.ReadAllText("../../../Datasets/products.json");

        string result = ImportProducts(context, inputJson);

        Console.WriteLine(result);
    }
    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));

        InportUserDto[] userDtos = JsonConvert.DeserializeObject<InportUserDto[]>(inputJson);

        ICollection<User> validUser = new HashSet<User>();

        foreach (var userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            validUser.Add(user);
        }

        context.Users.AddRange(validUser);
        context.SaveChanges();

        return $"Successfully imported {validUser.Count}";
    }
    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMappper();

        ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);
        ICollection<Product> validProducts = new HashSet<Product>();

        foreach (var productDto in productDtos)
        {
            Product product = mapper.Map<Product>(productDto);
            validProducts.Add(product);
        }
        context.Products.AddRange(validProducts);
        context.SaveChanges();

        return $"Successfully imported {validProducts.Count}";

    }

    private static IMapper CreateMappper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}


