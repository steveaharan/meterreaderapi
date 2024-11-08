using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public class MeterReadingDataProvider : IMeterReadingDataProvider
	{
		private readonly ApplicationDbContext _context;

		public MeterReadingDataProvider(ApplicationDbContext context)
		{
			_context = context;
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
			return await _context.MeterReading.AnyAsync(x => x.AccountId == meterReading.AccountId && x.ReadingDateTime == meterReading.ReadingDateTime);
		}

		public async Task<MeterReading> CreateMeterReadingAsync(MeterReading meterReading)
		{
			_context.MeterReading.Add(meterReading);
			await _context.SaveChangesAsync();
			return meterReading;
		}
	}
}