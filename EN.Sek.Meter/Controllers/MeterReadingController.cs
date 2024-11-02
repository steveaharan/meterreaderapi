using EN.Sek.Meter.BLL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace EN.Sek.Meter.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MeterReadingController : ControllerBase
	{
		private readonly IMeterReadingManager _meterReadingManager;
		public MeterReadingController(IMeterReadingManager meterReadingManager)
		{
			_meterReadingManager = meterReadingManager;
		}

		[HttpPost]
		public async Task<IActionResult> MeterReadingUploads(IFormFile meterReadingCSV)
		{
			if (meterReadingCSV == null)
			{
				return BadRequest("No file uploaded");
			}

			try
			{
				var response = await _meterReadingManager.BulkMeterReadingAsync(meterReadingCSV);
				return Ok(response);
			}
			catch
			{
				return StatusCode(500, "Error processing meter reading");
			}
		}
	}
}