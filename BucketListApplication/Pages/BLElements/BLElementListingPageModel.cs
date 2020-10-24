using BucketListApplication.Data;
using BucketListApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
	public class BLElementListingPageModel : PageModel
	{
		public SelectList BucketListSL { get; set; }
		public IEnumerable<BucketListElement> SelectedBLElements { get; set; }

		public void PopulateBucketListDropDownList(BLContext _context, string UserId, object selectedBucketList = null)
		{
			List<SelectListItem> SelectListItems = new List<SelectListItem>();
			SelectListItems.Add(new SelectListItem()
			{
				Text = "--Válassz egy listát--",
				Value = "null"
			});

			var bucketListsQuery = from bl in _context.BucketLists
								   where bl.UserId == UserId
								   orderby bl.Name
								   select bl;

			foreach (BucketList bl in bucketListsQuery)
			{
				SelectListItems.Add(new SelectListItem()
				{
					Text = bl.Name,
					Value = bl.BucketListID.ToString()
				});
			}

			BucketListSL = new SelectList(SelectListItems, "Value", "Text", null);
		}

		public void PopulateSelectedBLElementsList(BLContext _context, int SelectedBucketListID, bool PublicOnly)
		{
			if (PublicOnly)
			{
				SelectedBLElements = _context.BLElements
									.Include(ble => ble.Progression)
									.Include(ble => ble.Progression.BLETasks)
									.Where(ble => ble.BucketListID == SelectedBucketListID)
									.Where(ble => ble.Visibility == Visibility.Public)
									.OrderBy(ble => ble.Name)
									.ToList();
			}
				
			else
			{
				SelectedBLElements = _context.BLElements
									.Include(ble => ble.Progression)
									.Include(ble => ble.Progression.BLETasks)
									.Where(ble => ble.BucketListID == SelectedBucketListID)
									.OrderBy(ble => ble.Name)
									.ToList();
			}
		}
	}
}
