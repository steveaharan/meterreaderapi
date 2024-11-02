using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EN.Sek.Meter.Entities
{
	public class MeterReading
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int Reading { get; set; }
		public DateTime ReadingDateTime { get; set; }

		// Navigation property
		public Account? Account { get; set; }
	}
}