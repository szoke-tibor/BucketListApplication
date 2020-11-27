using System.Collections.Generic;

namespace BucketListApplication.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }
		public IEnumerable<ElementCategory> ElementCategories { get; set; }
	}
}
