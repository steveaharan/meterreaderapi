using Microsoft.EntityFrameworkCore;
using EN.Sek.Meter.Entities;
using EN.Sek.Meter.DAL;
using Moq;
using EN.Sek.Meter.BLL;
using Microsoft.AspNetCore.Http;
using EN.Sek.Meter.BLL.Models;

[TestClass]
public class MeterReadingManagerTests
{
	[TestMethod]
	public async Task BulkMeterReadingAsync_ValidCSV_ReturnsExpectedResponse()
	{
		// Arrange
		var csvContent = "AccountId,MeterReadValue,MeterReadingDateTime\n1,10000,2023-01-01T00:00:00\n2,20000,2023-01-02T00:00:00";
		var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
		var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

		var _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
		_mockMeterReadingValidator.Setup(x => x.ValidMeterReading(It.IsAny<BulkMeterReadingCSV>())).ReturnsAsync((true, string.Empty));

		var _mockMeterReadingDataProvider = new Mock<IMeterReadingDataProvider>();
		_mockMeterReadingDataProvider.Setup(x => x.MeterReadingExistsByAccountIdAndDateAsync(It.IsAny<MeterReading>())).ReturnsAsync(false);

		var _meterReadingManager = CreateManager(moqMeterReadingValidator: _mockMeterReadingValidator, moqMeterReadingDataProvider: _mockMeterReadingDataProvider);

		// Act
		var response = await _meterReadingManager.BulkMeterReadingAsync(formFile);

		// Assert
		Assert.IsNotNull(response);
		Assert.AreEqual(2, response.SuccessCount);
	}

	[TestMethod]
	public async Task BulkMeterReadingAsync_InvalidRow_ReturnsExpectedResponse()
	{
		// Arrange
		var csvContent = "AccountId,MeterReadValue,MeterReadingDateTime\n1,VOID,2023-01-01T00:00:00\n2,200,2023-01-02T00:00:00";
		var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
		var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

		var _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
		_mockMeterReadingValidator.Setup(x => x.ValidMeterReading(It.IsAny<BulkMeterReadingCSV>())).ReturnsAsync((false, "Invalid meter reading value."));

		var _mockMeterReadingDataProvider = new Mock<IMeterReadingDataProvider>();
		_mockMeterReadingDataProvider.Setup(x => x.MeterReadingExistsByAccountIdAndDateAsync(It.IsAny<MeterReading>())).ReturnsAsync(false);

		var _meterReadingManager = CreateManager(moqMeterReadingValidator: _mockMeterReadingValidator, moqMeterReadingDataProvider: _mockMeterReadingDataProvider);

		// Act
		var response = await _meterReadingManager.BulkMeterReadingAsync(formFile);

		// Assert
		Assert.IsNotNull(response);
		Assert.AreEqual(0, response.SuccessCount);
	}

	[TestMethod]
	public async Task MeterReadingExistsByAccountIdAndDateAsync_ValidMeterReading_ReturnsTrue()
	{
		var csvContent = "AccountId,MeterReadValue,MeterReadingDateTime\n1,10000,2023-01-01T00:00:00\n2,10000,2023-01-02T00:00:00";
		var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
		var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

		var _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
		_mockMeterReadingValidator.Setup(x => x.ValidMeterReading(It.IsAny<BulkMeterReadingCSV>())).ReturnsAsync((true, string.Empty));

		var _mockMeterReadingDataProvider = new Mock<IMeterReadingDataProvider>();
		_mockMeterReadingDataProvider.Setup(x => x.MeterReadingExistsByAccountIdAndDateAsync(It.IsAny<MeterReading>())).ReturnsAsync(true);

		var _meterReadingManager = CreateManager(moqMeterReadingValidator: _mockMeterReadingValidator, moqMeterReadingDataProvider: _mockMeterReadingDataProvider);

		// Act
		var response = await _meterReadingManager.BulkMeterReadingAsync(formFile);

		// Assert
		Assert.IsNotNull(response);
		Assert.AreEqual(0, response.SuccessCount);
	}

	private MeterReadingManager CreateManager(
		IMock<IMeterReadingDataProvider>? moqMeterReadingDataProvider = null,
		IMock<IMeterReadingValidator>? moqMeterReadingValidator = null)
	{
		return new MeterReadingManager(
			moqMeterReadingDataProvider?.Object ?? new Mock<IMeterReadingDataProvider>().Object,
			moqMeterReadingValidator?.Object ?? new Mock<IMeterReadingValidator>().Object);
	}
}