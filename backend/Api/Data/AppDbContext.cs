using Api.Authentication.Services;

namespace Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUsersTable(modelBuilder);
            ConfigureClientsTable(modelBuilder);
            ConfigurePaymentsTable(modelBuilder);
            ConfigureRatesTable(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureUsersTable(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<User>();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.HasIndex(x => x.ReferenceId)
                .IsUnique();
        }

        private static void ConfigureClientsTable(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Client>();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.ReferenceId)
                .IsUnique();

            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Client)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigurePaymentsTable(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Payment>();

            builder.HasIndex(x => x.ReferenceId)
                .IsUnique();
        }

        private static void ConfigureRatesTable(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Rate>();

            builder.HasIndex(x => x.ReferenceId)
                .IsUnique();

            builder.HasMany(x => x.Payments)
                .WithOne(x => x.Rate)
                .HasForeignKey(x => x.RateId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
