namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string? input = Console.ReadLine();
            string result = GetBooksByAgeRestriction(db, input);
            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            var books = context.Books
                .OrderBy(b => b.Title)
                .ToArray()
                .Select(b => new
                {
                    AgerestrictionTostring = b.AgeRestriction.ToString().ToUpper(),
                    BookTitle = b.Title,
                })
                .Where(b => b.AgerestrictionTostring == command.ToUpper())
                .ToArray();
                
               
                
                
            foreach ( var book in books ) 
            {
                stringBuilder.AppendLine( book.BookTitle );
            }

            return stringBuilder.ToString().TrimEnd();
        }

    }
}


