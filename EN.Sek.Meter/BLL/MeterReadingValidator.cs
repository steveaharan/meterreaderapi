using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

		public async Task<(bool, string)> ValidMeterReading(BulkMeterReadingCSV meterReading)
		{
			if (meterReading.MeterReadValue == null || !Regex.IsMatch(meterReading.MeterReadValue, @"^\d{5}$"))
			{
				return (false, "Invalid meter reading value.");
			};

			var readingExists = await MeterReadingNewAsync(meterReading);

			if (readingExists)
			{
				return (false, "Meter reading already exists.");
			};

			var accountExists = await MeterReadingAccountExists(meterReading);

			if (!accountExists)
			{
				return (false, "Account does not exist.");
			};

			return (true, "");
		}

		public async Task<bool> MeterReadingNewAsync(BulkMeterReadingCSV meterReading)
		{
			if (string.IsNullOrEmpty(meterReading.AccountId) || string.IsNullOrEmpty(meterReading.MeterReadingDateTime))
			{
				return false;
			}

			return await _meterReadingDataProvider.MeterReadingExistsByAccountIdAndDateAsync(new MeterReading
			{
				AccountId = int.Parse(meterReading.AccountId),
				ReadingDateTime = DateTime.Parse(meterReading.MeterReadingDateTime)
			});
		}

		public async Task<bool> MeterReadingAccountExists(BulkMeterReadingCSV meterReading)
		{
			if (string.IsNullOrEmpty(meterReading.AccountId))
			{
				return false;
			}

			return await _accountDataProvider.AccountExists(int.Parse(meterReading.AccountId));
		}
	}
}