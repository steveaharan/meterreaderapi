namespace EN.Sek.Meter.Entities
{
	public class Account
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		// Navigation property
		public List<MeterReading> MeterReadings { get; set; }
	}
}