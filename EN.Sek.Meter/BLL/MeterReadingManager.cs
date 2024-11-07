using System.Collections.Concurrent;
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

		private readonly IServiceProvider _serviceProvider;
		public MeterReadingManager(IMeterReadingDataProvider meterReadingDataProvider,
				IMeterReadingValidator meterReadingValidator,
				IServiceProvider serviceProvider)
		{
			_meterReadingDataProvider = meterReadingDataProvider;
			_meterReadingValidator = meterReadingValidator;
			_serviceProvider = serviceProvider;
		}

		public async Task<BulkMeterReadingResponse> BulkMeterReadingAsync(IFormFile meterReadingCSV)
		{
			var response = new BulkMeterReadingResponse();
			var bulkMeterReadingCSVs = new List<BulkMeterReadingCSV>();
			var meterReadingEntities = new ConcurrentBag<MeterReading>();

			using (var reader = new StreamReader(meterReadingCSV.OpenReadStream()))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				csv.Read();
				csv.ReadHeader();
				var meterReadings = csv.GetRecords<BulkMeterReadingCSV>().ToList();

				var tasks = meterReadings.Select(async row =>
				{
					using (var scope = _serviceProvider.CreateScope())
					{
						var meterReadingValidator = scope.ServiceProvider.GetRequiredService<IMeterReadingValidator>();
						var validationResult = await _meterReadingValidator.ValidateMeterReadingAsync(row);
						var isValid = validationResult.Item1;
						var failureReason = validationResult.Item2;

						if (isValid)
						{
							var meterReadingEntity = new MeterReading
							{
								AccountId = int.TryParse(row.AccountId, out int accountId) ? accountId : 0,
								Reading = int.TryParse(row.MeterReadValue, out int reading) ? reading : 0,
								ReadingDateTime = row.MeterReadingDateTime != null ? DateTime.Parse(row.MeterReadingDateTime) : DateTime.MinValue,
							};

							meterReadingEntities.Add(meterReadingEntity);
							response.SuccessCount++;
						}
						else
						{
							response.FailedCount++;
							failureReason = $"{failureReason} (AccountId: {row.AccountId}, MeterReadValue: {row.MeterReadValue}, ReadingDateTime: {row.MeterReadingDateTime})";
							response.FailedReadings.Add(failureReason);
						}
					}
				});

				await Task.WhenAll(tasks);
			}

			using (var scope = _serviceProvider.CreateScope())
			{
				var meterReadingDataProvider = scope.ServiceProvider.GetRequiredService<IMeterReadingDataProvider>();
				await meterReadingDataProvider.CreateMeterReadingsAsync(meterReadingEntities.ToList());
			}
			return response;
		}
	}
}