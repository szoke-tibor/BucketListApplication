using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string PictureFileName { get; set; }
		public ICollection<ElementCategory> ElementCategories { get; set; }
	}
}
