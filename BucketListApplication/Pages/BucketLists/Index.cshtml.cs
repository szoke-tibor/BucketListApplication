using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BucketListApplication.Pages.BLElements
{
    public class IndexModel : PageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;
		private readonly IBucketListService _bucketListService;

		public IEnumerable<BucketListElement> SelectedBLElements;
		public SelectList BucketListSL;

		public IndexModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
			_userService = userService;
			_bucketListService = bucketListService;
		}

		[BindProperty]
		public BucketList SelectedBucketList { get; set; }

		public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			BucketListSL = await _bucketListService.PopulateBucketListDropDownListOrderedByNameAsync(_context, _userService.GetUserId(User), false, true);

			if (bucketListId != null)
			{
				SelectedBucketList = await _bucketListService.GetBLByIDAsync(_context, bucketListId);

				if (SelectedBucketList == null)
					return NotFound();

				if (_userService.BucketListIsNotBelongingToUser(User, SelectedBucketList))
					return Forbid();

				SelectedBLElements = await _bucketListService.PopulateSelectedBLElementsListWithProgressionOrderedByNameAsync(_context, (int)bucketListId, false);
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
