using Microsoft.EntityFrameworkCore;

namespace startApp.Classes
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> users{get;set;}
        public DbSet<Oglas> oglasi{get;set;}
        public DbSet<Request> requests{get;set;}
        public DbSet<Response> responses{get;set;}

        public DbSet<Komentar> komentari { get;set;}
        public DbSet<Ocena> ocene{ get;set;}
        public DbSet<FavoriteMobilni> favorites{get;set;}
        public DbSet<Mobilni> mobilni{get;set;}
    }
}