using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
	public class ElementCategory
	{
		public int ElementCategoryID { get; set; }
		public int ElementID { get; set; }
		public int CategoryID { get; set; }
		
		public Element Element { get; set; }
		public Category Category { get; set; }
	}
}
