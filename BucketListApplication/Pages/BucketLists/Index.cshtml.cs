using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
    public class IndexModel : BLElementListingPageModel
	{
        private readonly BLContext _context;

        public IndexModel(BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				await PopulateBucketListDropDownList(_context, CurrentUserId, false);
				if (bucketListId != null)
				{
					SelectedBucketList = await _context.BucketLists.FindAsync(bucketListId);
					await PopulateSelectedBLElementsList(_context, (int)bucketListId, false);
				}
				return Page();
			}
			else
				return RedirectToPage("../AuthError");

		}
		public IActionResult OnPost()
		{
			return RedirectToPage("Index", new { bucketListId = SelectedBucketList.BucketListID });
		}
	}
}
