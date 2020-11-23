using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;

namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : BLElementCategoriesPageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;
        private readonly IBucketListService _bucketListService;

        [BindProperty]
        public BucketListElement BucketListElement { get; set; }

        public EditModel(BLContext context, IUserService userService, IBucketListService bucketListService)
        {
            _context = context;
            _userService = userService;
            _bucketListService = bucketListService;
        }

        public async Task<IActionResult> OnGetAsync(int? bucketListElementId)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            BucketListElement = await _context.BLElements
                .AsNoTracking()
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                .FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);

            if (BucketListElement == null)
                return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, BucketListElement))
                return Forbid();


            _bucketListService.PopulateAssignedCategoryData(_context, BucketListElement, ref AssignedCategoryDataList);
            _bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListElementId, string[] selectedCategories)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            var blElementToUpdate = await _context.BLElements
                .Include(ble => ble.BucketList)
                .Include(ble => ble.Progression)
                    .ThenInclude(p => p.BLETasks)
                .Include(ble => ble.ElementCategories)
                    .ThenInclude(ec => ec.Category)
                .FirstOrDefaultAsync(ble => ble.ElementID == bucketListElementId);

            if (blElementToUpdate == null)
                return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, blElementToUpdate))
                return Forbid();

            // Defense against overposting attacks. Returns true if the update was successful.
            if (await TryUpdateModelAsync<BucketListElement>(blElementToUpdate, "BucketListElement",
                ble => ble.Name,
                ble => ble.BucketListID,
                ble => ble.Description,
                ble => ble.Completed,
                ble => ble.Visibility,
                ble => ble.Progression))
			{
                blElementToUpdate.Progression.DeleteEmptyTasks();
                await _bucketListService.UpdateBLElementCategories(_context, selectedCategories, blElementToUpdate);
				await _context.SaveChangesAsync();
                return RedirectToPage("DetailsBLE", new { bucketListElementId = blElementToUpdate.ElementID });
            }

            //If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
            _bucketListService.PopulateAssignedCategoryData(_context, blElementToUpdate, ref AssignedCategoryDataList);
            _bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), ref BucketListSL, blElementToUpdate.BucketListID);
            return Page();
		}
    }
}
