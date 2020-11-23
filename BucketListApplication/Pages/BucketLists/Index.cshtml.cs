using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BucketListApplication.Pages.BLElements
{
    public class IndexModel : BLElementListingPageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;

		public IndexModel(BLContext context, IUserService userService)
        {
            _context = context;
			_userService = userService;
        }

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			await PopulateBucketListDropDownList(_context, _userService.GetUserId(User), false);

			if (bucketListId != null)
			{
				SelectedBucketList = await _context.BucketLists.FindAsync(bucketListId);

				if (SelectedBucketList == null)
					return NotFound();

				if (_userService.BucketListIsNotBelongingToUser(User, SelectedBucketList))
					return Forbid();

				await PopulateSelectedBLElementsList(_context, (int)bucketListId, false);
			}
			return Page();

		}
		public IActionResult OnPost()
		{
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			return RedirectToPage("Index", new { bucketListId = SelectedBucketList.BucketListID });
		}
	}
}
