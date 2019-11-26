using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BucketListApplication.Models;

namespace BucketListApplication.Data
{
	public class BLContext : IdentityDbContext<BLUser>
	{
		public BLContext(DbContextOptions<BLContext> options) : base(options) { }


		public DbSet<BucketListApplication.Models.BucketList> BucketLists { get; set; }
		public DbSet<BucketListApplication.Models.Category> Categories { get; set; }
		public DbSet<BucketListApplication.Models.Design> Designs { get; set; }
		public DbSet<BucketListApplication.Models.ElementCategory> ElementCategories { get; set; }

		// Elements + BuckeListElements
		public DbSet<BucketListApplication.Models.Element> Elements { get; set; }
		public DbSet<BucketListApplication.Models.BucketListElement> BLElements { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<BucketList>().ToTable("BucketList");
			modelBuilder.Entity<Category>().ToTable("Category");
			modelBuilder.Entity<Design>().ToTable("Design");
			modelBuilder.Entity<ElementCategory>().ToTable("ElementCategory");
			modelBuilder.Entity<Element>().ToTable("Element");
			//modelBuilder.Entity<BucketListElement>().ToTable("BucketListElement");
		}
	}
}