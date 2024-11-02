using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public interface IMeterReadingManager
	{
		Task<BulkMeterReadingResponse> BulkMeterReadingAsync(IFormFile meterReadingCSV);
	}
}