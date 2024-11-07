using EN.Sek.Meter.BLL.Models;

namespace EN.Sek.Meter.BLL
{
	public interface IMeterReadingValidator
	{
		Task<(bool, string)> ValidateMeterReadingAsync(BulkMeterReadingCSV meterReading);
	}
}