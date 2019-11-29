using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BucketListApplication.Data;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Security.Claims;

namespace BucketListApplication.Data
{
	public static class DbInitializer
	{
		public static void Initialize(BLContext context, UserManager<BLUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (context.Elements.Any())
			{
				return;   // DB has been seeded already
			}

			InitializeRoles(roleManager);
			InitializeUsers(userManager);
			InitializeData(context, userManager);
		}

		public static void InitializeRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.RoleExistsAsync("Admin").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "Admin";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}

			if (!roleManager.RoleExistsAsync("User").Result)
			{
				IdentityRole role = new IdentityRole();
				role.Name = "User";
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}
		}

		static void InitializeUsers(UserManager<BLUser> userManager)
		{
			if (userManager.FindByNameAsync("1@gmail.com").Result == null)
			{
				BLUser user = new BLUser();
				user.UserName = "1@gmail.com";
				user.Email = "1@gmail.com";
				user.EmailConfirmed = true;
				user.FullName = "1";

				IdentityResult result = userManager.CreateAsync(user, "Proba123'").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "Admin").Wait();
			}

			if (userManager.FindByNameAsync("2@gmail.com").Result == null)
			{
				BLUser user = new BLUser();
				user.UserName = "2@gmail.com";
				user.Email = "2@gmail.com";
				user.EmailConfirmed = true;
				user.FullName = "2";

				IdentityResult result = userManager.CreateAsync(user, "Proba123'").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "User").Wait();
			}

			if (userManager.FindByNameAsync("3@gmail.com").Result == null)
			{
				BLUser user = new BLUser();
				user.UserName = "3@gmail.com";
				user.Email = "3@gmail.com";
				user.EmailConfirmed = true;
				user.FullName = "3";

				IdentityResult result = userManager.CreateAsync(user, "Proba123'").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "User").Wait();
			}
		}

		static void InitializeData(BLContext context, UserManager<BLUser> userManager)
		{
			//CATEGORIES
			var categories = new Category[]
			{
				new Category { Name = "Sport" },
				new Category { Name = "Family" },
				new Category { Name = "Travel" },
				new Category { Name = "Education" },
				new Category { Name = "Other" }
			};

			foreach (Category c in categories)
			{
				context.Categories.Add(c);
			}
			context.SaveChanges();

			//DESIGNS
			var designs = new Design[]
			//Itt majd még szöszölni kell ezzel
			{
				new Design { PictureURL = "url",    BorderColorARGB = Color.Black.ToArgb(),
					BackgroundColorARGB = Color.White.ToArgb(), BorderType = 0,
					Name = "Default"
				},
				new Design { PictureURL = "url",    BorderColorARGB = Color.Blue.ToArgb(),
					BackgroundColorARGB = Color.Orange.ToArgb(),    BorderType = 0,
					Name = "Random1"
				},
				new Design { PictureURL = "url",    BorderColorARGB = Color.Green.ToArgb(),
					BackgroundColorARGB = Color.Red.ToArgb(),   BorderType = 0,
					Name = "Random2"
				}
			};

			foreach (Design d in designs)
			{
				context.Designs.Add(d);
			}
			context.SaveChanges();

			//BUCKETLISTS
			var bucketlists = new BucketList[]
			{
				new BucketList { Name = "MyEpicBucketList",
					UserId = userManager.Users.Where(u => u.Email == "1@gmail.com").First().Id
				},
				new BucketList { Name = "MyNonEpicBucketList",
					UserId = userManager.Users.Where(u => u.Email == "1@gmail.com").First().Id
				},
				new BucketList { Name = "FamilyList",
					UserId = userManager.Users.Where(u => u.Email == "2@gmail.com").First().Id
				},
				new BucketList { Name = "SportList",
					UserId = userManager.Users.Where(u => u.Email == "2@gmail.com").First().Id
				},
				new BucketList { Name = "LanguageList",
					UserId = userManager.Users.Where(u => u.Email == "3@gmail.com").First().Id
				},
				new BucketList { Name = "TravelList",
					UserId = userManager.Users.Where(u => u.Email == "3@gmail.com").First().Id
				}
			};

			foreach (BucketList bl in bucketlists)
			{
				context.BucketLists.Add(bl);
			}
			context.SaveChanges();

			//ELEMENTS
			var elements = new Element[]
			{
				new Element { Name = "Learn to play curling",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Try parachuting",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Visit Spain",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
			};

			foreach (Element e in elements)
			{
				context.Elements.Add(e);
			}
			context.SaveChanges();

			//BUCKETLISTELEMENTS
			var bucketlistelements = new BucketListElement[]
			{
				new BucketListElement { Name = "Learn 2 different languages",
					Description = "Angol, Spanyol", Completed = true,
					BucketListID = bucketlists.Single( bl => bl.Name == "LanguageList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Learn 3 different languages",
					Description = "Angol, Spanyol, Francia", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "LanguageList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Learn to play Curling",
					Description = "It will be expensive", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "SportList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Try bungee jumping",
					Description = "I will need some courage", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "SportList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Travel to Spain with my family",
					Description = "", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "TravelList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Have 3 children",
					Description = "", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "FamilyList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "ASD",
					Description = "", Completed = false,
					BucketListID = bucketlists.Single( bl => bl.Name == "MyEpicBucketList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Random1").DesignID
				}
			};

			foreach (BucketListElement ble in bucketlistelements)
			{
				context.BLElements.Add(ble);
			}
			context.SaveChanges();

			var elementcategories = new ElementCategory[]
			{
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Learn to play curling").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Try parachuting").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Visit Spain").ElementID,
					CategoryID = categories.Single(c => c.Name == "Travel" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Learn 2 different languages").ElementID,
					CategoryID = categories.Single(c => c.Name == "Education" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Learn 3 different languages").ElementID,
					CategoryID = categories.Single(c => c.Name == "Education" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Learn to play Curling").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Try bungee jumping").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Travel to Spain with my family").ElementID,
					CategoryID = categories.Single(c => c.Name == "Travel" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Travel to Spain with my family").ElementID,
					CategoryID = categories.Single(c => c.Name == "Family" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Have 3 children").ElementID,
					CategoryID = categories.Single(c => c.Name == "Family" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "ASD").ElementID,
					CategoryID = categories.Single(c => c.Name == "Other" ).CategoryID
				}
			};

			foreach (ElementCategory ec in elementcategories)
			{
				context.ElementCategories.Add(ec);
			}
			context.SaveChanges();
		}
	}
}
