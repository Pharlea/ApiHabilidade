using Microsoft.EntityFrameworkCore;
using RPG_API.Models;

namespace RPG_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Habilidade> TabHabilidades { get; set; }
        public DbSet<User> TabUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("DataSource=app.db; Cache=Shared");
        }
    }
}
