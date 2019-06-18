using Microsoft.EntityFrameworkCore;

namespace ConditionalStringReversal.Entities
{
    public class MortgageConnectDbContext : DbContext
    {
        public MortgageConnectDbContext()
        {
        }

        public MortgageConnectDbContext(DbContextOptions<MortgageConnectDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<StringToReverse> StringToReverse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=MortgageConnectDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<StringToReverse>(entity =>
            {
                entity.Property(e => e.DataValue).HasMaxLength(4000);
            });
        }
    }
}
