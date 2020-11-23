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

		public async Task<IActionResult> OnGetAsync(string userId)
        {
			//Logged user's userId
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if ( currentUserId != null )
			{
				var selectedUser = await _context.Users.FindAsync(userId);
				Title = selectedUser.FullName + " Bakancslistái";
				await PopulateBucketListDropDownList(_context, userId, true);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}

		public async Task<IActionResult> OnPostAsync(string userId)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (currentUserId != null)
			{
				var selectedUser = await _context.Users.FindAsync(userId);
				Title = selectedUser.FullName + " Bakancslistái";
				await PopulateBucketListDropDownList(_context, userId, true);
				await PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, true);
				return Page();
			}
			else
				return RedirectToPage("../AuthError");
		}
	}
}
