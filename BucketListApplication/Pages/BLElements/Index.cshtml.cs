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

namespace BucketListApplication.Pages.BLElements
{
    public class IndexModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;

        public IndexModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public int SelectedBLID { get; set; }

		IQueryable<BucketList> CurrentUsersBucketLists;
		public List<BucketListElement> SelectedBLElements = new List<BucketListElement>();

		public async Task OnGetAsync()
        {
			//Logged user's userId
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				//Logged user's BucketLists
				CurrentUsersBucketLists = from bl in _context.BucketLists
										  where bl.UserId == CurrentUserId
									      select bl;
				ViewData["BucketList"] = new SelectList(CurrentUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");
			
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				//BucketListElements in selected BucketList
				IQueryable<BucketListElement> bucketlistelementsIQ = from ble in _context.BLElements
																	 where ble.BucketListID == SelectedBLID
																	 select ble;
				SelectedBLElements = await bucketlistelementsIQ.AsNoTracking().ToListAsync();
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");

			return Page();
		}
	}
}
