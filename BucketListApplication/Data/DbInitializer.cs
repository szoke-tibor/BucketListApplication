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
				IdentityRole role = new IdentityRole
				{
					Name = "Admin"
				};
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}

			if (!roleManager.RoleExistsAsync("User").Result)
			{
				IdentityRole role = new IdentityRole
				{
					Name = "User"
				};
				IdentityResult roleResult = roleManager.CreateAsync(role).Result;
			}
		}

		static void InitializeUsers(UserManager<BLUser> userManager)
		{
			if (userManager.FindByNameAsync("1@gmail.com").Result == null)
			{
				BLUser user = new BLUser
				{
					UserName = "1@gmail.com",
					Email = "1@gmail.com",
					EmailConfirmed = true,
					FullName = "Sovány Áldáska",
					SeededUser = true
				};

				IdentityResult result = userManager.CreateAsync(user, "Proba123'").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "Admin").Wait();
			}

			if (userManager.FindByNameAsync("2@gmail.com").Result == null)
			{
				BLUser user = new BLUser
				{
					UserName = "2@gmail.com",
					Email = "2@gmail.com",
					EmailConfirmed = true,
					FullName = "Kardos Pompónia",
					SeededUser = true
				};

				IdentityResult result = userManager.CreateAsync(user, "Proba123'").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "User").Wait();
			}

			if (userManager.FindByNameAsync("3@gmail.com").Result == null)
			{
				BLUser user = new BLUser
				{
					UserName = "3@gmail.com",
					Email = "3@gmail.com",
					EmailConfirmed = true,
					FullName = "Lakatos Fortunátó",
					SeededUser = true
				};

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
				new Category { Name = "Készségek" },
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

			//BUCKETLISTS
			var bucketlists = new BucketList[]
			{
				new BucketList { Name = "Elsődleges céljaim",
					UserId = userManager.Users.Where(u => u.Email == "1@gmail.com").First().Id
				},
				new BucketList { Name = "Másodlagos céljaim",
					UserId = userManager.Users.Where(u => u.Email == "1@gmail.com").First().Id
				},
				new BucketList { Name = "Családi lista",
					UserId = userManager.Users.Where(u => u.Email == "2@gmail.com").First().Id
				},
				new BucketList { Name = "Sportbeli céljaim",
					UserId = userManager.Users.Where(u => u.Email == "2@gmail.com").First().Id
				},
				new BucketList { Name = "Tanulmányi céljaim",
					UserId = userManager.Users.Where(u => u.Email == "3@gmail.com").First().Id
				},
				new BucketList { Name = "Utazási lista",
					UserId = userManager.Users.Where(u => u.Email == "3@gmail.com").First().Id
				}
			};

			context.BucketLists.AddRange(bucketlists);
			context.SaveChanges();

			//BUCKETLISTELEMENTS
			var bucketlistelements = new BucketListElement[]
			{
				new BucketListElement { Name = "Tanulj meg 2 különböző nyelven folyékonyan beszélni",
					Description = "Angol, Spanyol", Completed = true, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Tanulmányi céljaim").BucketListID
				},
				new BucketListElement { Name = "Végezd el az egyetemet",
					Description = "Bukás nélkül, mintatanterv szerint", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Tanulmányi céljaim").BucketListID
				},
				new BucketListElement { Name = "Tanulj meg curlingezni",
					Description = "Nem olcsó sportág, érdemes előtte egy kisebb összeget félre raknom.", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Sportbeli céljaim").BucketListID
				},
				new BucketListElement { Name = "Próbáld ki a bungee jumpingot",
					Description = "Kötnöm kell egy fogadást előtte, hogy még véletlen se gondoljam meg magam fent.", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Sportbeli céljaim").BucketListID
				},
				new BucketListElement { Name = "Csinálj egy spanyolországi családi nyaralást",
					Description = "Látványosságok, melyeket mindenképp meg szeretnénk nézni: ...", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Utazási lista").BucketListID
				},
				new BucketListElement { Name = "Légy 3 gyermek szülője",
					Description = "Ha az egyik gyermekem lány lesz, Karolinának fogom elnevezni.", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Családi lista").BucketListID
				},
				new BucketListElement { Name = "Alapíts családot",
					Description = "Igyekszem majd a lehető legjobb családapa lenni.", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Elsődleges céljaim").BucketListID
				},
				new BucketListElement { Name = "Kalandozz egy moziban a feleségeddel",
					Description = "Mindenképp a hátsó sorba kell majd helyet foglalnunk.", Completed = false, Visibility = Visibility.Private,
					BucketListID = bucketlists.Single( bl => bl.Name == "Elsődleges céljaim").BucketListID
				},
				new BucketListElement { Name = "Tanulj meg 20 különböző főtt ételt elkészíteni",
					Description = "Nagymamám receptkönyve alapján fogok megtanulni.", Completed = false, Visibility = Visibility.Public,
					BucketListID = bucketlists.Single( bl => bl.Name == "Másodlagos céljaim").BucketListID
				}
			};

			context.BLElements.AddRange(bucketlistelements);
			context.SaveChanges();

			//PROGRESSIONS
			var progressions = new Progression[]
			{
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Tanulj meg 2 különböző nyelven folyékonyan beszélni").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Végezd el az egyetemet").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Tanulj meg curlingezni").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Próbáld ki a bungee jumpingot").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Csinálj egy spanyolországi családi nyaralást").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Légy 3 gyermek szülője").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Kalandozz egy moziban a feleségeddel").ElementID
				},
				new Progression {
					ElementID = bucketlistelements.Single( ble => ble.Name == "Tanulj meg 20 különböző főtt ételt elkészíteni").ElementID
				}
			};

			context.Progressions.AddRange(progressions);
			context.SaveChanges();

			//BLETASKS
			var bletasks = new BLETask[]
			{
				new BLETask {
					Text = "Találd meg a párod",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Költözz vele össze",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Jegyezd el",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Házasodj össze vele",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Vállaljatok gyermeket",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Alapíts családot").Progression.ProgressionID,
					Completed = false
				},

				new BLETask {
					Text = "Beszéld meg a pároddal",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Kalandozz egy moziban a feleségeddel").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Keress egy unalmas filmet amire senki sem megy be",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Kalandozz egy moziban a feleségeddel").Progression.ProgressionID,
					Completed = true
				},
				new BLETask {
					Text = "Vedd meg a jegyeket",
					ProgressionID = bucketlistelements.Single( ble => ble.Name == "Kalandozz egy moziban a feleségeddel").Progression.ProgressionID,
					Completed = false
				}
			};

			context.BLETasks.AddRange(bletasks);
			context.SaveChanges();

			//ELEMENTS
			var elements = new Element[]
			{
				// Hobbi

				new Element { Name = "Restaurálj valamit"},
				new Element { Name = "Kerülj be egy zenekarba"},
				new Element { Name = "Vigyél végig egy számítógépes játékot"},
				new Element { Name = "Nyerj meg egy e-sport bajnokságot"},
				new Element { Name = "Olvasd el a kedvenc iród össze könyvét"},
				new Element { Name = "Publikáltasd egy fotódat"},
				new Element { Name = "Menj el egy kaszinóba"},
				new Element { Name = "Fess egy tájképet"},
				new Element { Name = "Vezess tankot"},
				new Element { Name = "Próbáld ki a célbalövést éles fegyverrel"},
				new Element { Name = "Írj naplót"},
				new Element { Name = "Próbáld ki a sörfőzést"},
				new Element { Name = "Próbáld ki magad az irodalomban"},
				new Element { Name = "Nézd meg a kedvenc csapatodat élőben"},
				new Element { Name = "Tervezz egy társasjátékot"},
				new Element { Name = "Menj el egy Forma 1-es futamra"},
				new Element { Name = "Készíts egy zenei albumot"},
				new Element { Name = "Nézz meg egy Cirque du Soleil előadást"},
				new Element { Name = "Tölts egy teljes napot moziban"},

				// Ételek és italok

				new Element { Name = "Próbáld ki az olasz konyhát"},
				new Element { Name = "Kóstold meg a Sushit"},
				new Element { Name = "Kóstolj meg egy 100 éves Whiskeyt"},
				new Element { Name = "Vegyél részt egy borkóstolón"},
				new Element { Name = "Vegyél részt egy pálinkakostolón"},
				new Element { Name = "Próbáld ki európa jellegzetes alkoholos italait"},
				new Element { Name = "Kóstold meg a kaviárt"},
				new Element { Name = "Egyél házi készítésű sajtokat"},
				new Element { Name = "Kóstold meg a sült békacombot"},
				new Element { Name = "Kóstold meg milyen íze van a csigának"},
				new Element { Name = "Kóstold meg a cápalevest"},

				// Készségek

				new Element { Name = "Tanulj meg egy harcművészetet"},
				new Element { Name = "Tanulj meg faragni"},
				new Element { Name = "Tanulj meg rajzolni"},
				new Element { Name = "Tanulj meg síelni"},
				new Element { Name = "Tanulj meg valamilyen hangszeren játszani"},
				new Element { Name = "Tanulj meg autót szerelni"},
				new Element { Name = "Tanulj meg táncolni"},
				new Element { Name = "Vegyél részt egy önfejlesztő tanfolyamon"},
				new Element { Name = "Szerezz jogosítványt"},
				new Element { Name = "Tanulj meg lóval ugratni"},
				new Element { Name = "Tanulj meg fotózni"},
				new Element { Name = "Tanulj meg videót vágni"},
				new Element { Name = "Tanulj meg kenyeret sütni"},
				new Element { Name = "Tanulj meg elektronikus zenét készíteni"},
				new Element { Name = "Tanulj meg elkészíteni 20 különböző ételt"},
				new Element { Name = "Tanulj meg pálinkát főzni"},
				new Element { Name = "Tanulj meg a 3D-s origami hajtogatást"},
				new Element { Name = "Tanulj meg kertészkedni"},
				new Element { Name = "Tanulj meg csempézni"},

				// Jótettek

				new Element { Name = "Önkénteskedj egy katasztrófa súlytotta területen"},
				new Element { Name = "Írj egy dalt, mely másokat jobb kedvre derít"},
				new Element { Name = "Adakozz a rászorulóknak"},
				new Element { Name = "Nyiss játszóházat mozgássérült gyerekek számára"},
				new Element { Name = "Vágasd le a hajad és adományozd rászoruló gyermekeknek"},
				new Element { Name = "Mentsd meg valaki életét"},
				new Element { Name = "Ápolj valakit míg teljesen fel nem épül"},
				new Element { Name = "Nyújts lelki támaszt valakinek"},
				new Element { Name = "Önkénteskedj vadakat mentő alapítványnál"},
				new Element { Name = "Vegyél fel az utcán egy szemetet és dobd a kukába"},
				new Element { Name = "Kapcsold le mások után a villanyt 20 alkalommal"},
				new Element { Name = "Adj ételt egy hajléktalannak"},
				new Element { Name = "Juttass vissza egy talált tárgyat a jogos tulajdonosának"},
				new Element { Name = "Adj borravalót 10 alkalommal"},

				// Szexuális élmények

				new Element { Name = "Feküdj le egy hírességgel"},
				new Element { Name = "Próbáld ki az édeshármast"},
				new Element { Name = "Szeretkezz a természetben"},
				new Element { Name = "Feküdj le egy külföldivel"},
				new Element { Name = "Próbálj ki izgalmas dolgokat nyilvános helyen"},
				new Element { Name = "Szeretkezz egy vízibiciklin"},
				new Element { Name = "Csókolj meg egy vadidegent"},
				new Element { Name = "Szeretkezz a Balatonban"},
				new Element { Name = "Teljesítsd egy különös vágyad"},

				// Tanulmányok

				new Element { Name = "Végezd el az orvosi egyetemet"},
				new Element { Name = "Szerezz diplomát"},
				new Element { Name = "Tanulj meg kínaiul"},
				new Element { Name = "Menj ki Finnországba tanulni"},
				new Element { Name = "Építs informatikai karriert"},
				new Element { Name = "Végezz el egy mérnöki egyetemet"},
				new Element { Name = "Szerezd meg a síoktatói státuszt"},
				new Element { Name = "Teljesíts egy tárgyat az egyetemen"},
				new Element { Name = "Tanulj meg spanyolul"},
				new Element { Name = "Végezd el a mesterképzést"},
				new Element { Name = "Végezd el a PhD-t"},
				new Element { Name = "Írj szakdolgozatot"},
				new Element { Name = "Végezd el a középiskolát"},
				new Element { Name = "Légy kitűnő év végén"},
				new Element { Name = "Ne bukj meg semmiből"},

				// Utazás

				new Element { Name = "Menj ki spontán a repülőtérre és ülj fel a legkorábbi járatra"},
				new Element { Name = "Nézd meg a sarki fényt"},
				new Element { Name = "Látogasd meg a kínai nagy falat"},
				new Element { Name = "Járd be Európa összes országát"},
				new Element { Name = "Vágj neki az El Caminonak"},
				new Element { Name = "Utazz el Ausztráliába"},
				new Element { Name = "Menj el egy világkörüli utazásra"},
				new Element { Name = "Látogass meg 3 lélegzetelállító túristalátványosságot"},
				new Element { Name = "Élj egy évig külföldön"},
				new Element { Name = "Sátrazz Izlandon"},
				new Element { Name = "Tekintsd meg a Grand Canyont"},
				new Element { Name = "Végy részt egy portugáliai szörftáborban"},
				new Element { Name = "Juss el Amerikába"},
				new Element { Name = "Vegyél részt egy road tripen"},
				new Element { Name = "Juss el Disneylandbe"},
				new Element { Name = "Menj el nászútra Erdélybe"},
				new Element { Name = "Utazz el egy lakatlan szigetre"},
				new Element { Name = "Juss el az űrbe"},
				new Element { Name = "Nézd meg a világ 7 csodáját"},

				// Sport

				new Element { Name = "Fusd le a maratont"},
				new Element { Name = "Próbáld ki a hegymászást"},
				new Element { Name = "Próbáld ki az ejtőernyőzést"},
				new Element { Name = "Próbáld ki a bungee jumpingot"},
				new Element { Name = "Próbáld ki a siklóernyőzést"},
				new Element { Name = "Próbáld ki a tandem ugrást"},
				new Element { Name = "Biciklizd körbe a Földközi-tengert"},
				new Element { Name = "Úszd át a Balatont"},
				new Element { Name = "Fuss az Ultrabalatonon"},
				new Element { Name = "Szörfözz"},
				new Element { Name = "Utazz kutyaszánnal"},
				new Element { Name = "Nyomj ki 100 kg-ot fekvenyomással"},
				new Element { Name = "Próbáld ki a snowboardozást"},
				new Element { Name = "Vegyél részt egy vitorlásversenyen"},
				new Element { Name = "Döntsd meg egy sport rekordod"},

				// Család és otthon

				new Element { Name = "Költözz családi házba"},
				new Element { Name = "Alapíts családot"},
				new Element { Name = "Fogadj örökbe egy kiskutyát"},
				new Element { Name = "Hosszabb időn keresztül figyelj oda, hogy minőségi időt tölts a szereteiddel"},
				new Element { Name = "Menj férhez / Vegyél feleségül valakit"},
				new Element { Name = "Építs egy családi házat"},
				new Element { Name = "Légy anyuka / apuka"},
				new Element { Name = "Adj életet 2 gyermeknek"},
				new Element { Name = "Válj nagyszülővé"},
				new Element { Name = "Találd meg életed szerelmét és öregedj meg vele"},
				new Element { Name = "Valósítsd meg a saját álomotthonodat"},
				new Element { Name = "Alakíts ki rendszeres családi programokat"},

				// Vagyon

				new Element { Name = "Vegyél egy saját lakást"},
				new Element { Name = "Vegyél egy autót"},
				new Element { Name = "Vegyél egy motort"},
				new Element { Name = "Vegyél egy vitorlást"},
				new Element { Name = "Vegyél egy önbefűzős cipőt"},
				new Element { Name = "Vegyél egy karórát"},
				new Element { Name = "Vegyél egy játékkonzolt"},
				new Element { Name = "Újítsd fel a ruhatárad"},
				new Element { Name = "Újítsd fel a házadat"},
				new Element { Name = "Vegyél egy medencét"},
				new Element { Name = "Vegyél egy VR headsettet"},
				new Element { Name = "Vegyél egy lávalámpát"},

				// Szórakozás és élmények

				new Element { Name = "Repülj hőlégballonnal"},
				new Element { Name = "Menj el a strand fesztiválra"},
				new Element { Name = "Juss el Tomorrowlandre"},
				new Element { Name = "Próbáld ki a búvárkodást"},
				new Element { Name = "Ússz delfinekkel"},
				new Element { Name = "Tarts egy olyan bulit 100 főnek ahol ingyen fogyaszt mindenki"},
				new Element { Name = "Juss el a kedvenc előadód koncertjére"},
				new Element { Name = "Ússz teljesen áttetsző tengerben"},
				new Element { Name = "Csinálj valami bolondságot részegen"},
				new Element { Name = "Látogass el a legnagyobb vidámparkba"},
				new Element { Name = "Vezess egy igazi F1-es autót"},

				// Vallás

				new Element { Name = "Lépj be egy gyülekezetbe"},
				new Element { Name = "Találd meg Istent"},
				new Element { Name = "Vegyél részt elsőáldozáson"},
				new Element { Name = "Vegyél részt bérmáláson"},
				new Element { Name = "Vegyél részt egy keresztelőn"},
				new Element { Name = "Mondj asztali áldást egy éven keresztül minden alkalommal"},
				new Element { Name = "Olvasd el a Bibliát"},
				new Element { Name = "Gyónd meg bűneidet"},
				new Element { Name = "Vegyél részt 100 misén"},

				// Karrier

				new Element { Name = "Légy magánvállalkozó"},
				new Element { Name = "Nyiss egy klinikát a barátaiddal"},
				new Element { Name = "Menj ki külföldre dolgozni"},
				new Element { Name = "Légy elismert"},
				new Element { Name = "Érj el egy bizonyos fizetést"},
				new Element { Name = "Részesülj fizetésemelésben"},
				new Element { Name = "Részesülj előléptetésben"},
				new Element { Name = "Válj vezetővé"},
				new Element { Name = "Válj szakértővé a munkaterületeden belül"},

				// Egyéb

				new Element { Name = "Tetováltass magadra valamit"},
				new Element { Name = "Fesd át a hajad"},
				new Element { Name = "Találkozz egy hírességgel"},
				new Element { Name = "Aludj a szabad ég alatt"},
				new Element { Name = "Próbáld ki a jeges vizes fürdőzést"},
				new Element { Name = "Szerepelj egy filmben"},
				new Element { Name = "Próbáld ki magad egy teljesen idegen munkában"},
				new Element { Name = "Furasd ki a füled"},
				new Element { Name = "Fehéríttesd ki a fogaidat"}
			};

			context.Elements.AddRange(elements);
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
					ElementID = elements.Single(e => e.Name == "Kóstolj meg egy 100 éves Whiskeyt").ElementID,
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

				// Készségek

				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg egy harcművészetet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg faragni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg rajzolni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg síelni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg valamilyen hangszeren játszani").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg autót szerelni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg táncolni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Vegyél részt egy önfejlesztő tanfolyamon").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Szerezz jogosítványt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg lóval ugratni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg fotózni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg videót vágni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg kenyeret sütni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg elektronikus zenét készíteni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg elkészíteni 20 különböző ételt").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg pálinkát főzni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg a 3D-s origami hajtogatást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg kertészkedni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
				},
				new ElementCategory {
					ElementID = elements.Single(e => e.Name == "Tanulj meg csempézni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek").CategoryID
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
					ElementID = bucketlistelements.Single(e => e.Name == "Tanulj meg 2 különböző nyelven folyékonyan beszélni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Végezd el az egyetemet").ElementID,
					CategoryID = categories.Single(c => c.Name == "Tanulmányok" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Tanulj meg curlingezni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Próbáld ki a bungee jumpingot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Sport" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Csinálj egy spanyolországi családi nyaralást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Utazás" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Csinálj egy spanyolországi családi nyaralást").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Légy 3 gyermek szülője").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Alapíts családot").ElementID,
					CategoryID = categories.Single(c => c.Name == "Család és otthon" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Kalandozz egy moziban a feleségeddel").ElementID,
					CategoryID = categories.Single(c => c.Name == "Szexuális élmények" ).CategoryID
				},
				new ElementCategory {
					ElementID = bucketlistelements.Single(e => e.Name == "Tanulj meg 20 különböző főtt ételt elkészíteni").ElementID,
					CategoryID = categories.Single(c => c.Name == "Készségek" ).CategoryID
				}
			};

			context.ElementCategories.AddRange(elementcategories);
			context.SaveChanges();
		}
	}
}
