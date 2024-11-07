using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class ReadingDateTimeValidator : BaseValidator
	{
		protected override Task<(bool, string)> PerformValidationAsync(BulkMeterReadingCSV meterReading)
		{
			if (!DateTime.TryParse(meterReading.MeterReadingDateTime, out DateTime readingDateTime))
			{
				return Task.FromResult((false, "Invalid reading date and time."));
			}

			if (readingDateTime > DateTime.Now)
			{
				return Task.FromResult((false, "Reading date and time cannot be in the future."));
			}

			return Task.FromResult((true, string.Empty));
		}
	}
}