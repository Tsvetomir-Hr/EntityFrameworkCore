namespace CodeFirstDbIntro
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            ApplicationContext context = new ApplicationContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();   
        }
    }
}