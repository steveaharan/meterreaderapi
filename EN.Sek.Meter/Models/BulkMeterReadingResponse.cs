namespace EN.Sek.Meter.BLL.Models
{
	public class BulkMeterReadingResponse
	{
		public int SuccessCount { get; set; }
		public int FailedCount { get; set; }
		public List<string> FailedReadings { get; set; }
		public BulkMeterReadingResponse()
		{
			FailedReadings = new List<string>();
		}
	}
}