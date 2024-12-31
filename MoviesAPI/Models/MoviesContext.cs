using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Models
{
    public class MoviesContext:DbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public MoviesContext(DbContextOptions<MoviesContext>options):base(options)
            {
        }

      


    }
}
