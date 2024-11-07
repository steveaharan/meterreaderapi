using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public interface IValidator
	{
		Task<(bool, string)> ValidateAsync(BulkMeterReadingCSV meterReading);
		IValidator SetNext(IValidator next);
	}
}