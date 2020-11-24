using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : PageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;
		private readonly IBucketListService _bucketListService;

		public List<AssignedCategoryData> AssignedCategoryDataList;
		public SelectList BucketListSL;

		[BindProperty]
		public BucketListElement BucketListElement { get; set; }

		public CreateModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
			_userService = userService;
			_bucketListService = bucketListService;
		}

        public async Task<IActionResult> OnGetAsync(int? bucketListId)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			BucketListElement = await _bucketListService.Initialize(_context, bucketListId);

			if (BucketListElement == null)
				return NotFound();

			if (_userService.BucketListIsNotBelongingToUser(User, BucketListElement.BucketList))
				return Forbid();

			_bucketListService.PopulateAssignedCategoryData(_context, BucketListElement, ref AssignedCategoryDataList);
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, false, false);
			return Page();
		}

        public async Task<IActionResult> OnPostAsync(int? bucketListId, string[] selectedCategories)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			if (bucketListId == null)
				return NotFound();

			var newBLElement = await _bucketListService.Initialize(_context, bucketListId);
			_bucketListService.AddCategoriesToBLE(selectedCategories, newBLElement);

			if (_userService.BucketListIsNotBelongingToUser(User, newBLElement.BucketList))
				return Forbid();

			// Defense against overposting attacks. Returns true if the update was successful.
			if (await TryUpdateModelAsync<BucketListElement>(newBLElement, "BucketListElement",
				ble => ble.Name,
				ble => ble.Description,
				ble => ble.Completed,
				ble => ble.Visibility))
			{
				await _context.BLElements.AddAsync(newBLElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("../BucketLists/Index", new { bucketListId = bucketListId });
			}
			//If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
			_bucketListService.PopulateAssignedCategoryData(_context, newBLElement, ref AssignedCategoryDataList);
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, false, false, newBLElement.BucketListID);
			return Page();
		}
    }
}
