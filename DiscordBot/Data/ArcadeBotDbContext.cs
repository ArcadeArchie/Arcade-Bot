using ArcadeBot.Infrastructure.Entities;
using ArcadeBot.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ArcadeBot.Data
{
    public partial class ArcadeBotDbContext : DbContext
    {
        public virtual DbSet<UserMessage> CachedMessages { get; set; } = null!;
        public virtual DbSet<Server> Servers { get; set; } = null!;
        public virtual DbSet<ServerConfigOverride> ServerConfigOverride { get; set; } = null!;
        public virtual DbSet<ServerUser> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "arcade_bot.db"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            optionsBuilder.UseSqlite(connection);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerUser>().
                HasMany(x => x.Guilds)
                .WithMany(x => x.Users);

            modelBuilder.Entity<ServerConfigOverride>()
                .HasOne(@override => @override.Server)
                .WithMany(server => server.ConfigOverrides);

            base.OnModelCreating(modelBuilder);
        }
    }
}
