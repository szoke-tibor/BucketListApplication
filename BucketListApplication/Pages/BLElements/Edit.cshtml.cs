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

namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : BLElementCategoriesPageModel
    {
        private readonly BucketListApplication.Data.BLContext _context;
        public SelectList DesignSelect { get; set; }
        public SelectList CategorySelect { get; set; }
        public SelectList BLSelect { get; set; }

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

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
                if (id == null)
                    return NotFound();

                BucketListElement = await _context.BLElements
                    .Include(ble => ble.Design)
                    .Include(ble => ble.ElementCategories)
                        .ThenInclude(ec => ec.Category)
                    .Include(ble => ble.BucketList)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ble => ble.ElementID == id);

                if (BucketListElement == null)
                    return NotFound();

                PopulateAssignedCategoryData(_context, BucketListElement);
                return Page();
            }
            else
                throw new Exception("Nincs bejelentkezett felhasználó.");
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
                return NotFound();

            var elementToUpdate = await _context.BLElements
                    .Include(ble => ble.Design)
                    .Include(ble => ble.ElementCategories)
                        .ThenInclude(ec => ec.Category)
                    .Include(ble => ble.BucketList)
                    .FirstOrDefaultAsync(ble => ble.ElementID == id);

            if (elementToUpdate == null)
                return NotFound();

			if (await TryUpdateModelAsync<BucketListElement>(
				elementToUpdate,
				"BucketListElement",
                ble => ble.Name, ble => ble.DesignID,
                ble => ble.BucketListID, ble => ble.Description, ble => ble.Completed, ble => ble.Visibility))
			{
                UpdateBLElementCategories(_context, selectedCategories, elementToUpdate);
				await _context.SaveChangesAsync();
				return RedirectToPage("./Index");
			}
            UpdateBLElementCategories(_context, selectedCategories, elementToUpdate);
            PopulateAssignedCategoryData(_context, elementToUpdate);
            return Page();
		}
    }
}
