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
		public string ListSelect { get; set; }

		public List<BucketListElement> BucketListElements = new List<BucketListElement>();
		public List<BucketList> BucketLists = new List<BucketList>();
		public BucketList SelectedBL = new BucketList();
		public string Title { get; set; }

		public async Task OnGetAsync(string Id)
        {
			//Logged user's userId
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				Title = _context.Users.FirstOrDefault(u => u.Id == Id).FullName + "'s BucketLists";
				//Checked user's BucketLists
				IQueryable<BucketList> bucketlistsIQ = from bl in _context.BucketLists select bl;
				bucketlistsIQ = bucketlistsIQ.Where(bl => bl.UserId == Id);
				BucketLists = await bucketlistsIQ.AsNoTracking().ToListAsync();
			}
			else
			{
				throw new Exception("Nincs bejelentkezett felhasználó.");
			}
		}
		public async Task<IActionResult> OnPostAsync(string Id)
		{
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				//Checked user's BucketLists
				IQueryable<BucketList> bucketlistsIQ = from bl in _context.BucketLists select bl;
				bucketlistsIQ = bucketlistsIQ.Where(bl => bl.UserId == Id);
				BucketLists = await bucketlistsIQ.AsNoTracking().ToListAsync();

				SelectedBL = BucketLists.Find(bl => bl.Name == ListSelect);
				Title = _context.Users.FirstOrDefault(u => u.Id == Id).FullName + " - " + SelectedBL.Name;

				//BucketListElements in selected BucketList
				IQueryable<BucketListElement> bucketlistelementsIQ = from ble in _context.BLElements select ble;
				bucketlistelementsIQ = bucketlistelementsIQ.Where(ble =>	ble.BucketListID == SelectedBL.BucketListID && 
																			ble.Visibility == Visibility.Public);
				BucketListElements = await bucketlistelementsIQ.AsNoTracking().ToListAsync();
			}
			else
			{
				throw new Exception("Nincs bejelentkezett felhasználó.");
			}

			return Page();
		}
	}
}
