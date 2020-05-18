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
				// DB has been seeded already
				return;
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
				user.FullName = "Sovány Áldáska";

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
				user.FullName = "Kardos Pompónia";

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
				user.FullName = "Lakatos Fortunátó";

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
				new Category { Name = "Hobbi" },
				new Category { Name = "Ételek és italok" },
				new Category { Name = "Képeszségek" },
				new Category { Name = "Jótettek" },
				new Category { Name = "Szexuális élmények" },
				new Category { Name = "Tanulmányok" },
				new Category { Name = "Utazás" },
				new Category { Name = "Sport" },
				new Category { Name = "Család és otthon" },
				new Category { Name = "Vagyon" },
				new Category { Name = "Szórakozás és élmények" },
				new Category { Name = "Vallás" },
				new Category { Name = "Karrier" },
				new Category { Name = "Egyéb" }
			};

			context.Categories.AddRange(categories);
			context.SaveChanges();

			//DESIGNS
			var designs = new Design[]
			{
				new Design { PictureURL = "url",    BorderColor = Color.Black,
					BackgroundColor = Color.White, BorderType = BorderType.Regular,
					Name = "Default"
				},
				new Design { PictureURL = "url",    BorderColor = Color.Blue,
					BackgroundColor = Color.Orange,    BorderType = BorderType.Fancy,
					Name = "Random1"
				},
				new Design { PictureURL = "url",    BorderColor = Color.Green,
					BackgroundColor = Color.Red,   BorderType = BorderType.Fancy,
					Name = "Random2"
				}
			};

			context.Designs.AddRange(designs);
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

			context.BucketLists.AddRange(bucketlists);
			context.SaveChanges();

			//ELEMENTS
			var elements = new Element[]
			{
				// Hobbi

				new Element { Name = "Restaurálj valamit",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kerülj be egy zenekarba",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vigyél végig egy számítógépes játékot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nyerj meg egy e-sport bajnokságot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Olvasd el a kedvenc iród össze könyvét",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Publikáltasd egy fotódat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj el egy kaszinóba",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Fess egy tájképet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vezess tankot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a célbalövést éles fegyverrel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Írj naplót",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a sörfőzést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki magad az irodalomban",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nézd meg a kedvenc csapatodat élőben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tervezz egy társasjátékot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj el egy Forma 1-es futamra",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Készíts egy zenei albumot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nézz meg egy Cirque du Soleil előadást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tölts egy teljes napot moziban",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Ételek és italok

				new Element { Name = "Próbáld ki az olasz konyhát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstold meg a Sushit",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstolj meg egy 100 éves Whikeyt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy borkóstolón",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy pálinkakostolón",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki európa jellegzetes alkoholos italait",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstold meg a kaviárt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Egyél házi készítésű sajtokat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstold meg a sült békacombot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstold meg milyen íze van a csigának",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kóstold meg a cápalevest",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Képeszségek

				new Element { Name = "Tanulj meg egy harcművészetet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg faragni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg rajzolni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg síelni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg valamilyen hangszeren játszani",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg autót szerelni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg táncolni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy önfejlesztő tanfolyamon",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szerezz jogosítványt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg lóval ugratni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg fotózni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg videót vágni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg kenyeret sütni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg elektronikus zenét készíteni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg elkészíteni 20 különböző ételt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg pálinkát főzni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg a 3D-s origami hajtogatást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg kertészkedni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg csempézni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Jótettek

				new Element { Name = "Önkénteskedj egy katasztrófa súlytotta területen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Írj egy dalt, mely másokat jobb kedvre derít",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Adakozz a rászorulóknak",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nyiss játszóházat mozgássérült gyerekek számára",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vágasd le a hajad és adományozd rászoruló gyermekeknek",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Mentsd meg valaki életét",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Ápolj valakit míg teljesen fel nem épül",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nyújts lelki támaszt valakinek",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Önkénteskedj vadakat mentő alapítványnál",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél fel az utcán egy szemetet és dobd a kukába",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Kapcsold le mások után a villanyt 20 alkalommal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Adj ételt egy hajléktalannak",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juttass vissza egy talált tárgyat a jogos tulajdonosának",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Adj borravalót 10 alkalommal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Szexuális élmények

				new Element { Name = "Feküdj le egy hírességgel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki az édeshármast",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szeretkezz a természetben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Feküdj le egy külföldivel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbálj ki izgalmas dolgokat nyilvános helyen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szeretkezz egy vízibiciklin",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Csókolj meg egy vadidegent",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szeretkezz a Balatonban",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Teljesítsd egy különös vágyad",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Tanulmányok

				new Element { Name = "Végezd el az orvosi egyetemet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szerezz diplomát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg kínaiul",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj ki Finnországba tanulni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Építs informatikai karriert",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Végezz el egy mérnöki egyetemet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szerezd meg a síoktatói státuszt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Teljesíts egy tárgyat az egyetemen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tanulj meg spanyolul",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Végezd el a mesterképzést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Végezd el a PhD-t",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Írj szakdolgozatot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Végezd el a középiskolát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Légy kitűnő év végén",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Ne bukj meg semmiből",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Utazás

				new Element { Name = "Menj ki spontán a repülőtérre és ülj fel a legkorábbi járatra",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nézd meg a sarki fényt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Látogasd meg a kínai nagy falat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Járd be Európa összes országát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vágj neki az El Caminonak",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Utazz el Ausztráliába",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj el egy világkörüli utazásra",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Látogass meg 3 lélegzetelállító túristalátványosságot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Élj egy évig külföldön",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Sátrazz Izlandon",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tekintsd meg a Grand Canyont",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Végy részt egy portugáliai szörftáborban",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juss el Amerikába",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy road tripen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juss el Disneylandbe",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj el nászútra Erdélybe",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Utazz el egy lakatlan szigetre",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juss el az űrbe",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nézd meg a világ 7 csodáját",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Sport

				new Element { Name = "Fusd le a maratont",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a hegymászást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki az ejtőernyőzést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a bungee jumpingot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a siklóernyőzést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a tandem ugrást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Biciklizd körbe a Földközi-tengert",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Úszd át a Balatont",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Fuss az Ultrabalatonon",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szörfözz",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Utazz kutyaszánnal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nyomj ki 100 kg-ot fekvenyomással",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a snowboardozást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy vitorlásversenyen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Döntsd meg egy sport rekordod",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Család és otthon

				new Element { Name = "Költözz családi házba",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Alapíts családot",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Fogadj örökbe egy kiskutyát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Hosszabb időn keresztül figyelj oda, hogy minőségi időt tölts a szereteiddel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj férhez / Vegyél feleségül valakit",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Építs egy családi házat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Légy anyuka / apuka",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Adj életet 2 gyermeknek",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Válj nagyszülővé",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Találd meg életed szerelmét és öregedj meg vele",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Valósítsd meg a saját álomotthonodat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Alakíts ki rendszeres családi programokat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Vagyon

				new Element { Name = "Vegyél egy saját lakást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy autót",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy motort",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy vitorlást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy önbefűzős cipőt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy karórát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy játékkonzolt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Újítsd fel a ruhatárad",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Újítsd fel a házadat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy medencét",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy VR headsettet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél egy lávalámpát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Szórakozás és élmények

				new Element { Name = "Repülj hőlégballonnal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj el a strand fesztiválra",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juss el Tomorrowlandre",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a búvárkodást",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Ússz delfinekkel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Tarts egy olyan bulit 100 főnek ahol ingyen fogyaszt mindenki",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Juss el a kedvenc előadód koncertjére",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Ússz teljesen áttetsző tengerben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Csinálj valami bolondságot részegen",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Látogass el a legnagyobb vidámparkba",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vezess egy igazi F1-es autót",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Vallás

				new Element { Name = "Lépj be egy gyülekezetbe",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Találd meg Istent",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt elsőáldozáson",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt bérmáláson",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt egy keresztelőn",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Mondj asztali áldást egy éven keresztül minden alkalommal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Olvasd el a Bibliát",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Gyónd meg bűneidet",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Vegyél részt 100 misén",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Karrier

				new Element { Name = "Légy magánvállalkozó",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Nyiss egy klinikát a barátaiddal",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Menj ki külföldre dolgozni",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Légy elismert",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Érj el egy bizonyos fizetést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Részesülj fizetésemelésben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Részesülj előléptetésben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Válj vezetővé",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Válj szakértővé a munkaterületeden belül",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},

				// Egyéb

				new Element { Name = "Tetováltass magadra valamit",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Fesd át a hajad",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Találkozz egy hírességgel",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Aludj a szabad ég alatt",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki a jeges vizes fürdőzést",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Szerepelj egy filmben",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Próbáld ki magad egy teljesen idegen munkában",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Furasd ki a füled",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new Element { Name = "Fehéríttesd ki a fogaidat",
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				}
			};

			context.Elements.AddRange(elements);
			context.SaveChanges();

			//BUCKETLISTELEMENTS
			var bucketlistelements = new BucketListElement[]
			{
				new BucketListElement { Name = "Learn 2 different languages",
					Description = "Angol, Spanyol", Completed = true, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "LanguageList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Learn 3 different languages",
					Description = "Angol, Spanyol, Francia", Completed = false, Visibility = Visibility.Private,
					BucketListID = bucketlists.Single( bl => bl.Name == "LanguageList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Learn to play Curling",
					Description = "It will be expensive", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "SportList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Try bungee jumping",
					Description = "I will need some courage", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "SportList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Travel to Spain with my family",
					Description = "", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "TravelList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "Have 3 children",
					Description = "", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "FamilyList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Default").DesignID
				},
				new BucketListElement { Name = "ASD",
					Description = "", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "MyEpicBucketList").BucketListID,
					DesignID = designs.Single( d => d.Name == "Random1").DesignID
				}
			};

			context.BLElements.AddRange(bucketlistelements);
			context.SaveChanges();

			var elementcategories = new ElementCategory[]
			{
				// Hobbi

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Restaurálj valamit").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kerülj be egy zenekarba").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vigyél végig egy számítógépes játékot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nyerj meg egy e-sport bajnokságot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Olvasd el a kedvenc iród össze könyvét").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Publikáltasd egy fotódat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj el egy kaszinóba").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fess egy tájképet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vezess tankot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a célbalövést éles fegyverrel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Írj naplót").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a sörfőzést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki magad az irodalomban").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nézd meg a kedvenc csapatodat élőben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tervezz egy társasjátékot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj el egy Forma 1-es futamra").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Készíts egy zenei albumot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nézz meg egy Cirque du Soleil előadást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tölts egy teljes napot moziban").ElementID,
					CategoryID = categories.Single(c => c.Name == "Hobbi").CategoryID
				},

				// Ételek és italok

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki az olasz konyhát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstold meg a Sushit").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstolj meg egy 100 éves Whikeyt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy borkóstolón").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy pálinkakostolón").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki európa jellegzetes alkoholos italait").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstold meg a kaviárt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Egyél házi készítésű sajtokat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstold meg a sült békacombot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstold meg milyen íze van a csigának").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kóstold meg a cápalevest").ElementID,
					CategoryID = categories.Single(c => c.Name == "Ételek és italok").CategoryID
				},

				// Képeszségek

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg egy harcművészetet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg faragni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg rajzolni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg síelni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg valamilyen hangszeren játszani").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg autót szerelni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg táncolni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy önfejlesztő tanfolyamon").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szerezz jogosítványt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg lóval ugratni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg fotózni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg videót vágni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg kenyeret sütni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg elektronikus zenét készíteni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg elkészíteni 20 különböző ételt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg pálinkát főzni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg a 3D-s origami hajtogatást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg kertészkedni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg csempézni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Képeszségek").CategoryID
				},

				// Jótettek

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Önkénteskedj egy katasztrófa súlytotta területen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Írj egy dalt, mely másokat jobb kedvre derít").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Adakozz a rászorulóknak").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nyiss játszóházat mozgássérült gyerekek számára").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vágasd le a hajad és adományozd rászoruló gyermekeknek").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Mentsd meg valaki életét").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Ápolj valakit míg teljesen fel nem épül").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nyújts lelki támaszt valakinek").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Önkénteskedj vadakat mentő alapítványnál").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél fel az utcán egy szemetet és dobd a kukába").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Kapcsold le mások után a villanyt 20 alkalommal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Adj ételt egy hajléktalannak").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juttass vissza egy talált tárgyat a jogos tulajdonosának").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Adj borravalót 10 alkalommal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Jótettek").CategoryID
				},

				// Szexuális élmények

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Feküdj le egy hírességgel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki az édeshármast").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szeretkezz a természetben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Feküdj le egy külföldivel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbálj ki izgalmas dolgokat nyilvános helyen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szeretkezz egy vízibiciklin").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Csókolj meg egy vadidegent").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szeretkezz a Balatonban").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Teljesítsd egy különös vágyad").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények").CategoryID
				},

				// Tanulmányok

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végezd el az orvosi egyetemet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szerezz diplomát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg kínaiul").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj ki Finnországba tanulni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Építs informatikai karriert").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végezz el egy mérnöki egyetemet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szerezd meg a síoktatói státuszt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Teljesíts egy tárgyat az egyetemen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg spanyolul").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végezd el a mesterképzést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végezd el a PhD-t").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Írj szakdolgozatot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végezd el a középiskolát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Légy kitűnő év végén").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Ne bukj meg semmiből").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok").CategoryID
				},

				// Utazás

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj ki spontán a repülőtérre és ülj fel a legkorábbi járatra").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nézd meg a sarki fényt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Látogasd meg a kínai nagy falat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Járd be Európa összes országát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vágj neki az El Caminonak").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Utazz el Ausztráliába").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj el egy világkörüli utazásra").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Látogass meg 3 lélegzetelállító túristalátványosságot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Élj egy évig külföldön").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Sátrazz Izlandon").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tekintsd meg a Grand Canyont").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Végy részt egy portugáliai szörftáborban").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juss el Amerikába").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy road tripen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juss el Disneylandbe").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj el nászútra Erdélybe").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Utazz el egy lakatlan szigetre").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juss el az űrbe").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nézd meg a világ 7 csodáját").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás").CategoryID
				},

				// Sport

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fusd le a maratont").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a hegymászást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki az ejtőernyőzést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a bungee jumpingot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a siklóernyőzést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a tandem ugrást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Biciklizd körbe a Földközi-tengert").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Úszd át a Balatont").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fuss az Ultrabalatonon").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szörfözz").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Utazz kutyaszánnal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nyomj ki 100 kg-ot fekvenyomással").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a snowboardozást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy vitorlásversenyen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Döntsd meg egy sport rekordod").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport").CategoryID
				},

				// Család és otthon

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Költözz családi házba").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Alapíts családot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fogadj örökbe egy kiskutyát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Hosszabb időn keresztül figyelj oda, hogy minőségi időt tölts a szereteiddel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj férhez / Vegyél feleségül valakit").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Építs egy családi házat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Légy anyuka / apuka").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Adj életet 2 gyermeknek").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Válj nagyszülővé").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Találd meg életed szerelmét és öregedj meg vele").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Valósítsd meg a saját álomotthonodat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Alakíts ki rendszeres családi programokat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon").CategoryID
				},

				// Vagyon

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy saját lakást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy autót").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy motort").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy vitorlást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy önbefűzős cipőt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy karórát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy játékkonzolt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Újítsd fel a ruhatárad").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Újítsd fel a házadat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy medencét").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy VR headsettet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél egy lávalámpát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vagyon").CategoryID
				},

				// Szórakozás és élmények

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Repülj hőlégballonnal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj el a strand fesztiválra").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juss el Tomorrowlandre").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a búvárkodást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Ússz delfinekkel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tarts egy olyan bulit 100 főnek ahol ingyen fogyaszt mindenki").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Juss el a kedvenc előadód koncertjére").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Ússz teljesen áttetsző tengerben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Csinálj valami bolondságot részegen").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Látogass el a legnagyobb vidámparkba").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vezess egy igazi F1-es autót").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szórakozás és élmények").CategoryID
				},

				// Vallás

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Lépj be egy gyülekezetbe").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Találd meg Istent").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt elsőáldozáson").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt bérmáláson").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy keresztelőn").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Mondj asztali áldást egy éven keresztül minden alkalommal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Olvasd el a Bibliát").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Gyónd meg bűneidet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt 100 misén").ElementID,
					CategoryID = categories.Single(c => c.Name == "Vallás").CategoryID
				},

				// Karrier

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Légy magánvállalkozó").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Nyiss egy klinikát a barátaiddal").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Menj ki külföldre dolgozni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Légy elismert").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Érj el egy bizonyos fizetést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Részesülj fizetésemelésben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Részesülj előléptetésben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Válj vezetővé").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Válj szakértővé a munkaterületeden belül").ElementID,
					CategoryID = categories.Single(c => c.Name == "Karrier").CategoryID
				},

				// Egyéb

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tetováltass magadra valamit").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fesd át a hajad").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Találkozz egy hírességgel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Aludj a szabad ég alatt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki a jeges vizes fürdőzést").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szerepelj egy filmben").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Próbáld ki magad egy teljesen idegen munkában").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Furasd ki a füled").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Fehéríttesd ki a fogaidat").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb").CategoryID
				},

				///////// BucketListElements

				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Learn 2 different languages").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Learn 3 different languages").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok" ).CategoryID
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
					CategoryID = categories.Single(c => c.Name == "Utazás" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Travel to Spain with my family").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Have 3 children").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "ASD").ElementID,
					CategoryID = categories.Single(c => c.Name == "Egyéb" ).CategoryID
				}
			};

			context.ElementCategories.AddRange(elementcategories);
			context.SaveChanges();
		}
	}
}
