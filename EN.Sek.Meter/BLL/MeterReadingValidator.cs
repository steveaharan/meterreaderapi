using EN.Sek.Meter.BLL.Models;
using EN.Sek.Meter.DAL;

namespace EN.Sek.Meter.BLL
{
	public class MeterReadingValidator : IMeterReadingValidator
	{
		private readonly IValidator _validatorChain;

		public MeterReadingValidator(IAccountDataProvider accountDataProvider, IMeterReadingDataProvider meterReadingDataProvider)
		{
			_validatorChain = new MeterReadingValueValidator();
			_validatorChain
				.SetNext(new AccountIdValidator(accountDataProvider))
				.SetNext(new ReadingDateTimeValidator())
				.SetNext(new MeterReadingExistsValidator(meterReadingDataProvider));
		}

		public async Task<(bool, string)> ValidateMeterReadingAsync(BulkMeterReadingCSV meterReading)
		{
			return await _validatorChain.ValidateAsync(meterReading);
		}
	}
}