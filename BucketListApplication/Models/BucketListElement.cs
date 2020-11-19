using System.ComponentModel.DataAnnotations;

namespace BucketListApplication.Models
{
    public class BucketListElement : Element
    {
		public int BucketListID { get; set; }
		public string Description { get; set; }
        public bool Completed { get; set; }
		public Visibility Visibility { get; set; }

		public BucketList BucketList { get; set; }
		public Progression Progression { get; set; }
	}

	//Later Hidden can be added
	public enum Visibility
	{
		[Display(Name = "Publikus")]
		Public,
		[Display(Name = "Privát")]
		Private
	}
}
