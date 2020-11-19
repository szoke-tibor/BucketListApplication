namespace BucketListApplication.Models
{
	public class BLETask
	{
		public int BLETaskID { get; set; }
		public int ProgressionID { get; set; }
		public string Text { get; set; }
		public bool Completed { get; set; }
		public Progression Progression { get; set; }
	}
}
