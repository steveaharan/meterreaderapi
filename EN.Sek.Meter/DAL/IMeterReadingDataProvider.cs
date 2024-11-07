using EN.Sek.Meter.Entities;
using Microsoft.EntityFrameworkCore;

namespace EN.Sek.Meter.DAL
{
	public interface IMeterReadingDataProvider
	{
		Task<MeterReading> GetMeterReadingByIdAsync(int id);
		Task<MeterReading> CreateMeterReadingAsync(MeterReading meterReading);
		Task<bool> MeterReadingExistsByAccountIdAndDateAsync(MeterReading meterReading);
		Task CreateMeterReadingsAsync(List<MeterReading> meterReadings);
	}
}