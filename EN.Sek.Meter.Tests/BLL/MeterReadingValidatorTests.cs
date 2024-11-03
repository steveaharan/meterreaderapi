using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.DAL;
using Moq;
using EN.Sek.Meter.BLL;
using Microsoft.AspNetCore.Http;
using EN.Sek.Meter.BLL.Models;

[TestClass]
public class MeterReadingValidatorTests
{
	[DataTestMethod]
	[DataRow("12345", "2023-01-01T00:00:00", "1", true, false, true, "")]
	[DataRow("1234", "2023-01-01T00:00:00", "2", true, false, false, "Invalid meter reading value. It must be exactly 5 numeric characters.")]
	[DataRow("xxxxx", "2023-01-01T00:00:00", "3", true, false, false, "Invalid meter reading value. It must be exactly 5 numeric characters.")]
	[DataRow("12345", "2023-01-01T00:00:00", "x", true, false, false, "Invalid account ID.")]
	[DataRow("12345", "2023-01-01T00:00:00", "1", false, false, false, "Account does not exist.")]
	[DataRow("12345", "2023-01-01T00:00:00", "1", true, true, false, "Meter reading already exists.")]
	[DataRow("12345", "3023-01-01T00:00:00", "1", true, false, false, "Reading date and time cannot be in the future.")]
	public async Task ValidateMeterReadingAsync_MeterReadValue_ReturnsCorrectResponse(string meterReadingValue, string meterReadingDateTime, string accountId, bool accountExists, bool meterReadingExists, bool isValid, string failReason)
	{
		// Arrange
		var meterReading = new BulkMeterReadingCSV
		{
			MeterReadValue = meterReadingValue,
			AccountId = accountId,
			MeterReadingDateTime = meterReadingDateTime
		};

		var _mockAccountDataProvider = new Mock<IAccountDataProvider>();
		_mockAccountDataProvider.Setup(x => x.AccountExists(It.IsAny<int>())).ReturnsAsync(accountExists);

		var _mockMeterReadingDataProvider = new Mock<IMeterReadingDataProvider>();
		_mockMeterReadingDataProvider.Setup(x => x.MeterReadingExistsByAccountIdAndDateAsync(It.IsAny<MeterReading>())).ReturnsAsync(meterReadingExists);

		var _meterReadingValidator = CreateManager(moqAccountDataProvider: _mockAccountDataProvider, moqMeterReadingDataProvider: _mockMeterReadingDataProvider);

		// Act
		var result = await _meterReadingValidator.ValidateMeterReadingAsync(meterReading);

		// Assert
		Assert.AreEqual(isValid, result.Item1);
		Assert.AreEqual(failReason, result.Item2);
	}

	private MeterReadingValidator CreateManager(
	IMock<IMeterReadingDataProvider> moqMeterReadingDataProvider = null,
	IMock<IAccountDataProvider> moqAccountDataProvider = null)
	{
		return new MeterReadingValidator(
			moqMeterReadingDataProvider?.Object ?? new Mock<IMeterReadingDataProvider>().Object,
			moqAccountDataProvider?.Object ?? new Mock<IAccountDataProvider>().Object);
	}
}