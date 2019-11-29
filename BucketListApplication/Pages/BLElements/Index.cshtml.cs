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
		public string ListSelect { get; set; }

		public List<BucketListElement> BucketListElements = new List<BucketListElement>();
		public List<BucketList> BucketLists = new List<BucketList>();
		public BucketList SelectedBL = new BucketList();

		public async Task OnGetAsync()
        {
			//Logged user's userId
			var userId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( userId != null )
			{
				//Logged user's BucketLists
				IQueryable<BucketList> bucketlistsIQ = from bl in _context.BucketLists select bl;
				bucketlistsIQ = bucketlistsIQ.Where(bl => bl.UserId == userId);
				BucketLists = await bucketlistsIQ.AsNoTracking().ToListAsync();
			}
			else
			{
				throw new Exception("Nincs bejelentkezett felhasználó.");
			}
		}
		public async Task<IActionResult> OnPostAsync()
		{
			var userId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId != null)
			{
				//Logged user's BucketLists
				IQueryable<BucketList> bucketlistsIQ = from bl in _context.BucketLists select bl;
				bucketlistsIQ = bucketlistsIQ.Where(bl => bl.UserId == userId);
				BucketLists = await bucketlistsIQ.AsNoTracking().ToListAsync();

				SelectedBL = BucketLists.Find(bl => bl.Name == ListSelect);

				//BucketListElements in that BucketList
				IQueryable<BucketListElement> bucketlistelementsIQ = from ble in _context.BLElements select ble;
				bucketlistelementsIQ = bucketlistelementsIQ.Where(ble => ble.BucketListID == SelectedBL.BucketListID);
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
