using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : BLElementCategoriesPageModel
	{
        private readonly BucketListApplication.Data.BLContext _context;

		[BindProperty]
		public int[] SelectedCategories { get; set; }
		[BindProperty]
		public BucketListElement BucketListElement { get; set; }

		public CreateModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
			//Logged user's userId
			var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (CurrentUserId != null)
			{
				// Empty collection for the loop
				// foreach (var category in Model.AssignedCategoryDataList)
				// in the Create Razor page.
				var emptyBLElement = new BucketListElement();
				emptyBLElement.ElementCategories = new List<ElementCategory>();

				PopulateAssignedCategoryData(_context, emptyBLElement);
				PopulateDesignDropDownList(_context);
				PopulateBucketListDropDownList(_context);
				return Page();
			}
			else
				return RedirectToPage("../Index");
		}

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
			var newBLElement = new BucketListElement();

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
			if (await TryUpdateModelAsync<BucketListElement>(
				newBLElement,
				"BucketListElement",
				ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
				_context.BLElements.Add(newBLElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("Index");
			}

			//If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
			PopulateAssignedCategoryData(_context, newBLElement);
			PopulateDesignDropDownList(_context, newBLElement.DesignID);
			PopulateBucketListDropDownList(_context, newBLElement.BucketListID);
			return Page();
		}
    }
}
