using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public UserCheckModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public int SelectedBLID { get; set; }

		public List<BucketListElement> SelectedBLElements = new List<BucketListElement>();
		public SelectList BLSelect { get; set; }
		public string Title { get; set; }

		public async Task OnGetAsync(string Id)
        {
			//Logged user's userId
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				Title = _context.Users.FirstOrDefault(u => u.Id == Id).FullName + "'s BucketLists";
				//Checked user's BucketLists
				var CheckedUsersBucketLists = from bl in _context.BucketLists
											  where bl.UserId == Id
											  select bl;
				BLSelect = new SelectList(CheckedUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");
		}
		public async Task<IActionResult> OnPostAsync(string Id)
		{
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				var SelectedBucketList = from bl in _context.BucketLists
										 where bl.BucketListID == SelectedBLID
										 select bl;

				Title = _context.Users.FirstOrDefault(u => u.Id == Id).FullName + " - " + SelectedBucketList.FirstOrDefault().Name;
				//Checked user's BucketLists
				var CheckedUsersBucketLists = from bl in _context.BucketLists
											  where bl.UserId == Id
											  select bl;
				BLSelect = new SelectList(CheckedUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));

				//BucketListElements in selected BucketList
				IQueryable<BucketListElement> bucketlistelementsIQ = from ble in _context.BLElements
																	 where ble.BucketListID == SelectedBLID
																	 select ble;
				SelectedBLElements = await bucketlistelementsIQ.AsNoTracking().ToListAsync();
			}
			else
			{
				throw new Exception("Nincs bejelentkezett felhasználó.");
			}

			return Page();
		}
	}
}
