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

        public List<BucketListElement> BucketListElement = new List<BucketListElement>();

		public async Task OnGetAsync()
        {
			//Logged user's userId
			var userId = _context._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			//Logged user's BucketLists
			IQueryable<BucketList> bucketlistsIQ = from bl in _context.BucketLists select bl;
			bucketlistsIQ = bucketlistsIQ.Where(bl => bl.UserId == userId);
			IList<BucketList> bucketlists = await bucketlistsIQ.AsNoTracking().ToListAsync();

			foreach (BucketList bl in bucketlists)
			{
				//BucketListElements on the actual BucketList
				IQueryable<BucketListElement> bucketlistelementsIQ = from ble in _context.BLElements select ble;
				bucketlistelementsIQ = bucketlistelementsIQ.Where(ble => ble.BucketListID == bl.BucketListID);
				//Adding actual BucketList's BucketListElements to the results
				BucketListElement.AddRange(await bucketlistelementsIQ.AsNoTracking().ToListAsync());
			}
		}
    }
}
