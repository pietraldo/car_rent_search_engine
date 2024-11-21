using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace car_rent_api2.Server.Database
{
    public class SearchEngineDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Rent> History { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Company>  Companies { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Rent>()
                .HasKey(rent => rent.Rent_ID);
            modelBuilder.Entity<Offer>()
                .HasKey(offer => offer.Offer_ID);
            modelBuilder.Entity<Company>()
                .HasKey(company => company.Company_ID);

            // Foreign keys for the tabels

            // one-to-many rent-user
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.User)
                .WithMany(user => user.Rents)
                .HasForeignKey(rent => rent.User_ID);

            // one-to-one history-offer
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Offer)
                .WithOne(offer => offer.Rent)
                .HasForeignKey<Rent>(rent => rent.Offer_ID);

            // one-to-one history-company
            modelBuilder.Entity<Rent>()
                .HasOne(rent => rent.Company)
                .WithOne(company => company.Rent)
                .HasForeignKey<Rent>(rent => rent.Company_ID);


        }
        public SearchEngineDbContext(DbContextOptions<SearchEngineDbContext> options) : base(options) { }
    }
}
