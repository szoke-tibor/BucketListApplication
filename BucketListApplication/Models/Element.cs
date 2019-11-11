using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Design Design { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
