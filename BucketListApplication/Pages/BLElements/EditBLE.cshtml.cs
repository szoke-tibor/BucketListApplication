using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BucketListApplication.Models;
using BucketListApplication.Data;
using BucketListApplication.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BucketListApplication.Models.BLViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BucketListApplication.Pages.BLElements
{
    public class EditModel : PageModel
    {
        private readonly BLContext _context;
        private readonly IUserService _userService;
        private readonly IBucketListService _bucketListService;

        public List<AssignedCategoryData> AssignedCategoryDataList;
        public SelectList BucketListSL;

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

            BucketListElement = await _bucketListService.GetBLEByIDWithDetails(_context, bucketListElementId);

            if (BucketListElement == null)
                return NotFound();

            if (_userService.BucketListElementIsNotBelongingToUser(User, BucketListElement))
                return Forbid();

            AssignedCategoryDataList = await _bucketListService.PopulateAssignedCategoryData(_context, BucketListElement);
            BucketListSL = await _bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), false, false);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? bucketListElementId, string[] selectedCategories)
        {
            if (_userService.UserIsNotAuthenticated(User))
                return RedirectToPage("../AuthError");

            if (bucketListElementId == null)
                return NotFound();

            var blElementToUpdate = await _bucketListService.GetBLEByIDWithDetails(_context, bucketListElementId);

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
                _bucketListService.DeleteEmptyTasks(blElementToUpdate.Progression.BLETasks);
                await _bucketListService.UpdateBLElementCategories(_context, selectedCategories, blElementToUpdate);
				await _context.SaveChangesAsync();
                return RedirectToPage("DetailsBLE", new { bucketListElementId = blElementToUpdate.ElementID });
            }

            //If TryUpdateModelAsync fails restore AssignedCategoryDataList and DropDownLists
            AssignedCategoryDataList = await _bucketListService.PopulateAssignedCategoryData(_context, blElementToUpdate);
            BucketListSL = await _bucketListService.PopulateBucketListDropDownList(_context, _userService.GetUserId(User), false, false, blElementToUpdate.BucketListID);
            return Page();
		}
    }
}
