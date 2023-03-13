namespace ProductShop;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

public class StartUp
{

    public static void Main()
    {

        ProductShopContext context = new ProductShopContext();


        string result = GetProductsInRange(context);

        Console.WriteLine(result);
    }

    //Imports
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


    //Exports
    public static string GetProductsInRange(ProductShopContext context)
    {
        var prodcuts = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Select(p => new
            {
                name = p.Name,
                price = p.Price,
                seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(prodcuts, Formatting.Indented);
    }


    private static IMapper CreateMappper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}


