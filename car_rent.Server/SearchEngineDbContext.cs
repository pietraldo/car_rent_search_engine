using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace car_rent.Server.Model
{
    public class SearchEngineDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Rent> History { get; set; }
        public DbSet<Offer> Offers { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Car> Cars { get; set; }

        public SearchEngineDbContext(DbContextOptions<SearchEngineDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Rent>()
                .HasKey(rent => rent.Rent_ID);
            modelBuilder.Entity<Offer>()
                .HasKey(offer => offer.Offer_ID);
            modelBuilder.Entity<Company>()
                .HasKey(company => company.Company_ID);
            modelBuilder.Entity<Car>()
                .HasKey(car => car.Car_ID);

            // Foreign keys for the tabels

            // one-to-many rent-user
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.User)
                .WithMany(user => user.Rents)
                .HasForeignKey(rent => rent.User_ID);

            // one-to-one rent-offer
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Offer)
                .WithOne(offer => offer.Rent)
                .HasForeignKey<Rent>(rent => rent.Offer_ID);

            // one-to-many rent-company
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Company)
                .WithMany(company => company.Rents)
                .HasForeignKey(rent => rent.Company_ID);

            // one-to-many offer-car
            modelBuilder.Entity<Offer>()
                .HasOne(offer => offer.Car)
                .WithMany(car => car.Offers)
                .HasForeignKey(offer => offer.Car_ID)
                .OnDelete(DeleteBehavior.Restrict);
        }
      
    }
}
