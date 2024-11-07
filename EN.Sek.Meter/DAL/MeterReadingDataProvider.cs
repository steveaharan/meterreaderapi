using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public class MeterReadingDataProvider : IMeterReadingDataProvider
	{
		private readonly ApplicationDbContext _context;
		private readonly IServiceProvider _serviceProvider;

		public MeterReadingDataProvider(ApplicationDbContext context, IServiceProvider serviceProvider)
		{
			_context = context;
			_serviceProvider = serviceProvider;
		}

		public async Task<MeterReading> GetMeterReadingByIdAsync(int id)
		{
			var meterReading = await _context.MeterReading
				.Include(x => x.Account)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (meterReading == null)
			{
				throw new InvalidOperationException($"Meter reading with ID {id} not found.");
			}

			return meterReading;
		}

		public async Task<bool> MeterReadingExistsByAccountIdAndDateAsync(MeterReading meterReading)
		{
			using (var scope = _serviceProvider.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				return await context.MeterReading.AnyAsync(x => x.AccountId == meterReading.AccountId && x.ReadingDateTime == meterReading.ReadingDateTime);
			}
		}

		public async Task<MeterReading> CreateMeterReadingAsync(MeterReading meterReading)
		{
			_context.MeterReading.Add(meterReading);
			await _context.SaveChangesAsync();
			return meterReading;
		}

		public async Task CreateMeterReadingsAsync(List<MeterReading> meterReadings)
		{
			_context.MeterReading.AddRange(meterReadings);
			await _context.SaveChangesAsync();
		}
	}
}