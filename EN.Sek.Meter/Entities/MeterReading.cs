namespace EN.Sek.Meter.Entities
{
	public class MeterReading
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int Reading { get; set; }
		public DateTime ReadingDateTime { get; set; }

		// Navigation property
		public Account Account { get; set; }
	}
}