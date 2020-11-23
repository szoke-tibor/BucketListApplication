using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Interfaces;
using BucketListApplication.Data;

namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : BLElementCategoriesPageModel
	{
        private readonly BLContext _context;
		private readonly IUserService _userService;
		private readonly IBucketListService _bucketListService;

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

			if (bucketListId == null)
				return NotFound();

			BucketListElement = new BucketListElement
			{
				BucketListID = bucketListId.Value,
				BucketList = await _context.BucketLists.FindAsync(bucketListId),
				ElementCategories = new List<ElementCategory>()
			};

			if (_userService.BucketListIsNotBelongingToUser(User, BucketListElement.BucketList))
				return Forbid();

			_bucketListService.PopulateAssignedCategoryData(_context, BucketListElement, ref AssignedCategoryDataList);
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL);
			return Page();
		}

        public async Task<IActionResult> OnPostAsync(int? bucketListId, string[] selectedCategories)
        {
			if (_userService.UserIsNotAuthenticated(User))
				return RedirectToPage("../AuthError");

			var newBLElement = new BucketListElement
			{
				BucketListID = bucketListId.Value,
				BucketList = await _context.BucketLists.FindAsync(bucketListId),
				Progression = new Progression
				{
					BLETasks = new List<BLETask>()
				}
			};

			if (_userService.BucketListIsNotBelongingToUser(User, newBLElement.BucketList))
				return Forbid();

			if (selectedCategories != null)
			{
				newBLElement.ElementCategories = new List<ElementCategory>();
				foreach (var category in selectedCategories)
				{
					var categoryToAdd = new ElementCategory
					{
						CategoryID = int.Parse(category)
					};
					newBLElement.ElementCategories.Add(categoryToAdd);
				}
			}

			// Defense against overposting attacks. Returns true if the update was successful.
			if (await TryUpdateModelAsync<BucketListElement>(newBLElement, "BucketListElement",
				ble => ble.Name,
				ble => ble.Description,
				ble => ble.Completed,
				ble => ble.Visibility,
				ble => ble.Progression))
			{
				await _context.BLElements.AddAsync(newBLElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("../BucketLists/Index", new { bucketListId = bucketListId });
			}
			//If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
			_bucketListService.PopulateAssignedCategoryData(_context, newBLElement, ref AssignedCategoryDataList);
			_bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, newBLElement.BucketListID);
			return Page();
		}
    }
}
