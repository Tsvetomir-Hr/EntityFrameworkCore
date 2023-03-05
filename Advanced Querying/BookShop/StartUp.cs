namespace BookShop;

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

        string result = GetGoldenBooks(db);
        Console.WriteLine(result);
    }



    //public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    //{
    //    StringBuilder stringBuilder = new StringBuilder();

    //    var books = context.Books
    //        .OrderBy(b => b.Title)
    //        .ToArray()
    //        .Select(b => new
    //        {
    //            AgerestrictionTostring = b.AgeRestriction.ToString().ToUpper(),
    //            BookTitle = b.Title,
    //        })
    //        .Where(b => b.AgerestrictionTostring == command.ToUpper())
    //        .ToArray();




    //    foreach ( var book in books ) 
    //    {
    //        stringBuilder.AppendLine( book.BookTitle );
    //    }

    //    return stringBuilder.ToString().TrimEnd();
    //}
    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        StringBuilder stringBuilder = new StringBuilder();

        AgeRestriction enumToString;

        bool isParseSuccessed = Enum.TryParse<AgeRestriction>(command, true, out enumToString);

        if (!isParseSuccessed)
        {
            return string.Empty;
        }

        var books = context.Books
            .Where(b => b.AgeRestriction == enumToString)
            .Select(b => new
            {
                BookTitle = b.Title
            })
            .OrderBy(b => b.BookTitle)
            .ToArray();


        foreach (var book in books)
        {
            stringBuilder.AppendLine(book.BookTitle);
        }

        return stringBuilder.ToString().TrimEnd();
    }

    public static string GetGoldenBooks(BookShopContext context)
    {
        StringBuilder sb = new StringBuilder();
      
        var books = context.Books
            .Where(b => b.Copies < 5000 &&
            ((int)b.EditionType)==2)
            .OrderBy(b => b.BookId)
            .Select(b => new
            {
                BookTitle = b.Title
            })
            .ToArray();

        foreach (var book in books)
        {
            sb.AppendLine(book.BookTitle);
        }
        return sb.ToString().TrimEnd();
    }

}


