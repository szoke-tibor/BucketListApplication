using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using System.Security.Claims;

namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : BLElementCategoriesPageModel
    {
        private readonly BLContext _context;

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

        public EditModel(BLContext context)
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

                //Not the owner tries to edit their BucketListElement
                if (BucketListElement.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                    return Forbid();

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

            //Not the owner tries to edit their BucketListElement
            if (elementToUpdate.BucketList.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return Forbid();

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
                return RedirectToPage("Details", new { id = elementToUpdate.ElementID });
            }

            //If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
            PopulateAssignedCategoryData(_context, elementToUpdate);
            PopulateBucketListDropDownList(_context, elementToUpdate.BucketListID);
            return Page();
		}
    }
}
