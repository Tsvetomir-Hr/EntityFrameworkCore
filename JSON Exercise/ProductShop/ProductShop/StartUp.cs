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
        string inputJson = File.ReadAllText("../../../Datasets/users.json");

        string result = ImportUsers(context, inputJson);

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
}


