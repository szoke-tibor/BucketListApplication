using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models.BLViewModels
{
	public class CategoryIndexData
	{
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<Element> Elements { get; set; }
		public IEnumerable<ElementCategory> ElementCategories { get; set; }
	}
}
