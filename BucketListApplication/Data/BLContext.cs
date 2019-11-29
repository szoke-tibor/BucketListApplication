using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Http;

namespace BucketListApplication.Data
{
	public class BLContext : IdentityDbContext<BLUser>
	{
		//For getting logged user
		public readonly IHttpContextAccessor _httpContextAccessor;

		public BLContext(DbContextOptions<BLContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}


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
			//Table per Hierarchy -> BucketListElement is included
			modelBuilder.Entity<Element>().ToTable("Element");

			//Pure Join Table
			modelBuilder.Entity<ElementCategory>()
				.HasKey(ec => new { ec.ElementID, ec.CategoryID });
		}
	}
}