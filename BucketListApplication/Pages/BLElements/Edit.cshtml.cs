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

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

        public EditModel(BucketListApplication.Data.BLContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //Logged user's userId
            var CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                if (id == null)
                    return NotFound();

                BucketListElement = await _context.BLElements
                    .Include(ble => ble.BucketList)
                        .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                        .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ble => ble.ElementID == id);

                if (BucketListElement == null)
                    return NotFound();

                PopulateAssignedCategoryData(_context, BucketListElement);
                PopulateBucketListDropDownList(_context);
                return Page();
            }
            else
                return RedirectToPage("../AuthError");
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
                return NotFound();

            var elementToUpdate = await _context.BLElements
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                .FirstOrDefaultAsync(ble => ble.ElementID == id);

            if (elementToUpdate == null)
                return NotFound();

            // Defense against overposting attacks. Returns true if the update was successful.
            if (await TryUpdateModelAsync<BucketListElement>(elementToUpdate, "BucketListElement",
                ble => ble.Name,
                ble => ble.BucketListID,
                ble => ble.Description,
                ble => ble.Completed,
                ble => ble.Visibility,
                ble => ble.Progression))
			{
                elementToUpdate.Progression.DeleteEmptyTasks();
                UpdateBLElementCategories(_context, selectedCategories, elementToUpdate);
				await _context.SaveChangesAsync();
                return RedirectToPage("Index", new { selectedbucketlistid = elementToUpdate.BucketListID });
            }

            //If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
            PopulateAssignedCategoryData(_context, elementToUpdate);
            PopulateBucketListDropDownList(_context, elementToUpdate.BucketListID);
            return Page();
		}
    }
}
