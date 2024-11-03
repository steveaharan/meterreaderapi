using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class MeterReadingManager : IMeterReadingManager
	{
		private readonly IMeterReadingDataProvider _meterReadingDataProvider;
		private readonly IAccountDataProvider _accountDataProvider;

		public MeterReadingManager(IMeterReadingDataProvider meterReadingDataProvider, IAccountDataProvider accountDataProvider)
		{
			_meterReadingDataProvider = meterReadingDataProvider;
			_accountDataProvider = accountDataProvider;
		}

		public async Task<BulkMeterReadingResponse> BulkMeterReadingAsync(IFormFile meterReadingCSV)
		{
			var response = new BulkMeterReadingResponse();
			var bulkMeterReadingCSVs = new List<BulkMeterReadingCSV>();
			using (var reader = new StreamReader(meterReadingCSV.OpenReadStream()))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				var meterReadings = csv.GetRecords<BulkMeterReadingCSV>();

				foreach (var row in meterReadings)
				{
					var validationResult = await ValidMeterReading(row);
					var isValid = validationResult.Item1;
					var failureReason = validationResult.Item2;

					if (isValid)
					{
						bulkMeterReadingCSVs.Add(row);
						var meterReadingEntity = new MeterReading
						{
							AccountId = int.TryParse(row.AccountId, out int accountId) ? accountId : 0,
							Reading = int.TryParse(row.MeterReadValue, out int reading) ? reading : 0,
							ReadingDateTime = row.MeterReadingDateTime != null ? DateTime.Parse(row.MeterReadingDateTime) : DateTime.MinValue,
						};

						await _meterReadingDataProvider.CreateMeterReadingAsync(meterReadingEntity);
						response.SuccessCount++;
					}
					else
					{
						response.FailedCount++;
						failureReason = $"{failureReason} (AccountId: {row.AccountId}, MeterReadValue: {row.MeterReadValue}, ReadingDateTime: {row.MeterReadingDateTime})";
						response.FailedReadings.Add(failureReason);
					}
				}
			}
			return response;
		}

		public async Task<MeterReading> GetMeterReadingByIdAsync(int id)
		{
			return await _meterReadingDataProvider.GetMeterReadingByIdAsync(id);
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