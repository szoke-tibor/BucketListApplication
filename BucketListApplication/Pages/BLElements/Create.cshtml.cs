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
    public class CreateModel : PageModel
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
			}
			else
				throw new Exception("Nincs bejelentkezett felhasználó.");

			DesignSelect = new SelectList(_context.Designs, nameof(Models.Design.DesignID), nameof(Models.Design.Name));
			CategorySelect = new SelectList(_context.Categories, nameof(Models.Category.CategoryID), nameof(Models.Category.Name));
			return Page();
        }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
			ElementCategory ec = new ElementCategory();

			var emptyBucketListElement = new BucketListElement();

			// Defense against overposting attacks. Returns true if the update was successful.
			if (await TryUpdateModelAsync<BucketListElement>(
				emptyBucketListElement,
				"BucketListElement",   // Prefix for form value.
				ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
				_context.Elements.Add(emptyBucketListElement);
				await _context.SaveChangesAsync();

				// Searching the added BLElement to get its ElementID
				BucketListElement addedBLElement = _context.BLElements.Where(bl => bl.Name == emptyBucketListElement.Name)
																	  .Where(bl => bl.DesignID == emptyBucketListElement.DesignID)
																	  .Where(bl => bl.BucketListID == emptyBucketListElement.BucketListID)
																	  .Where(bl => bl.Description == emptyBucketListElement.Description)
																	  .Where(bl => bl.Completed == emptyBucketListElement.Completed)
																	  .Where(bl => bl.Visibility == emptyBucketListElement.Visibility)
																	  .First();

				Category selectedCategory;

				for (int i = 0; i < SelectedCategories.Length; i++ )
				{
					selectedCategory = _context.Categories.Where(c => c.CategoryID == SelectedCategories[i]).First();
					ec.ElementID = addedBLElement.ElementID;
					ec.CategoryID = selectedCategory.CategoryID;
					_context.ElementCategories.Add(ec);
					await _context.SaveChangesAsync();
				}

				return RedirectToPage("Index");
			}

			return Page();
		}
    }
}
