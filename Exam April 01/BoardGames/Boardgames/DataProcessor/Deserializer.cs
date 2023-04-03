namespace Boardgames.DataProcessor;

using Boardgames.Data;
using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor.ImportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedCreator
        = "Successfully imported creator – {0} {1} with {2} boardgames.";

    private const string SuccessfullyImportedSeller
        = "Successfully imported seller - {0} with {1} boardgames.";

    public static string ImportCreators(BoardgamesContext context, string xmlString)
    {
        XmlHelper helper = new XmlHelper();

        ImportCreatorDto[] creatorDtos = helper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

        ICollection<Creator> validCreators = new HashSet<Creator>();
        StringBuilder output = new StringBuilder();
        foreach (var cDto in creatorDtos)
        {
            if (!IsValid(cDto))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            Creator creator = new Creator()
            {
                FirstName = cDto.FirstName,
                LastName = cDto.LastName
            };
            foreach (var bgDto in cDto.Boardgames)
            {
                if (!IsValid(bgDto))
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                Boardgame boardgame = new Boardgame()
                {
                    Name = bgDto.Name,
                    Rating = bgDto.Rating,
                    YearPublished = bgDto.YearPublished,
                    CategoryType = Enum.Parse<CategoryType>(bgDto.CategoryType),
                    Mechanics = bgDto.Mechanics
                };
                creator.Boardgames.Add(boardgame);
            }
            validCreators.Add(creator);
            output.AppendLine(String.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
        }
        context.Creators.AddRange(validCreators);
        context.SaveChanges();

        return output.ToString().TrimEnd();
    }

    public static string ImportSellers(BoardgamesContext context, string jsonString)
    {
        ImportSellerDto[] sellerDtos = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;
        ICollection<Seller> validSellers = new HashSet<Seller>();
        StringBuilder output = new StringBuilder();

        foreach (var sDto in sellerDtos)
        {
            if (!IsValid(sDto))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Seller seller = new Seller()
            {
                Name = sDto.Name,
                Address = sDto.Address,
                Country = sDto.Country,
                Website = sDto.Website
            };

            foreach (var bgDtoId in sDto.Boardgames)
            {
                Boardgame boardgame = context.Boardgames.Find(bgDtoId);

                if (boardgame == null)
                {
                    output.AppendLine(ErrorMessage);
                    continue;
                }

                seller.BoardgamesSellers.Add(new BoardgameSeller()
                {
                    Boardgame = boardgame
                    
                });
            }
            validSellers.Add(seller);

            output.AppendLine(String.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
        }
        context.Sellers.AddRange(validSellers);
        context.SaveChanges();

        return output.ToString().TrimEnd();

    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}


