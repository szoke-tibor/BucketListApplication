using System.Collections.Generic;

namespace BucketListApplication.Models.BLViewModels
{
	public class CategoryIndexData
	{
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Element> Elements { get; set; }
		public IEnumerable<ElementCategory> ElementCategories { get; set; }
	}
}
