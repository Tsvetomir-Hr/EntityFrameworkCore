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
        string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

        string result = ImportCategoryProducts(context, inputJson);

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
    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMappper();
        ImportCategorieDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategorieDto[]>(inputJson);
        ICollection<Category> validCategories = new HashSet<Category>();
        foreach (var categoryDto in categoryDtos)
        {
            if (String.IsNullOrEmpty(categoryDto.Name))
            {
                continue;
            }
            Category category = mapper.Map<Category>(categoryDto);
            validCategories.Add(category);
        }
        context.Categories.AddRange(validCategories);
        context.SaveChanges();

        return $"Successfully imported {validCategories.Count}";
    }
    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMappper();

        ImportCategoryProductsDto[] categortyProductsDtos = JsonConvert
            .DeserializeObject<ImportCategoryProductsDto[]>(inputJson);

        ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();

        foreach (var cpDto in categortyProductsDtos)
        {
            if (!context.Categories.Any(c => c.Id == cpDto.CategoryId) ||
                !context.Products.Any(p => p.Id == cpDto.ProductId))
            {
                continue;
            }
            CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(cpDto);
            validCategoryProducts.Add(categoryProduct);

        }
        context.CategoriesProducts.AddRange(validCategoryProducts);
        context.SaveChanges();

        return $"Successfully imported {validCategoryProducts.Count}";

    }

    private static IMapper CreateMappper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}


