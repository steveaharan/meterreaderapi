using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public abstract class BaseValidator : IValidator
	{
		private IValidator _nextValidator;

		public IValidator SetNext(IValidator next)
		{
			_nextValidator = next;
			return next;
		}

		public async Task<(bool, string)> ValidateAsync(BulkMeterReadingCSV meterReading)
		{
			var result = await PerformValidationAsync(meterReading);
			var valid = result.Item1;
			var failReason = result.Item2;
			if (valid && _nextValidator != null)
			{
				return await _nextValidator.ValidateAsync(meterReading);
			}
			return result;
		}

		protected abstract Task<(bool, string)> PerformValidationAsync(BulkMeterReadingCSV meterReading);
	}
}