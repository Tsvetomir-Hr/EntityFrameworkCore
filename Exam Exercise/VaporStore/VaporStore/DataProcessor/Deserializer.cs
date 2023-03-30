namespace VaporStore.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Data;
using Newtonsoft.Json;
using VaporStore.Utilities;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.ImportDto;
using VaporStore.Utilities;

public static class Deserializer
{
    public const string ErrorMessage = "Invalid Data";

    public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

    public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

    public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

    public static string ImportGames(VaporStoreDbContext context, string jsonString)
    {
        ImportGameDto[] gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString)!;
        ICollection<Game> validGames = new HashSet<Game>();
        StringBuilder output = new StringBuilder();
        foreach (var gDto in gameDtos)
        {
            if (!IsValid(gDto))
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            if (gDto.GameTags.Length == 0)
            {
                output.AppendLine(ErrorMessage);
                continue;
            }
            bool isDateValid = DateTime.TryParseExact(gDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
            if (!isDateValid)
            {
                output.AppendLine(ErrorMessage);
                continue;
            }

            Game game = new Game()
            {
                Name = gDto.Name,
                Price = gDto.Price,
                ReleaseDate = releaseDate
            };
            if (!context.Developers.Any(d => d.Name == gDto.DeveloperName))
            {
                Developer NewDeveloper = new Developer()
                {
                    Name = gDto.DeveloperName
                };
                game.Developer = NewDeveloper;
            }
            else
            {
                Developer developer = context.Developers.FirstOrDefault(d => d.Name == gDto.DeveloperName)!;
                game.Developer = developer;
            }
            if (!context.Genres.Any(g => g.Name == gDto.Genre))
            {
                Genre NewGenre = new Genre()
                {
                    Name = gDto.Genre
                };
                game.Genre = NewGenre;
            }
            else
            {
                Genre genre = context.Genres.FirstOrDefault(g => g.Name == gDto.Genre)!;
                game.Genre = genre;
            }

            foreach (var tag in gDto.GameTags)
            {
                Tag newTag = context.Tags.FirstOrDefault(t => t.Name == tag)!;


                if (newTag == null)
                {

                    GameTag gameTag = new GameTag()
                    {
                        Tag = new Tag()
                        {
                            Name = tag
                        }
                        //>>>>>>> maybe i need to add it in the db also <<<<<<<<<
                    };
                    game.GameTags.Add(gameTag);
                    continue;
                }
                GameTag gameTagInDb = new GameTag()
                {
                    Tag = newTag
                };
                game.GameTags.Add(gameTagInDb);

            }
            validGames.Add(game);
            output.AppendLine(String.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));

            context.Games.Add(game);
            context.SaveChanges();
        }

        return output.ToString().TrimEnd();
    }




    public static string ImportUsers(VaporStoreDbContext context, string jsonString)
    {
        ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString)!;

        ICollection<User> validUsers = new HashSet<User>();
        StringBuilder sb = new StringBuilder();
        foreach (var uDto in userDtos)
        {
            if (!IsValid(uDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            if (uDto.Cards.Length == 0)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            if (uDto.Cards.Any(c => !IsValid(c)))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            User user = new User()
            {
                FullName = uDto.FullName,
                Username = uDto.Username,
                Email = uDto.Email,
                Age = uDto.Age,
            };
            ICollection<Card> validcards = new HashSet<Card>();
            foreach (var cDto in uDto.Cards)
            {
                bool isCardTypeValid = Enum.TryParse<CardType>(cDto.Type, out CardType type);

                if (!isCardTypeValid)
                {
                    validcards = new HashSet<Card>();
                    break;
                }

                Card card = new Card()
                {
                    Number = cDto.Number,
                    Cvc = cDto.Cvc,
                    Type = type,
                };
                validcards.Add(card);
            }
            if (validcards.Count == 0)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            user.Cards = validcards;
            validUsers.Add(user);
            sb.AppendLine(String.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
        }
        context.Users.AddRange(validUsers);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
    {
        XmlHelper helper = new XmlHelper();
        ImportPurchaseDto[] purchaseDtos = helper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");
        ICollection<Purchase> validPurchases = new HashSet<Purchase>();
        StringBuilder sb = new StringBuilder();
        foreach (var pDto in purchaseDtos)
        {
            if (!IsValid(pDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            bool isPurchaseTypeValid = Enum.TryParse<PurchaseType>(pDto.Type, out PurchaseType purchaseType);
            if (!isPurchaseTypeValid)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            bool isDateValid = DateTime.TryParseExact(pDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            if (!isDateValid)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            Game game = context.Games.FirstOrDefault(g => g.Name == pDto.GameName);
            if (game == null)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }
            Card card = context.Cards.FirstOrDefault(c => c.Number == pDto.CardNumber);
            if (card == null)
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Purchase purchase = new Purchase()
            {
                Game = game,
                Type = purchaseType,
                ProductKey = pDto.ProductKey,
                Card = card,
                Date = date,
            };
            User user = context.Users.FirstOrDefault(u => u.Cards.Any(c => c.Number == card.Number))!;

            validPurchases.Add(purchase);
            sb.AppendLine(String.Format(SuccessfullyImportedPurchase,purchase.Game.Name,user.Username));
        }
        context.AddRange(validPurchases);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }

}



