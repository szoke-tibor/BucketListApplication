using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using System.Security.Claims;
using BucketListApplication.Pages.BLElements;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : BLElementListingPageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;

		public UserCheckModel(BLContext context, IUserService userService)
        {
            _context = context;
			_userService = userService;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public string Title { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _context.Users.FindAsync(userId);
			Title = selectedUser.FullName + " Bakancslistái";
			await PopulateBucketListDropDownList(_context, userId, true);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string userId)
		{
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _context.Users.FindAsync(userId);
			Title = selectedUser.FullName + " Bakancslistái";
			await PopulateBucketListDropDownList(_context, userId, true);
			await PopulateSelectedBLElementsList(_context, SelectedBucketList.BucketListID, true);
			return Page();
		}
	}
}
