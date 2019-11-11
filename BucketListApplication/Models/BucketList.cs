using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class BucketList
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DbSet<Element> Elements { get; set; }

    }
}
