using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Models
{
    public class BucketList
    {
        public int BucketListID { get; set; }
		public int UserId { get; set; }
        public string Name { get; set; }

		public BucketUser User { get; set; }
        public ICollection<Element> Elements { get; set; }
    }
}
