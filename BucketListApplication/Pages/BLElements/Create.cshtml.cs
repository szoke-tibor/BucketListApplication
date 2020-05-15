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

//Not yet implemented
namespace BucketListApplication.Pages.BLElements
{
    public class CreateModel : BLElementCategoriesPageModel
	{
        private readonly BucketListApplication.Data.BLContext _context;
		public SelectList DesignSelect { get; set; }
		public SelectList CategorySelect { get; set; }
		public SelectList BLSelect { get; set; }

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
				//Logged user's BucketLists
				var CurrentUsersBucketLists = from bl in _context.BucketLists
											  where bl.UserId == CurrentUserId
											  select bl;
				BLSelect = new SelectList(CurrentUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));

				// Empty collection for the loop
				// foreach (var category in Model.AssignedCategoryDataList)
				// in the Create Razor page.
				var emptyBLElement = new BucketListElement();
				emptyBLElement.ElementCategories = new List<ElementCategory>();

				DesignSelect = new SelectList(_context.Designs, nameof(Models.Design.DesignID), nameof(Models.Design.Name));
				CategorySelect = new SelectList(_context.Categories, nameof(Models.Category.CategoryID), nameof(Models.Category.Name));
				PopulateAssignedCategoryData(_context, emptyBLElement);
				return Page();
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");
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
				"BucketListElement",   // Prefix for form value.
				ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
				_context.BLElements.Add(newBLElement);
				await _context.SaveChangesAsync();
				return RedirectToPage("Index");
			}
			PopulateAssignedCategoryData(_context, newBLElement);
			return Page();
		}
    }
}
