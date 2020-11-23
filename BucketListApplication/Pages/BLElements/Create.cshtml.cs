using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BucketListApplication.Models;
using BucketListApplication.Data;

namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : BLElementCategoriesPageModel
	{
        private readonly BLContext _context;

		[BindProperty]
		public BucketListElement BucketListElement { get; set; }

		public CreateModel(BLContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int bucketListId)
        {
			if (!User.Identity.IsAuthenticated)
				return RedirectToPage("../AuthError");

			BucketListElement = new BucketListElement
			{
				BucketListID = bucketListId,
				BucketList = await _context.BucketLists.FindAsync(bucketListId),
				ElementCategories = new List<ElementCategory>()
			};

			await PopulateAssignedCategoryData(_context, BucketListElement);
			await PopulateBucketListDropDownList(_context);
			return Page();
		}

        public async Task<IActionResult> OnPostAsync(int bucketListId, string[] selectedCategories)
        {
			if (!User.Identity.IsAuthenticated)
				return RedirectToPage("../AuthError");

			var newBLElement = new BucketListElement
			{
				BucketListID = bucketListId,
				Progression = new Progression
				{
					BLETasks = new List<BLETask>()
				}
			};

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
			await PopulateAssignedCategoryData(_context, newBLElement);
			await PopulateBucketListDropDownList(_context, newBLElement.BucketListID);
			return Page();
		}
    }
}
