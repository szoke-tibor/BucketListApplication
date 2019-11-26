using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BucketListApplication.Models
{
    public class BucketListElement : Element
    {
		//Ideiglenesen nullable, hogy ne dobjon kivételt, ha list nélkül szeretném létrehozni
		public int? BucketListID { get; set; }
		public string Description { get; set; }
        public bool Completed { get; set; }

		public BucketList BucketList { get; set; }
	}
}
