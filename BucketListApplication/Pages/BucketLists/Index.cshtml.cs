using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

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

		public IActionResult OnGet(int? selectedbucketlistid)
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				PopulateBucketListDropDownList(_context, CurrentUserId, false);
				if (selectedbucketlistid != null)
				{
					SelectedBucketList = _context.BucketLists.Single(bl => bl.BucketListID == selectedbucketlistid);
					PopulateSelectedBLElementsList(_context, (int)selectedbucketlistid, false);
				}
				return Page();
			}
			else
				return RedirectToPage("../AuthError");

		}
		public IActionResult OnPost()
		{
			return RedirectToPage("Index", new { selectedbucketlistid = SelectedBucketList.BucketListID });
		}
	}
}
