using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Http;

namespace BucketListApplication.Data
{
	public class BLContext : IdentityDbContext<BLUser>
	{
		public BLContext(DbContextOptions<BLContext> options) : base(options)
		{
		}

		public DbSet<BucketList> BucketLists { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ElementCategory> ElementCategories { get; set; }
		public DbSet<Element> Elements { get; set; }
		public DbSet<BucketListElement> BLElements { get; set; }
		public DbSet<Progression> Progressions { get; set; }
		public DbSet<BLETask> BLETasks { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<BucketList>().ToTable("BucketList");
			modelBuilder.Entity<Category>().ToTable("Category");
			modelBuilder.Entity<ElementCategory>().ToTable("ElementCategory");

			modelBuilder.Entity<Progression>().ToTable("Progression")
				.HasOne(p => p.BLElement)
				.WithOne(ble => ble.Progression)
				.HasForeignKey<Progression>(p => p.ElementID);

			modelBuilder.Entity<BLETask>().ToTable("BLETask");

			//Table per Hierarchy -> BucketListElement is included
			modelBuilder.Entity<Element>().ToTable("Element");

			//Pure Join Table
			modelBuilder.Entity<ElementCategory>()
				.HasKey(ec => new { ec.ElementID, ec.CategoryID });
		}
	}
}