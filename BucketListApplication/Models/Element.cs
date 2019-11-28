using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class Element
    {
        public int ElementID { get; set; }
        public string Name { get; set; }
		public int DesignID { get; set; }

        public Design Design { get; set; }
        public ICollection<ElementCategory> ElementCategories { get; set; }
    }
}
