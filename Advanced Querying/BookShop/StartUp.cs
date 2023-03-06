namespace BookShop;

using BookShop.Models.Enums;
using Data;
using Initializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text;

public class StartUp
{
    public static void Main()
    {
        using var context = new BookShopContext();
        DbInitializer.ResetDatabase(context);

        string input = Console.ReadLine();

        string result = GetBooksByCategory(context, input);

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
            ((int)b.EditionType) == 2)
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

    public static string GetBooksByPrice(BookShopContext context)
    {
        StringBuilder sb = new StringBuilder();

        var books = context.Books
            .Select(b => new
            {
                BookTitle = b.Title,
                b.Price
            })
            .Where(b => b.Price > 40)
            .OrderByDescending(b => b.Price)
            .ToArray();

        foreach (var book in books)
        {
            sb.AppendLine($"{book.BookTitle} - ${book.Price:F2}");
        }
        return sb.ToString().TrimEnd();
    }

    public static string GetBooksNotReleasedIn(BookShopContext context, int year)
    {
        StringBuilder sb = new StringBuilder();

        string[] books = context.Books
            .OrderBy(b => b.BookId)
            .Where(b => b.ReleaseDate.Value.Year != year)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, books);
    }

    public static string GetBooksByCategory(BookShopContext context, string input)
    {
        string[] categories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(bt => bt.ToLower())
            .ToArray();


        string[] output = context.Books
            .Where(b => b.BookCategories
            .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToArray();

        return string.Join(Environment.NewLine, output);

    }

    public static string GetBooksReleasedBefore(BookShopContext context, string date)
    {
        StringBuilder output = new StringBuilder();

        string[] tokens = date.Split('-', StringSplitOptions.RemoveEmptyEntries);
        int day = int.Parse(tokens[0]);
        int month = int.Parse(tokens[1]);
        int year = int.Parse(tokens[2]);
        DateTime inputDate = new DateTime(year, month, day);
        var books = context.Books
            .OrderByDescending(b => b.ReleaseDate)
            .Where(b => b.ReleaseDate < inputDate)
            .Select(b => new
            {
                BookTitle = b.Title,
                BookEdtiton = b.EditionType.ToString(),
                BookPrice = b.Price
            })
            .ToArray();
        foreach (var book in books)
        {
            output.AppendLine($"{book.BookTitle} - {book.BookEdtiton} - ${book.BookPrice:F2}");
        }

        return output.ToString().TrimEnd();
    }

    public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
    {
        StringBuilder sb = new StringBuilder();

        var authors = context.Authors
            .Where(a => a.FirstName.EndsWith(input))
            .Select(a => new
            {
                FullName = a.FirstName + " " + a.LastName
            })
            .OrderBy(a => a.FullName)
            .ToArray();

        foreach (var author in authors)
        {
            sb.AppendLine(author.FullName);
        }
        return sb.ToString().TrimEnd();
    }

    public static string GetBookTitlesContaining(BookShopContext context, string input)
    {
        StringBuilder sb = new StringBuilder();

        var booksTitles = context.Books
            .Where(b => b.Title.ToUpper().Contains(input.ToUpper()))
            .Select(b => new
            {
                Title = b.Title
            })
            .OrderBy(b => b.Title)
            .ToArray();

        foreach (var book in booksTitles)
        {
            sb.AppendLine(book.Title);
        }
        return sb.ToString().TrimEnd();
    }

    public static string GetBooksByAuthor(BookShopContext context, string input)
    {
        StringBuilder sb = new StringBuilder();

        var info = context.Books
            .Select(b => new
            {
                BookId = b.BookId,
                BookTitle = b.Title,
                BookAuthor = b.Author
            })
            .Where(b => b.BookAuthor.LastName.ToUpper().StartsWith(input.ToUpper()))
            .OrderBy(b => b.BookId)
            .ToArray();

        foreach (var book in info)
        {
            sb.AppendLine($"{book.BookTitle} ({book.BookAuthor.FirstName} {book.BookAuthor.LastName})");
        }
        return sb.ToString().TrimEnd();
    }

    public static int CountBooks(BookShopContext context, int lengthCheck)
    {
        var numberOfBooks = context.Books
            .ToArray()
            .Count(b => b.Title.ToString().Length > lengthCheck);

        return numberOfBooks;

    }
}


