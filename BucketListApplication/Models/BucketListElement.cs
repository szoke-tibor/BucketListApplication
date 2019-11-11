using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class BucketListElement : Element
    {
        public string Description { get; set; }
        public int ListID { get; set; }
        public bool Completed { get; set; }

        public DbSet<BucketListElement> Constraints { get; set; }
    }
}
