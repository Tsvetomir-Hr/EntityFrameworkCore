
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {

       
        public static void Main()
        {
        
            
            
            ProductShopContext dbContext = new ProductShopContext();

            string inputJson = File.ReadAllText("../../../Datasets/users.json");


            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            string output = ImportUsers(dbContext, inputJson);
            Console.WriteLine(output);
        }

       
    }
}