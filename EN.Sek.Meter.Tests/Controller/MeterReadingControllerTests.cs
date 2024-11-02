using System.Threading.Tasks;
using EN.Sek.Meter.BLL;
using EN.Sek.Meter.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EN.Sek.Meter.Tests.Controller
{
	[TestClass]
	public class MeterReadingControllerTests
	{
		[TestMethod]
		public async Task CallsHealthCheck()
		{
			// Arrange
			var csvContent = "AccountId,MeterReadValue,ReadingDateTime\n1,12345,2023-01-01T00:00:00\n2,67890,2023-01-02T00:00:00";
			var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
			var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "meterReadingCSV", "meterReading.csv");

			// And I have instantiated my service
			var moqMeterReadingManager = new Mock<IMeterReadingManager>();
			var controller = CreateController(moqMeterReadingManager);

			// When I call the controller
			await controller.MeterReadingUploads(formFile);

			// Then the service should have been called
			moqMeterReadingManager.Verify(x => x.BulkMeterReadingAsync(formFile), Times.Once);

			// And the response should be an OK response
			Assert.IsInstanceOfType(controller.MeterReadingUploads(formFile).Result, typeof(OkObjectResult));
		}

		private MeterReadingController CreateController(Mock<IMeterReadingManager> moqMeterReadingManager)
		{
			return new MeterReadingController(moqMeterReadingManager?.Object ?? Mock.Of<IMeterReadingManager>());
		}
	}
}