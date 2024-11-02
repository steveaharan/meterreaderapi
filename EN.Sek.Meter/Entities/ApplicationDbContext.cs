using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.Entities
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Account> Account { get; set; }
		public DbSet<MeterReading> MeterReading { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MeterReading>()
				.HasOne(m => m.Account)
				.WithMany(a => a.MeterReadings)
				.HasForeignKey(m => m.AccountId);

			base.OnModelCreating(modelBuilder);
		}
	}
}