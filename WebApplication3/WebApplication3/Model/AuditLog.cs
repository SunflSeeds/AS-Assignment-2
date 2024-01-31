namespace AS_Assignment_2_222256B.Model
{
	public class AuditLog
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Action { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
