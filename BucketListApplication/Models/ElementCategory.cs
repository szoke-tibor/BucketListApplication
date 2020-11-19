namespace BucketListApplication.Models
{
	public class ElementCategory
	{
		public int ElementID { get; set; }
		public int CategoryID { get; set; }

		public Element Element { get; set; }
		public Category Category { get; set; }
	}
}
