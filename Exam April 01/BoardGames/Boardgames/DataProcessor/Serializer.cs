namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Newtonsoft.Json;
    using System.Xml;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .ToArray()
                .Select(c => new ExportCreatorDto()
                {
                    BoardgamesCount = c.Boardgames.Count(),
                    FullName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                    .Select(bg => new ExportCreatorBoeardgameDto()
                    {
                        Name = bg.Name,
                        YearPublished = bg.YearPublished,
                    })
                    .OrderBy(bg => bg.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.FullName)
                .ToArray();


            XmlHelper helper = new XmlHelper();
            return helper.Serialize(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating))
                .ToArray()
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                    .ToArray()
                    .Select(bg => new
                    {
                        Name = bg.Boardgame.Name,
                        Rating = bg.Boardgame.Rating,
                        Mechanics = bg.Boardgame.Mechanics,
                        Category = bg.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(bg => bg.Rating)
                    .ThenBy(bg => bg.Name)

                })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Newtonsoft.Json.Formatting.Indented);
        }
    }
}