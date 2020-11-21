using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;
using BucketListApplication.Pages.BLElements;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : BLElementListingPageModel
	{
        private readonly BLContext _context;

        public UserCheckModel(BLContext context)
        {
            _context = context;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public string Title { get; set; }

		public async Task<IActionResult> OnGetAsync(string Id)
        {
			//Logged user's userId
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( CurrentUserId != null )
			{
				var currentUser = await _context.Users.FindAsync(Id);
				Title = currentUser.FullName + " Bakancslistái";
				await PopulateBucketListDropDownList(_context, Id, true);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}

		public async Task<IActionResult> OnPostAsync(string Id)
		{
			var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				var currentUser = await _context.Users.FindAsync(Id);
				Title = currentUser.FullName + " Bakancslistái";
				await PopulateBucketListDropDownList(_context, Id, true);
				await PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, true);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}
	}
}
