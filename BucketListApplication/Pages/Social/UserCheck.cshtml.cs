using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using System.Security.Claims;
using BucketListApplication.Pages.BLElements;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BucketListApplication.Pages.Social
{
    public class UserCheckModel : PageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;
		private readonly IBucketListService _bucketListService;

		public IEnumerable<BucketListElement> SelectedBLElements;
		public SelectList BucketListSL;
		public string Title { get; set; }

		public UserCheckModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
			_userService = userService;
			_bucketListService = bucketListService;
		}

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public async Task<IActionResult> OnGetAsync(string userId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _userService.FindUserByID(_context, userId);
			Title = _bucketListService.SetUserCheckPageTitle(selectedUser);
			BucketListSL = await _bucketListService.PopulateBucketListDropDownListOrderedByNameAsync(_context, selectedUser.Id, true, true);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string userId)
		{
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var selectedUser = await _context.Users.FindAsync(userId);
			Title = _bucketListService.SetUserCheckPageTitle(selectedUser);
			BucketListSL = await _bucketListService.PopulateBucketListDropDownListOrderedByNameAsync(_context, selectedUser.Id, true, true);
			SelectedBLElements = await _bucketListService.PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync(_context, SelectedBucketList.BucketListID, true);
			return Page();
		}
	}
}
