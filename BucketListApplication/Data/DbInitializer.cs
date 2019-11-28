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

namespace BucketListApplication.Data
{
	public static class DbInitializer
	{
		public static void Initialize(BLContext context, UserManager<BLUser> userManager)
		{
			//context.Database.EnsureCreated();

			if (context.Elements.Any())
			{
				return;   // DB has been seeded already
			}
			/*
			//ROLES
			var roles = new string[] { "Admin", "User" };
			foreach (string role in roles)
			{
				if (!context.Roles.Any(r => r.Name == role))
				{
					context.Roles.Add(new IdentityRole(role));
				}
			}

			//USERS
			if (!context.Users.Any(u => u.UserName == "1@gmail.com"))
			{
				var user = new BLUser
				{
					FullName = "1",
					Email = "1@gmail.com",
					UserName = "1@gmail.com",
					PhoneNumber = "+36301112233",
					EmailConfirmed = true,
					PhoneNumberConfirmed = false,
					SecurityStamp = Guid.NewGuid().ToString("D"),
					LockoutEnabled = true,
				};
				user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Proba123'");
				await userManager.CreateAsync(user);
				await userManager.AddToRoleAsync(user, "Admin");
				users[0] = user;
			}

			if (!context.Users.Any(u => u.UserName == "2@gmail.com"))
			{
				var user = new BLUser
				{
					FullName = "2",
					Email = "2@gmail.com",
					UserName = "2@gmail.com",
					PhoneNumber = "+36302223344",
					EmailConfirmed = true,
					PhoneNumberConfirmed = false,
					SecurityStamp = Guid.NewGuid().ToString("D"),
					LockoutEnabled = true,
				};
				user.PasswordHash = userManager.PasswordHasher.HashPassword(user, "Proba123'");
				await userManager.CreateAsync(user);
				await userManager.AddToRoleAsync(user, "User");
				users[1] = user;
			}

			if (!context.Users.Any(u => u.UserName == "3@gmail.com"))
			{
				var user = new BLUser
				{
					FullName = "3",
					Email = "3@gmail.com",
					UserName = "3@gmail.com",
					PhoneNumber = "+36303334455",
					EmailConfirmed = true,
					PhoneNumberConfirmed = false,
					SecurityStamp = Guid.NewGuid().ToString("D"),
					LockoutEnabled = true,

				};
				await userManager.CreateAsync(user);
				await userManager.AddToRoleAsync(user, "User");
				users[2] = user;
			}

			await context.SaveChangesAsync();
			*/
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
				new Design { PictureURL = "url",	BorderColorARGB = Color.Black.ToArgb(),
					BackgroundColorARGB = Color.White.ToArgb(),	BorderType = 0,
					Name = "Default"
				},
				new Design { PictureURL = "url",    BorderColorARGB = Color.Blue.ToArgb(),
					BackgroundColorARGB = Color.Orange.ToArgb(),	BorderType = 0,
					Name = "Random1"
				},
				new Design { PictureURL = "url",    BorderColorARGB = Color.Green.ToArgb(),
					BackgroundColorARGB = Color.Red.ToArgb(),	BorderType = 0,
					Name = "Random2"
				}
			};

			foreach (Design d in designs)
			{
				context.Designs.Add(d);
			}
			context.SaveChanges();

			//BUCKETLISTS
			/*
			var bucketlists = new BucketList[]
			{
				new BucketList { Name = "MyEpicBucketList",
					UserId = users.Single( u => u.FullName == "1").Id
				},
				new BucketList { Name = "MyNonEpicBucketList",
					UserId = users.Single( u => u.FullName == "1").Id
				},
				new BucketList { Name = "FamilyList",
					UserId = users.Single( u => u.FullName == "2").Id
				},
				new BucketList { Name = "SportList",
					UserId = users.Single( u => u.FullName == "2").Id
				},
				new BucketList { Name = "LanguageList",
					UserId = users.Single( u => u.FullName == "3").Id
				},
				new BucketList { Name = "TravelList",
					UserId = users.Single( u => u.FullName == "3").Id
				}
			};*/

			var bucketlists = new BucketList[]
			{
				new BucketList { Name = "MyEpicBucketList",
					UserId = "4f8b1381-8ddb-47a2-8998-6f6207cfdb3e"
				},
				new BucketList { Name = "MyNonEpicBucketList",
					UserId = "4f8b1381-8ddb-47a2-8998-6f6207cfdb3e"
				},
				new BucketList { Name = "FamilyList",
					UserId = "6d901cb7-9da7-4f39-8615-9ca540ab550b"
				},
				new BucketList { Name = "SportList",
					UserId = "6d901cb7-9da7-4f39-8615-9ca540ab550b"
				},
				new BucketList { Name = "LanguageList",
					UserId = "e0e7ff20-36b5-4bf4-a4f3-57a8a4eabf06"
				},
				new BucketList { Name = "TravelList",
					UserId = "e0e7ff20-36b5-4bf4-a4f3-57a8a4eabf06"
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
