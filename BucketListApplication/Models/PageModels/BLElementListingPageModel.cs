using BucketListApplication.Data;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
	public class BLElementListingPageModel : PageModel
	{
		public SelectList BucketListSL { get; set; }
		public IEnumerable<BucketListElement> SelectedBLElements { get; set; }

		public async Task PopulateBucketListDropDownList(BLContext context, string UserId, bool PublicOnly)
		{
			List<SelectListItem> SelectListItems = new List<SelectListItem>();
			SelectListItems.Add(new SelectListItem()
			{
				Text = "--Válassz egy listát--",
				Value = "null"
			});

			IEnumerable<BucketList> BucketListsQuery;

			if (PublicOnly)
			{
				BucketListsQuery = await context.BucketLists
					.AsNoTracking()
					.Where(bl => bl.UserId == UserId)
					.Where(bl => bl.Visibility == Visibility.Public)
					.OrderBy(bl => bl.Name)
					.ToListAsync();
			}
			else
			{
				BucketListsQuery = await context.BucketLists
					.AsNoTracking()
					.Where(bl => bl.UserId == UserId)
					.OrderBy(bl => bl.Name)
					.ToListAsync();
			}

			foreach (BucketList bl in BucketListsQuery)
			{
				SelectListItems.Add(new SelectListItem()
				{
					Text = bl.Name,
					Value = bl.BucketListID.ToString()
				});
			}

			BucketListSL = new SelectList(SelectListItems, "Value", "Text", null);
		}

		public async Task PopulateSelectedBLElementsList(BLContext context, int SelectedBucketListID, bool PublicOnly)
		{
			if (PublicOnly)
			{
				SelectedBLElements = await context.BLElements
					.AsNoTracking()
					.Include(ble => ble.Progression)
					.Include(ble => ble.Progression.BLETasks)
					.Where(ble => ble.BucketListID == SelectedBucketListID)
					.Where(ble => ble.Visibility == Visibility.Public)
					.OrderBy(ble => ble.Name)
					.ToListAsync();
			}
				
			else
			{
				SelectedBLElements = await context.BLElements
					.AsNoTracking()
					.Include(ble => ble.Progression)
					.Include(ble => ble.Progression.BLETasks)
					.Where(ble => ble.BucketListID == SelectedBucketListID)
					.OrderBy(ble => ble.Name)
					.ToListAsync();
			}
		}
	}
}
