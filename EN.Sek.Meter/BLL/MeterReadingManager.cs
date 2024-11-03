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
		private readonly IMeterReadingValidator _meterReadingValidator;

		public MeterReadingManager(IMeterReadingDataProvider meterReadingDataProvider, IMeterReadingValidator meterReadingValidator)
		{
			_meterReadingDataProvider = meterReadingDataProvider;
			_meterReadingValidator = meterReadingValidator;
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
					var validationResult = await _meterReadingValidator.ValidateMeterReadingAsync(row);
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
	}
}