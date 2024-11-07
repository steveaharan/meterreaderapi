using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class MeterReadingValueValidator : BaseValidator
	{
		protected override Task<(bool, string)> PerformValidationAsync(BulkMeterReadingCSV meterReading)
		{
			if (meterReading.MeterReadValue == null || !Regex.IsMatch(meterReading.MeterReadValue, @"^\d{5}$"))
			{
				return Task.FromResult((false, "Invalid meter reading value. It must be exactly 5 numeric characters."));
			}
			return Task.FromResult((true, string.Empty));
		}
	}
}