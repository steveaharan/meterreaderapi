using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class MeterReadingValidator : IMeterReadingValidator
	{
		private readonly IMeterReadingDataProvider _meterReadingDataProvider;
		private readonly IAccountDataProvider _accountDataProvider;

		public MeterReadingValidator(IMeterReadingDataProvider meterReadingDataProvider, IAccountDataProvider accountDataProvider)
		{
			_meterReadingDataProvider = meterReadingDataProvider;
			_accountDataProvider = accountDataProvider;
		}

		public async Task<(bool, string)> ValidateMeterReadingAsync(BulkMeterReadingCSV meterReading)
		{
			if (meterReading.MeterReadValue == null || !Regex.IsMatch(meterReading.MeterReadValue, @"^\d{5}$"))
			{
				return (false, "Invalid meter reading value. It must be exactly 5 numeric characters.");
			}

			if (!int.TryParse(meterReading.AccountId, out int accountId))
			{
				return (false, "Invalid account ID.");
			}

			var accountExists = await _accountDataProvider.AccountExists(accountId);
			if (!accountExists)
			{
				return (false, "Account does not exist.");
			}

			if (!DateTime.TryParse(meterReading.MeterReadingDateTime, out DateTime readingDateTime))
			{
				return (false, "Invalid reading date and time.");
			}

			if (readingDateTime > DateTime.Now)
			{
				return (false, "Reading date and time cannot be in the future.");
			}

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