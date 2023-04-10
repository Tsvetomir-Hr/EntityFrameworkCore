namespace MusicHub;

using System;
using System.Globalization;
using System.Text;

using Data;

using Initializer;


public class StartUp
{
    public static void Main()
    {
        MusicHubDbContext context =
            new MusicHubDbContext();

        DbInitializer.ResetDatabase(context);

        //Test your solutions here
        string result = ExportAlbumsInfo(context, 9);
        Console.WriteLine(result);
    }

    public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
    {
        StringBuilder sb = new StringBuilder();
        var albums = context.Albums
            .Where(a => a.ProducerId.HasValue &&
                      a.ProducerId.Value == producerId)
            .ToArray()
            .OrderByDescending(a=>a.Price)
             .Select(a => new
             {
                 a.Name,
                 ReleaseDate = a.ReleaseDate
                 .ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                 ProducerName = a.Producer.Name,
                 Songs = a.Songs
                     .Select(s => new
                     {
                         SongName = s.Name,
                         Price = s.Price.ToString("F2"),
                         Writer = s.Writer.Name
                     })
                     .OrderByDescending(s => s.SongName)
                     .ThenBy(s => s.Writer)
                     .ToArray(),
                 AlbumPrice = a.Price.ToString("f2")
             })
             .ToArray();

        foreach (var album in albums)
        {
            sb
                .AppendLine($"- AlbumName: {album.Name}")
                .AppendLine($"- ReleaseDate: {album.ReleaseDate}")
                .AppendLine($"- ProducerName: {album.ProducerName}")
                .AppendLine($"-  Songs:    ");

            int songNumber = 1;
            foreach ( var s in album.Songs)
            {
                sb
                    .AppendLine($"---#{songNumber}")
                    .AppendLine($"---SongName: {s.SongName}")
                    .AppendLine($"---Price: {s.Price}")
                    .AppendLine($"---Writer: {s.Writer}");
                songNumber++;
            }
            sb.AppendLine($"-AlbumPrice: {album.AlbumPrice}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
    {
        throw new NotImplementedException();
    }
}
