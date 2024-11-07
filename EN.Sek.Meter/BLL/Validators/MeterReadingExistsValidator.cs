using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class MeterReadingExistsValidator : BaseValidator
	{
		private readonly IMeterReadingDataProvider _meterReadingDataProvider;

		public MeterReadingExistsValidator(IMeterReadingDataProvider meterReadingDataProvider)
		{
			_meterReadingDataProvider = meterReadingDataProvider;
		}

		protected override async Task<(bool, string)> PerformValidationAsync(BulkMeterReadingCSV meterReading)
		{
			var readingExists = await _meterReadingDataProvider.MeterReadingExistsByAccountIdAndDateAsync(new MeterReading
			{
				AccountId = int.Parse(meterReading.AccountId),
				ReadingDateTime = DateTime.Parse(meterReading.MeterReadingDateTime)
			});

			if (readingExists)
			{
				return (false, "Meter reading already exists.");
			}

			return (true, string.Empty);
		}
	}
}