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
	[DataTestMethod]
	[DataRow("AccountId,MeterReadValue,MeterReadingDateTime\n1,10000,2023-01-01T00:00:00", true, "")]
	[DataRow("AccountId,MeterReadValue,MeterReadingDateTime\n1,123,2023-01-01T00:00:00", false, "Invalid meter reading value. It must be exactly 5 numeric characters.")]
	public async Task BulkMeterReadingAsync_ReturnsExpectedResponse(string csvRow, bool isValid, string failReason)
	{
		// Arrange
		var csvContent = csvRow;
		var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
		var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

		var _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
		_mockMeterReadingValidator.Setup(x => x.ValidateMeterReadingAsync(It.IsAny<BulkMeterReadingCSV>())).ReturnsAsync((isValid, failReason));

		var _meterReadingManager = CreateManager(moqMeterReadingValidator: _mockMeterReadingValidator);

		// Act
		var response = await _meterReadingManager.BulkMeterReadingAsync(formFile);

		// Assert
		Assert.IsNotNull(response);
		if (!isValid)
		{
			Assert.AreEqual(0, response.SuccessCount);
			Assert.AreEqual(1, response.FailedCount);
			Assert.IsTrue(response.FailedReadings[0].Contains(failReason));
		}
		else
		{
			Assert.AreEqual(1, response.SuccessCount);
			Assert.AreEqual(0, response.FailedCount);
			Assert.AreEqual(0, response.FailedReadings.Count);
		}
	}

	[DataTestMethod]
	[DataRow("AccountId,MeterReadValue,MeterReadingDateTime\n1,10000,2023-01-01T00:00:00\n2,20000,2023-01-01T00:00:00\n3,30000,2023-01-01T00:00:00", true, "")]
	public async Task BulkMeterReadingAsync_BulkLoad_ReturnsExpectedResponse(string csvRow, bool isValid, string failReason)
	{
		// Arrange
		var csvContent = csvRow;
		var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
		var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

		var _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
		_mockMeterReadingValidator.Setup(x => x.ValidateMeterReadingAsync(It.IsAny<BulkMeterReadingCSV>())).ReturnsAsync((isValid, failReason));

		var _meterReadingManager = CreateManager(moqMeterReadingValidator: _mockMeterReadingValidator);

		// Act
		var response = await _meterReadingManager.BulkMeterReadingAsync(formFile);

		// Assert
		Assert.IsNotNull(response);
		Assert.AreEqual(3, response.SuccessCount);
	}

	private MeterReadingManager CreateManager(
		IMock<IMeterReadingDataProvider> moqMeterReadingDataProvider = null,
		IMock<IMeterReadingValidator> moqMeterReadingValidator = null)
	{
		return new MeterReadingManager(
			moqMeterReadingDataProvider?.Object ?? new Mock<IMeterReadingDataProvider>().Object,
			moqMeterReadingValidator?.Object ?? new Mock<IMeterReadingValidator>().Object);
	}
}