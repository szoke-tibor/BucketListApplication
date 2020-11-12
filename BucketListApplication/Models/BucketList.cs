using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BucketListApplication.Models
{
    public class BucketList
    {
        public int BucketListID { get; set; }
		public string UserId { get; set; }
		[Required(ErrorMessage = "A név megadása kötelező.")]
		public string Name { get; set; }
		public Visibility Visibility { get; set; }
		public BLUser User { get; set; }
        public ICollection<BucketListElement> BLElements { get; set; }

		[NotMapped]
		public int ElementCount
		{
			get
			{
				if (BLElements == null)
					return 0;
				return BLElements.Count();
			}
		}
	}
}
