using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BucketListApplication.Models
{
    public class Element
    {
        public int ElementID { get; set; }
        [Required(ErrorMessage = "A cél megadása kötelező.")]
        public string Name { get; set; }

        public ICollection<ElementCategory> ElementCategories { get; set; }

		public string Discriminator { get; private set; }
	}
}
