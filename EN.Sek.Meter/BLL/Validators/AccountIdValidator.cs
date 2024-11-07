using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;
using EN.Sek.Meter.Entities;

namespace EN.Sek.Meter.BLL
{
	public class AccountIdValidator : BaseValidator
	{
		private readonly IAccountDataProvider _accountDataProvider;

		public AccountIdValidator(IAccountDataProvider accountDataProvider)
		{
			_accountDataProvider = accountDataProvider;
		}

		protected override async Task<(bool, string)> PerformValidationAsync(BulkMeterReadingCSV meterReading)
		{
			if (!int.TryParse(meterReading.AccountId, out int accountId))
			{
				return (false, "Invalid account ID.");
			}

			var accountExists = await _accountDataProvider.AccountExists(accountId);
			if (!accountExists)
			{
				return (false, "Account does not exist.");
			}

			return (true, string.Empty);
		}
	}
}