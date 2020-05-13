using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Data;
using BucketListApplication.Models;
using System.Security.Claims;

//Not yet implemented
namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : PageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;
        public SelectList DesignSelect { get; set; }
        public SelectList CategorySelect { get; set; }
        public SelectList BLSelect { get; set; }

        [BindProperty]
        public int[] SelectedCategories { get; set; }
        [BindProperty]
        public BucketListElement BLElement { get; set; }

        public EditModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Logged user's userId
            var CurrentUserId = _context._httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                //Logged user's BucketLists
                var CurrentUsersBucketLists = from bl in _context.BucketLists
                                              where bl.UserId == CurrentUserId
                                              select bl;

                //SelectLists
                BLSelect = new SelectList(CurrentUsersBucketLists, nameof(Models.BucketList.BucketListID), nameof(Models.BucketList.Name));
                DesignSelect = new SelectList(_context.Designs, nameof(Models.Design.DesignID), nameof(Models.Design.Name));
                CategorySelect = new SelectList(_context.Categories, nameof(Models.Category.CategoryID), nameof(Models.Category.Name));

                //Searching for the BLElement
                if (id == null) { return NotFound(); }
                BLElement = await _context.BLElements.FindAsync(id);
                if (BLElement == null) { return NotFound(); }

                //Setting the active Categories in the SelectList
                int CategoryCount = _context.Categories.Count();
                SelectedCategories = new int[CategoryCount];
                var ElementCategories = _context.ElementCategories.Where(ec => ec.ElementID == BLElement.ElementID).ToList();
                for (int i = 0; i < ElementCategories.Count(); i++)
                    SelectedCategories[i] = ElementCategories.ElementAt(i).CategoryID;
            }
            else
                throw new Exception("Nincs bejelentkezett felhasználó.");

            return Page();
        }

		// DONE
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
			var elementToUpdate = await _context.BLElements.FindAsync(id);

			if (elementToUpdate == null)
			{
				return NotFound();
			}

			if (await TryUpdateModelAsync<BucketListElement>(
				elementToUpdate,
				"blelement",
                ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}

            //TODO
            //managing ElementCategories
            //Setting navigation properties

			return Page();
		}

        private bool BLElementExists(int id)
        {
            return _context.BLElements.Any(e => e.ElementID == id);
        }
    }
}
